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
using SpfInventaire.Core.BLL;
using static SpfInventaire.Core.BLL.Constantes;
using Microsoft.AspNet.Identity;
using SpfInventaire.Core.DAL.ViewModels;

namespace SpfInventaire.Web.Controllers
{
    [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
    public class EvenementController : Controller
    {
        private IEvenementService evenementService;
        private ILoggerService logService;

        public EvenementController(IEvenementService evenementService, ILoggerService logService)
        {
            this.evenementService = evenementService;
            this.logService = logService;
        }

        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.evenementService.GetEvenements());
        }

        // GET: Evenement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Evenement manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Evenement unEvenement = this.evenementService.GetEvenementById(id);
            if (unEvenement == null)
            {
                return HttpNotFound();
            }
            return View(unEvenement);
        }

        public ActionResult Create()
        {
            FormEvenementViewModels formModel = this.GetFormEvenement(false);
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Description,Duree,DateEvenement")] Evenement unEvenement)
        {
            FormEvenementViewModels formModel = this.GetFormEvenement(false, unEvenement);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.evenementService.InsertEvenement(unEvenement, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Evenement, null, "Création d'un événement", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Evenement manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Evenement unEvenement = this.evenementService.GetEvenementById(id);
            if (unEvenement == null)
            {
                return HttpNotFound();
            }
            FormEvenementViewModels formModel = this.GetFormEvenement(true, unEvenement);
            return View(formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Description,Duree,DateEvenement, DateCreation,DateModification")] Evenement unEvenement)
        {
            FormEvenementViewModels formModel = this.GetFormEvenement(true, unEvenement);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.evenementService.UpdateEvenement(unEvenement, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Evenement, unEvenement.ID, "Modification d'un événement", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Evenement manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Evenement unEvenement = this.evenementService.GetEvenementById(id);
            if (unEvenement == null)
            {
                return HttpNotFound();
            }
            return View(unEvenement);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.evenementService.DeleteEvenement(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.Evenement, id, "Suppression d'un événement", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }




        private FormEvenementViewModels GetFormEvenement(bool isEdit, Evenement unEvenement = null)
        {
            FormEvenementViewModels formModel = new FormEvenementViewModels();
            formModel.unEvenement = unEvenement;

            formModel.isEdit = isEdit;
            return formModel;
        }


    }
}
