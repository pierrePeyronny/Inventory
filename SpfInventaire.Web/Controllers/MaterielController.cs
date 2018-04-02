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
using SpfInventaire.Web.Helpers.Interfaces;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class MaterielController : Controller
    {
        private IMaterielService materielService;
        private IBlocInventaireService blocInventaireService;
        private ITypeMaterielService typeMaterielService;
        private ISelectListHelper selectListHelper;
        private ILoggerService logService;

        public MaterielController(IMaterielService materielService, IBlocInventaireService blocInventaireService, ITypeMaterielService typeMaterielService, ISelectListHelper selectListHelper, ILoggerService logService)
        {
            this.materielService = materielService;
            this.blocInventaireService = blocInventaireService;
            this.typeMaterielService = typeMaterielService;
            this.selectListHelper = selectListHelper;
            this.logService = logService;
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }

            if (Session["isFromBlocInventaire"] != null)
            {
                Session.Remove("isFromBlocInventaire");
            }

            //Gestion Session isFromInventaire
            if (Session["isFromInventaire"] != null)
            {
                Session.Remove("isFromInventaire");
            }

            return View(this.materielService.GetMateriels());
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetMaterielsByBlocsInventaire(int blocInventaireId)
        {
            IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsByBlocInventaire(blocInventaireId);
            Dictionary<string, string> dictionaryItem = new Dictionary<string, string>();

            foreach(Materiel unMateriel in listMateriel)
            {
                if(unMateriel.TypeMateriel != null)
                {
                    dictionaryItem.Add(unMateriel.ID.ToString(), unMateriel.TypeMateriel.Nom);
                }
                else
                {
                    dictionaryItem.Add(unMateriel.ID.ToString(), unMateriel.Nom);
                }           
            }           

            return Json(this.selectListHelper.CreateSelectList(dictionaryItem));
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Details(int? id, bool? isFromBlocInventaire)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Materiel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Materiel unMateriel = this.materielService.GetMaterielById(id);
            if (unMateriel == null)
            {
                return HttpNotFound();
            }

            //Gestion session isFromInventaire
            if (isFromBlocInventaire != null && isFromBlocInventaire == true)
            {
                if (Session["isFromBlocInventaire"] == null)
                {
                    Session.Add("isFromBlocInventaire", true);
                }
            }

            this.IsFromBlocInventaire();
            return View(unMateriel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create()
        {
            FormMaterielViewModels formModel = this.GetFormMateriel(false);
            return View("Create", formModel);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult GetFormCreate(int? blocInventaireId = null)
        {
            FormMaterielViewModels formModel = GetFormMateriel(false, blocInventaireId: blocInventaireId);
            return PartialView("_AjaxAddMateriel", formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create(Materiel unMateriel)
        {
            FormMaterielViewModels formModel = this.GetFormMateriel(false, unMateriel);
            if (ModelState.IsValid)
            {
                //insertion du blocInventaire
                ActionControllerResult result = this.materielService.InsertMateriel(unMateriel, User.Identity.GetUserId());
                //si erreur on affiche un message
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Materiel, unMateriel.ID, "Création d'un Matériel", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult AjaxCreate(Materiel unMateriel)
        {
            FormMaterielViewModels formModel = this.GetFormMateriel(false, unMateriel);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.materielService.InsertMateriel(unMateriel, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuMateriel", formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Materiel, unMateriel.ID, "Création d'un Matériel", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuMateriel", formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit(int? id)
        {
            if (id == null && !Request.IsAjaxRequest())
            {
                string ErrorMessage = "Id Materiel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Materiel unMateriel = this.materielService.GetMaterielById(id);
            if (unMateriel == null && !Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }

            this.IsFromBlocInventaire();

            FormMaterielViewModels formModel = this.GetFormMateriel(true, unMateriel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxEditMateriel", formModel);
            }
            else
            {
                return View(formModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit([Bind(Include = "ID,Nom,Description,Quantite,Tester,Rank,Active,DateCreation,DateModification,TypeMaterielID,BlocInventaireID")] Materiel unMateriel)
        {
            FormMaterielViewModels formModel = this.GetFormMateriel(false, unMateriel);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.materielService.UpdateMateriel(unMateriel, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;

                    this.IsFromBlocInventaire();
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Materiel, unMateriel.ID, "Modification d'un Matériel", null, User.Identity.GetUserId());

                if (Session["isFromBlocInventaire"] != null)
                {
                    if (Convert.ToBoolean(Session["isFromBlocInventaire"]))
                    {
                        return RedirectToAction("Details", "Materiel", new { id = unMateriel.ID, isFromBlocInventaire = true });
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
            this.IsFromBlocInventaire();
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult AjaxEdit([Bind(Include = "ID,Nom,Description,Quantite,Tester,Rank,Active,DateCreation,DateModification,TypeMaterielID,BlocInventaireID")] Materiel unMateriel)
        {
            FormMaterielViewModels formModel = this.GetFormMateriel(false, unMateriel);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.materielService.UpdateMateriel(unMateriel, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuMateriel", formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Materiel, unMateriel.ID, "Modification d'un Matériel", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuMateriel", formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Materiel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Materiel unMateriel = this.materielService.GetMaterielById(id);
            if (unMateriel == null)
            {
                return HttpNotFound();
            }
            return View(unMateriel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult DeleteConfirmed(int id)
        {
            this.materielService.DeleteMateriel(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.Materiel, id, "Suppression d'un Matériel", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        private FormMaterielViewModels GetFormMateriel(bool isEdit, Materiel unMateriel = null, int? blocInventaireId = null)
        {
            int? BlocInventaireId = null;
            if (unMateriel != null)
            {
                BlocInventaireId = unMateriel.BlocInventaireID;
            }
            else if (blocInventaireId != null)
            {
                BlocInventaireId = blocInventaireId;
            }

            int? TypeMaterielId = null;
            if (unMateriel != null)
            {
                TypeMaterielId = unMateriel.TypeMaterielID;
            }


            if (unMateriel == null)
            {
                unMateriel = new Materiel();
                unMateriel.Active = true;
                unMateriel.Quantite = 1;
                unMateriel.Tester = true;
                unMateriel.Rank = 1;

                if(blocInventaireId != null)
                {
                    unMateriel.BlocInventaireID = Convert.ToInt32(blocInventaireId);
                }
            }

            FormMaterielViewModels formModel = new FormMaterielViewModels();
            formModel.unMateriel = unMateriel;

            //Gestion des listes
            formModel.listBlocInventaire = new SelectList(this.blocInventaireService.GetBlocsInventaireListe(), "Id", "Nom", BlocInventaireId);
            formModel.listTypeMateriel = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.typeMaterielService.GetTypeMateriels(), "ID", "Nom"), TypeMaterielId, "Sélectionner");
            //Gestion du mode Edit et du mode Ajax
            formModel.isEdit = isEdit;
            return formModel;
        }


        private void IsFromBlocInventaire()
        {
            if (Session["isFromBlocInventaire"] != null)
            {
                ViewBag.isFromBlocInventaire = Convert.ToBoolean(Session["isFromBlocInventaire"]);
            }
        }


    }
}
