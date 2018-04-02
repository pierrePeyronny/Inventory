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
using SpfInventaire.Web.Helpers.Interfaces;
using static SpfInventaire.Core.BLL.Constantes;
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class StockMaterielController : Controller
    {
        private IStockMaterielService stockMaterielService;
        private IInventaireService inventaireService;
        private IBlocInventaireService blocInventaireService;
        private IMaterielService materielService;
        private ITypeMaterielService typeMaterielService;
        private ISelectListHelper selectListHelper;
        private ILoggerService logService;

        public StockMaterielController(IStockMaterielService stockMaterielService, IInventaireService inventaireService, IBlocInventaireService blocInventaireService, IMaterielService materielService, ITypeMaterielService typeMaterielService, ISelectListHelper selectListHelper, ILoggerService logService)
        {
            this.stockMaterielService = stockMaterielService;
            this.inventaireService = inventaireService;
            this.blocInventaireService = blocInventaireService;
            this.materielService = materielService;
            this.typeMaterielService = typeMaterielService;
            this.selectListHelper = selectListHelper;
            this.logService = logService;
        }


        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.stockMaterielService.GetStockMateriels());
        }

        [HttpPost]
        public ActionResult GetStocksByMateriel(int materielId)
        {
            return PartialView("_IndexCreationStock", this.stockMaterielService.GetStockMaterielByMaterielId(materielId));
        }

        [HttpPost]
        public ActionResult GetStocksListByMateriel(int materielId)
        {
            return Json(this.selectListHelper.CreateSelectList(this.stockMaterielService.CreateListStockByMaterielId(materielId)));        
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Stock Materiel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            StockMateriel unStockMateriel = this.stockMaterielService.GetStockMaterielById(id);
            if (unStockMateriel == null)
            {
                return HttpNotFound();
            }
            return View(unStockMateriel);
        }

        public ActionResult IndexListingStocks()
        {
            ListingStockMaterielViewModel formModel = new ListingStockMaterielViewModel();
            formModel.listMateriel = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.typeMaterielService.GetTypeMateriels(), "ID", "Nom"), null, "Sélectionner");
            return View(formModel);
        }

        [HttpPost]
        public ActionResult AjaxListingStocks(int typeMaterielID)
        {            
            return PartialView("_listingStocks", this.stockMaterielService.GetStockMaterielByTypeMateriel(typeMaterielID));
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create()
        {
            StockMaterielFormViewModel formModel = GetFormStock(false);
            return View("Create", formModel);
        }

        [HttpPost]
        public ActionResult GetFormCreate(int materielId)
        {
            StockMaterielFormViewModel formModel = GetFormStock(false, materielId: materielId);
            return PartialView("_AjaxAddStock", formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Quantite,DatePeremption,Supprime,MaterielID")] StockMateriel unStock)
        {
            StockMaterielFormViewModel formModel = this.GetFormStock(false, unStock);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.stockMaterielService.InsertStockMateriel(unStock, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.StockMateriel, null, "Création d'un Stock Materiel", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(StockMateriel unStock)
        {
            StockMaterielFormViewModel formModel = this.GetFormStock(false, unStock);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.stockMaterielService.InsertStockMateriel(unStock, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuStock", formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.StockMateriel, null, "Création d'un Stock Materiel", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuStock", formModel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Stock matériel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            StockMateriel unStock = this.stockMaterielService.GetStockMaterielById(id);
            if (unStock == null)
            {
                return HttpNotFound();
            }

            StockMaterielFormViewModel formModel = this.GetFormStock(true, unStock);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxEditStock", formModel);
            }
            else
            {
                return View(formModel);
            }
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Quantite,DatePeremption,Supprime,DateCreation,DateModification,MaterielID,TypeMaterielID")] StockMateriel unStock)
        {
            StockMaterielFormViewModel formModel = this.GetFormStock(true, unStock);
            if (ModelState.IsValid)
            {

                ActionControllerResult result = this.stockMaterielService.UpdateStockMateriel(unStock, User.Identity.GetUserId());

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                return RedirectToAction("Index");
            }
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxEdit([Bind(Include = "ID,Quantite,DatePeremption,Supprime,DateCreation,DateModification,MaterielID,TypeMaterielID")] StockMateriel unStock)
        {
            StockMaterielFormViewModel formModel = this.GetFormStock(true, unStock);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.stockMaterielService.UpdateStockMateriel(unStock, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuStock", formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.StockMateriel, unStock.ID, "Création d'un Stock Materiel", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuStock", formModel);
        }

        [HttpPost]
        public ActionResult GetFormTransfert(int id)
        {
            FormTransfertStockMaterielViewModel formModel = new FormTransfertStockMaterielViewModel();
            formModel.stockSourceId = id;
            formModel.FormStock = this.GetFormStock(false);
            return PartialView("_AjaxTransfertStock", formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxTransfert(FormTransfertStockMaterielViewModel unTransfertStock)
        {
            if(unTransfertStock.FormStock == null)
            {
                unTransfertStock.FormStock = this.GetFormStock(false);
            }
            else
            {
                unTransfertStock.FormStock = this.GetFormStock(false, unTransfertStock.FormStock.unStock);
            }

            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.stockMaterielService.TransfertStockMateriel(unTransfertStock.stockSourceId, unTransfertStock.FormStock.unStock, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuTransfertStock", unTransfertStock);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.TransfertMateriel, LOG_TYPE_OBJECT.StockMateriel, unTransfertStock.stockSourceId, "Transfert d'un Stock Materiel", null, User.Identity.GetUserId());
                return Json(string.Empty);
            }
            return PartialView("_FormContenuTransfertStock", unTransfertStock);
        }


        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Stock matériel manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            StockMateriel unStockMateriel = this.stockMaterielService.GetStockMaterielById(id);
            if (unStockMateriel == null)
            {
                return HttpNotFound();
            }
            return View(unStockMateriel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.stockMaterielService.DeleteStockMateriel(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.StockMateriel, id, "Suppression d'un Stock Matériel", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost]
        public ActionResult AjaxDelete(int id)
        {
            this.stockMaterielService.DeleteStockMateriel(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.StockMateriel, id, "Suppression d'un Stock Matériel", null, User.Identity.GetUserId());
            return Json(string.Empty);
        }


        private StockMaterielFormViewModel GetFormStock(bool isEdit, StockMateriel unStock = null, int? materielId = null)
        {
            //Gestion de la récupératio des ID
            Materiel unMateriel = null;
            int? IdInventaire = null;
            int? IdBloc = null;
            int? IdMateriel = null;
            int? IdTypeMateriel = null;

            if(unStock == null)
            {
                unStock = new StockMateriel();
            }

            if (unStock.MaterielID > 0)
            {
                unMateriel = this.materielService.GetMaterielById(unStock.MaterielID);
                IdMateriel = unStock.MaterielID;
            }
            else if (materielId != null)
            {
                unMateriel = this.materielService.GetMaterielById(materielId);
                IdMateriel = materielId;
            }

            if (IdMateriel != null)
            {
                IdInventaire = unMateriel.BlocInventaire.InventaireID;
                IdBloc = unMateriel.BlocInventaireID;
                unStock.MaterielID = Convert.ToInt32(IdMateriel);
                IdTypeMateriel = unStock.TypeMaterielID;
            }


            //Création Form
            StockMaterielFormViewModel formModel = new StockMaterielFormViewModel();
            formModel.unStock = unStock;

            //Gestion des listes
            formModel.listInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.inventaireService.GetInventaires(), "ID", "Nom"), IdInventaire, "Sélectionner");

            if (IdInventaire != null)
            {
                formModel.listBlocInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.blocInventaireService.GetBlocsInventaireByInventaire(Convert.ToInt32(IdInventaire)), "ID", "Nom"), IdBloc, "Sélectionner");
            }
            else
            {
                formModel.listBlocInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(Enumerable.Empty<SelectListItem>(), "ID", "Nom"), IdBloc, "Aucun");
            }

            if (IdInventaire != null)
            {
                IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsByBlocInventaire(Convert.ToInt32(IdBloc));
                Dictionary<string, string> dictionaryItem = new Dictionary<string, string>();

                foreach (Materiel unMat in listMateriel)
                {
                    if (unMat.TypeMateriel != null)
                    {
                        dictionaryItem.Add(unMat.ID.ToString(), unMat.TypeMateriel.Nom);
                    }
                    else
                    {
                        dictionaryItem.Add(unMat.ID.ToString(), unMat.Nom);
                    }
                }
                formModel.listMateriel = this.selectListHelper.AddFirstItemSelectList(this.selectListHelper.CreateSelectList(dictionaryItem), IdMateriel, "Sélectionner");
            }
            else
            {
                formModel.listMateriel = this.selectListHelper.AddFirstItemSelectList(new SelectList(Enumerable.Empty<SelectListItem>(), "ID", "Nom"), IdMateriel, "Aucun");
            }

            formModel.ListTypeMateriel = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.typeMaterielService.GetTypeMateriels(), "ID", "Nom"), IdTypeMateriel, "Sélectionner");

            //Gestion du mode Edit et du mode Ajax
            formModel.isEdit = isEdit;
            return formModel;
        }


    }
}
