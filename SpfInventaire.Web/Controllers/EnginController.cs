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
    [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
    public class EnginController : Controller
    {
        private IEnginService enginService;
        private ILoggerService logService;

        public EnginController(IEnginService enginService, ILoggerService logService)
        {
            this.enginService = enginService;
            this.logService = logService;
        }


        public ActionResult Index()
        {
            ViewBag.isAdmin = User.IsInRole(Constantes.ROLE_ADMINISTRATEUR);

            return View(this.enginService.ListEngins());
        }

        public ActionResult IndexPlein(int ID)
        {
            ViewBag.isAdmin = User.IsInRole(Constantes.ROLE_ADMINISTRATEUR);

            return View(this.enginService.ListPleinByEnginID(ID));
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Engin manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Engin unEngin = this.enginService.GetEnginById(id);
            if (unEngin == null)
            {
                return HttpNotFound();
            }
            return View(unEngin);
        }

        public ActionResult CreatePleinEngin(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Engin manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Engin unEngin = this.enginService.GetEnginById(id);
            if (unEngin == null)
            {
                return HttpNotFound();
            }

            PleinEssence unPlein = new PleinEssence();
            unPlein.EnginID = unEngin.ID;
            unPlein.Engin = unEngin;
            return View(this.GetFormPleinEngin(false, unPlein));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePleinEngin([Bind(Include = "Kilometrage,Litrage,Prix,EnginID")] PleinEssence unPlein)
        {
            FormPleinEnginViewModel formModel = this.GetFormPleinEngin(false, unPlein);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.enginService.InsertPlein(unPlein, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.PleinEssence, unPlein.ID, "Création d'un Plein Essence", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }



        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Create()
        {
            FormEnginViewModel formModel = this.GetFormEngin(false);
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nom,Numero,Immatriculation,CodeConf,CodeChauff")] Engin unEngin)
        {
            FormEnginViewModel formModel = this.GetFormEngin(false, unEngin);
            if (ModelState.IsValid)
            {
                //insertion du blocInventaire
                ActionControllerResult result = this.enginService.InsertEngin(unEngin, User.Identity.GetUserId());
                //si erreur on affiche un message
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Engin, unEngin.ID, "Création d'un Engin", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Engin manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Engin unEngin = this.enginService.GetEnginById(id);
            if (unEngin == null)
            {
                return HttpNotFound();
            }
            FormEnginViewModel formModel = this.GetFormEngin(true, unEngin);
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nom,Numero,Immatriculation,CodeConf,CodeChauff,DateCreation,DateModification")] Engin unEngin)
        {
            FormEnginViewModel formModel = this.GetFormEngin(true, unEngin);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.enginService.UpdateEngin(unEngin, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Engin, unEngin.ID, "Modification d'un Engin", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult EditPlein(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Plein manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            PleinEssence unPlein = this.enginService.GetPleinById(id);
            if (unPlein == null)
            {
                return HttpNotFound();
            }
            FormPleinEnginViewModel formModel = this.GetFormPleinEngin(true, unPlein);
            return View(formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlein([Bind(Include = "ID,Kilometrage,Litrage,Prix,EnginID")] PleinEssence unPlein)
        {
            FormPleinEnginViewModel formModel = this.GetFormPleinEngin(true, unPlein);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.enginService.UpdatePlein(unPlein, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.PleinEssence, unPlein.ID, "Modification d'un Pleind'essence", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }


        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Engin manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Engin unEngin = this.enginService.GetEnginById(id);
            if (unEngin == null)
            {
                return HttpNotFound();
            }
            return View(unEngin);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.enginService.DeleteEngin(id);
            return RedirectToAction("Index");
        }


        private FormEnginViewModel GetFormEngin(bool isEdit, Engin unEngin = null)
        {
            FormEnginViewModel formModel = new FormEnginViewModel();

            formModel.unEngin = unEngin;
            formModel.isEdit = isEdit;
            return formModel;
        }

        private FormPleinEnginViewModel GetFormPleinEngin(bool isEdit, PleinEssence unPlein = null)
        {
            FormPleinEnginViewModel formModel = new FormPleinEnginViewModel();

            if(unPlein.Engin == null && unPlein.EnginID > 0)
            {
                unPlein.Engin = this.enginService.GetEnginById(unPlein.EnginID);
            }

            formModel.unPlein = unPlein;
            formModel.isEdit = isEdit;
            return formModel;
        }

    }
}
