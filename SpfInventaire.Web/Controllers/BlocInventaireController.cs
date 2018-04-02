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
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class BlocInventaireController : Controller
    {
        private IBlocInventaireService blocInventaireService;
        private IInventaireService inventaireService;
        private ILoggerService logService;

        public BlocInventaireController(IBlocInventaireService blocInventaireService, IInventaireService inventaireService, ILoggerService logService)
        {
            this.blocInventaireService = blocInventaireService;
            this.inventaireService = inventaireService;
            this.logService = logService;
        }

        // GET: BlocInventaire
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }

            if (Session["isFromInventaire"] != null)
            {
                Session.Remove("isFromInventaire");
            }

            return View(this.blocInventaireService.GetBlocsInventaire());
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetBlocsInventaireByInventaire(int inventaireId)
        {
            SelectList listBlocInventaire = new SelectList(this.blocInventaireService.GetBlocsInventaireByInventaire(inventaireId), "Id", "Nom");
            return Json(listBlocInventaire);
        }

        // GET: BlocInventaire/Details/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Details(int? id, bool? isFromInventaire)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Bloc Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            BlocInventaire unBlocInventaire = this.blocInventaireService.GetBlocInventaireById(id);
            if (unBlocInventaire == null)
            {
                return HttpNotFound();
            }

            //Gestion session isFromInventaire
            if (isFromInventaire != null && isFromInventaire == true)
            {
                if (Session["isFromInventaire"] == null)
                {
                    Session.Add("isFromInventaire", true);
                }
            }

            //Gestion Session isFromBlocInventaire
            if (Session["isFromBlocInventaire"] != null)
            {
                Session.Remove("isFromBlocInventaire");
            }
            this.IsFromInventaire();
            return View(unBlocInventaire);
        }

        // GET: BlocInventaire/Create
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create()
        {
            FormBlocInventaireViewModels formModel = GetFormBlocInventaire(false);
            return View("Create", formModel);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult GetFormCreate(int? inventaireId = null)
        {
            FormBlocInventaireViewModels formModel = GetFormBlocInventaire(false, inventaireId: inventaireId);
            return PartialView("_AjaxAddBlocInventaire", formModel);
        }

        // POST: BlocInventaire/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create(BlocInventaire unBlocInventaire)
        {
            FormBlocInventaireViewModels formModel = this.GetFormBlocInventaire(false, unBlocInventaire);
            if (ModelState.IsValid)
            {
                //insertion du blocInventaire
                ActionControllerResult result = this.blocInventaireService.InsertBlocInventaire(unBlocInventaire, User.Identity.GetUserId());
                //si erreur on affiche un message
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.BlocInventaire, unBlocInventaire.ID, "Création d'un bloc Inventaire", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult AjaxCreate(BlocInventaire unBlocInventaire)
        {
            FormBlocInventaireViewModels formModel = this.GetFormBlocInventaire(false, unBlocInventaire);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.blocInventaireService.InsertBlocInventaire(unBlocInventaire, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuBlocInventaire", formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.BlocInventaire, unBlocInventaire.ID, "Création d'un bloc Inventaire", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuBlocInventaire", formModel);
        }


        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit(int? id)
        {
            if (id == null && !Request.IsAjaxRequest())
            {
                string ErrorMessage = "Id Bloc-Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            BlocInventaire unBlocInventaire = this.blocInventaireService.GetBlocInventaireById(id);
            if (unBlocInventaire == null && !Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }

            this.IsFromInventaire();

            FormBlocInventaireViewModels formModel = this.GetFormBlocInventaire(true, unBlocInventaire);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxEditBlocInventaire", formModel);
            }
            else
            {
                return View(formModel);
            }
        }

        // POST: BlocInventaire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit([Bind(Include = "ID,Nom,Rank,Active,DateCreation,DateModification,InventaireID")] BlocInventaire unBlocInventaire)
        {
            FormBlocInventaireViewModels formModel = this.GetFormBlocInventaire(true, unBlocInventaire);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.blocInventaireService.UpdateBlocInventaire(unBlocInventaire, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;

                    this.IsFromInventaire();
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.BlocInventaire, unBlocInventaire.ID, "Modification d'un bloc Inventaire", null, User.Identity.GetUserId());

                if (Session["isFromInventaire"] != null)
                {
                    if (Convert.ToBoolean(Session["isFromInventaire"]))
                    {
                        return RedirectToAction("Details", "BlocInventaire", new { id = unBlocInventaire.ID, isFromInventaire = true });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            this.IsFromInventaire();
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult AjaxEdit([Bind(Include = "ID,Nom,Rank,Active,DateCreation,DateModification,InventaireID")] BlocInventaire unBlocInventaire)
        {
            FormBlocInventaireViewModels formModel = this.GetFormBlocInventaire(false, unBlocInventaire);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.blocInventaireService.UpdateBlocInventaire(unBlocInventaire, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuBlocInventaire", formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.BlocInventaire, unBlocInventaire.ID, "Modification d'un bloc Inventaire", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuBlocInventaire", formModel);
        }

        // GET: BlocInventaire/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Bloc Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            BlocInventaire unBlocInventaire = this.blocInventaireService.GetBlocInventaireById(id);
            if (unBlocInventaire == null)
            {
                return HttpNotFound();
            }
            return View(unBlocInventaire);
        }

        // POST: BlocInventaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult DeleteConfirmed(int id)
        {
            this.blocInventaireService.DeleteBlocInventaire(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.BlocInventaire, id, "Suppression d'un bloc Inventaire", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        private FormBlocInventaireViewModels GetFormBlocInventaire(bool isEdit, BlocInventaire unBlocInventaire = null, int? inventaireId = null)
        {
            int? IdInventaire = null;
            if (unBlocInventaire != null)
            {
                IdInventaire = unBlocInventaire.InventaireID;
            }
            else if (inventaireId != null)
            {
                IdInventaire = inventaireId;
            }

            if(unBlocInventaire == null)
            {
                unBlocInventaire = new BlocInventaire();
                unBlocInventaire.Active = true;
                unBlocInventaire.Rank = 1;

                if (IdInventaire != null)
                {
                    unBlocInventaire.InventaireID = Convert.ToInt32(IdInventaire);
                }
            }

            FormBlocInventaireViewModels formModel = new FormBlocInventaireViewModels();
            formModel.unBlocInventaire = unBlocInventaire;

            //Gestion des listes
            formModel.listInventaire = new SelectList(this.inventaireService.GetInventaires(), "Id", "Nom", IdInventaire);

            //Gestion du mode Edit et du mode Ajax
            formModel.isEdit = isEdit;
            return formModel;
        }


        private void IsFromInventaire()
        {
            if (Session["isFromInventaire"] != null)
            {
                ViewBag.isFromInventaire = Convert.ToBoolean(Session["isFromInventaire"]);
            }
        }

    }
}
