using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpfInventaire.Core.DAL;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.ViewModels;
using static SpfInventaire.Core.BLL.Constantes;
using Microsoft.AspNet.Identity;
using SpfInventaire.Core.BLL;

namespace SpfInventaire.Web.Controllers
{
    [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
    public class TypeMaterielController : Controller
    {
        private ITypeMaterielService typeMaterielService;
        private ILoggerService logService;

        public TypeMaterielController(ITypeMaterielService typeMaterielService, ILoggerService logService)
        {
            this.typeMaterielService = typeMaterielService;
            this.logService = logService;
        }


        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.typeMaterielService.GetTypeMateriels());
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Type manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            TypeMateriel unType = this.typeMaterielService.GetTypeMaterielById(id);
            if (unType == null)
            {
                return HttpNotFound();
            }

            return View(unType);
        }


        public ActionResult Create()
        {
            FormTypeMaterielViewModels formModel = this.GetFormTypeMateriel(false);
            return View(formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nom,Domaine")] TypeMateriel unTypeMateriel)
        {
            FormTypeMaterielViewModels formModel = this.GetFormTypeMateriel(false, unTypeMateriel);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.typeMaterielService.InsertTypeMateriel(unTypeMateriel, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.TypeMateriel, unTypeMateriel.ID, "Création d'un Type de Matériel", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Type Materiel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            TypeMateriel unTypeMateriel = this.typeMaterielService.GetTypeMaterielById(id);
            if(unTypeMateriel == null)
            {
                return HttpNotFound();
            }
            FormTypeMaterielViewModels formModel = this.GetFormTypeMateriel(true, unTypeMateriel);
            return View(formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nom,Domaine")] TypeMateriel unTypeMateriel)
        {
            FormTypeMaterielViewModels formModel = this.GetFormTypeMateriel(true, unTypeMateriel);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.typeMaterielService.UpdateTypeMateriel(unTypeMateriel, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.TypeMateriel, unTypeMateriel.ID, "Modification d'un Type de Matériel", null, User.Identity.GetUserId());

                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.typeMaterielService.DeleteTypeMateriel(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.TypeMateriel, id, "Suppression d'un Type de Matériel", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }


        public ActionResult GenererTypeAuto()
        {
            string messageError = "";
            ActionControllerResult result = this.typeMaterielService.GenerationTypeMaterielFromMateriel();
            if (result == ActionControllerResult.FAILURE)
            {
                messageError = "Une erreur est survenue lors de la génération des Types de Matériel";
            }
            return RedirectToAction("Index", new { MessageErreur = messageError});
        }


        private FormTypeMaterielViewModels GetFormTypeMateriel(bool isEdit, TypeMateriel unTypeMateriel = null)
        {
            FormTypeMaterielViewModels model = new FormTypeMaterielViewModels();
            model.unTypeMateriel = unTypeMateriel;
            model.isEdit = isEdit;
            return model;
        }


    }
}
