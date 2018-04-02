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
using SpfInventaire.Web.Helpers.Interfaces;
using SpfInventaire.Core.DAL.ViewModels;
using static SpfInventaire.Core.BLL.Constantes;
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class SortieStockController : Controller
    {
        private ISortieStockService sortieStockService;
        private IStockMaterielService stockService;
        private IInventaireService inventaireService;
        private IBlocInventaireService blocInventaireService;
        private IMaterielService materielService;
        private ITypeMaterielService typeMaterielService;
        private ISelectListHelper selectListHelper;
        private ILoggerService logService;

        public SortieStockController(ISortieStockService sortieStockService, IStockMaterielService stockService, IInventaireService inventaireService, IBlocInventaireService blocInventaireService, IMaterielService materielService, ITypeMaterielService typeMaterielService, ISelectListHelper selectListHelper, ILoggerService logService)
        {
            this.sortieStockService = sortieStockService;
            this.stockService = stockService;
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

            return View(this.sortieStockService.GetSortieStock());
        }

        public ActionResult IndexSaisieStock()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStockBySearch(string search)
        {
            if (search == null)
            {
                string ErrorMessage = "Id Inventaire manquant";
                return RedirectToAction("IndexSaisieStock", new { MessageErreur = ErrorMessage });
            }
            IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsBySearch(search);

            return PartialView("_FormSaisieStock", listMateriel);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Sortie de Stock manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            SortieStockMateriel uneSortie = this.sortieStockService.GetSortieStockById(id);
            if (uneSortie == null)
            {
                return HttpNotFound();
            }
            return View(uneSortie);
        }

        [HttpPost]
        public ActionResult GetFormCreate(int? materielID = null)
        {
            NewFormSortieStockViewModel formModel = GetFormAjax(materielID);
            return PartialView("_AjaxAddSortieStock", formModel);
        }

        public ActionResult Create(string MessageRetour = null)
        {
            ViewBag.isChefGarde = User.IsInRole(Constantes.ROLE_CHEF_GARDE);

            if (!String.IsNullOrEmpty(MessageRetour))
            {
                ViewBag.Success = MessageRetour;
            }

            FormSortieStockViewModel formModel = GetForm(false);
            return View("Create", formModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormSortieStockViewModel formSortieStock)
        {
            ViewBag.isChefGarde = User.IsInRole(Constantes.ROLE_CHEF_GARDE);
            FormSortieStockViewModel formModel = this.GetForm(false, formSortieStock.sortieStock.uneSortieStock);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.sortieStockService.InsertSortieStock(formSortieStock.sortieStock.uneSortieStock, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.SortieStockMateriel, null, "Création d'une Sortie de Materiel", null, User.Identity.GetUserId());
                return RedirectToAction("Create", new { MessageRetour = "Réussi" });
            }
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(NewFormSortieStockViewModel formSortieStock)
        {
            NewFormSortieStockViewModel formModel = this.GetFormAjax(formSortieStock.idMateriel, formSortieStock.quantite);
            if(ModelState.IsValid)
            {
                if (formSortieStock.stockSortieID > 0 && formSortieStock.stockEntreeID > 0 && formSortieStock.quantite > 0)
                {
                    ActionControllerResult result = this.sortieStockService.InsertSortieStock(formSortieStock.stockSortieID, formSortieStock.stockEntreeID, formSortieStock.quantite, User.Identity.GetUserId());
                    if (result == ActionControllerResult.FAILURE)
                    {
                        ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                        return PartialView("_FormContenuSortieStock", formModel);
                    }
                    this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.SortieStockMateriel, null, "Création d'une Sortie de Materiel", null, User.Identity.GetUserId());
                    return Json(string.Empty);
                }
                else
                {
                    ModelState.AddModelError("quantite", "La quantité doit etre supérieur à zéro");
                }
            }

            return PartialView("_FormContenuSortieStock", formModel);
        }


        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SortieStockMateriel sortieStockMateriel = this.sortieStockService.GetSortieStockById(id);
            if (sortieStockMateriel == null)
            {
                return HttpNotFound();
            }
            return View(sortieStockMateriel);
        }

        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.sortieStockService.DeleteSortieStock(id);
            return RedirectToAction("Index");
        }



        private FormSortieStockViewModel GetForm(bool isEdit, SortieStockMateriel uneSortie = null, SortieStockMateriel uneSource = null, int? idMaterielParametre = null)
        {
            FormSortieStockViewModel formModel = new FormSortieStockViewModel();
            formModel.sortieStock = GetSortieStockForm(uneSortie, idMaterielParametre);
            formModel.idMaterielParametre = idMaterielParametre;

            formModel.sourceStock= GetSortieStockForm(uneSource);

            //Gestion du mode Edit et du mode Ajax
            formModel.isEdit = isEdit;
            return formModel;
        }

        private NewFormSortieStockViewModel GetFormAjax(int? idMateriel, int? quantite = null)
        {
            NewFormSortieStockViewModel formModel = new NewFormSortieStockViewModel();

            //Recherche du matériel par son ID
            Materiel unMateriel = this.materielService.GetMaterielById(idMateriel);

            if(unMateriel != null)
            {
                //Affectation des stock du matériel à la liste "listStockSortie" du form
                IEnumerable<StockMateriel> listStockSortie = this.stockService.GetStockMaterielByMaterielId(Convert.ToInt32(idMateriel));
                Dictionary<string, string> dictionaryItemSortie = new Dictionary<string, string>();
                Dictionary<string, string> dictionaryItemEntree = new Dictionary<string, string>();

                string libelle = "";

                foreach (StockMateriel unStockSortie in listStockSortie)
                {
                    if(unStockSortie.DatePeremption != null)
                    {
                        libelle = "Péremption: " + unStockSortie.DatePeremption.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        libelle = unStockSortie.Materiel.TypeMateriel.Nom;
                    }
                    dictionaryItemSortie.Add(unStockSortie.ID.ToString(), libelle);
                }  

                //Recherches des stocks matériel du même type aux autres emplacements
                IEnumerable<StockMateriel> listTemp = this.stockService.GetStockMaterielByTypeMateriel(Convert.ToInt32(unMateriel.TypeMaterielID)).Where(w => w.MaterielID != unMateriel.ID);
                foreach (StockMateriel unStockEntree in listTemp)
                {
                    libelle = unStockEntree.Materiel.BlocInventaire.Inventaire.Nom + "=>" + unStockEntree.Materiel.BlocInventaire.Nom;
                    if(unStockEntree.DatePeremption != null)
                    {
                        libelle += ": " + unStockEntree.DatePeremption.Value.ToString("dd/MM/yyyy");
                    }
                    dictionaryItemEntree.Add(unStockEntree.ID.ToString(), libelle);
                }

                formModel.listStockSortie = this.selectListHelper.CreateSelectList(dictionaryItemSortie);
                formModel.listStockEntree = this.selectListHelper.CreateSelectList(dictionaryItemEntree);
                formModel.quantite = Convert.ToInt32(quantite);
                formModel.NomMateriel = unMateriel.TypeMateriel.Nom;
                formModel.idMateriel = unMateriel.ID;
            }

            return formModel;
        }

        private sortieStockMaterielViewModel GetSortieStockForm(SortieStockMateriel uneSortie, int? idMaterielParametre = null)
        {
            //Gestion de la récupératio des ID
            Materiel unMateriel = null;
            int? IdInventaire = null;
            int? IdBloc = null;
            int? IdMateriel = null;
            int? IdStock = null;

            if (uneSortie == null)
            {
                uneSortie = new SortieStockMateriel();

                //Initialisation des listes si un matériel est passer en paramètre (formulaireAjax)
                if(idMaterielParametre != null && idMaterielParametre > 0)
                {
                    unMateriel = GetIdsForList(idMaterielParametre, ref IdInventaire, ref IdBloc, ref IdMateriel);
                }

            }
            else if (uneSortie.UsedStockMaterielID > 0)
            {
                StockMateriel unStock = this.stockService.GetStockMaterielById(uneSortie.UsedStockMaterielID);
                if(unStock != null)
                {
                    IdMateriel = unStock.MaterielID;
                    IdBloc = unStock.Materiel.BlocInventaireID;
                    IdInventaire = unStock.Materiel.BlocInventaire.InventaireID;
                }

            }else if(uneSortie != null && idMaterielParametre > 0)
            {
                unMateriel = GetIdsForList(idMaterielParametre, ref IdInventaire, ref IdBloc, ref IdMateriel);
            }


            //Création Form
            sortieStockMaterielViewModel formModel = new sortieStockMaterielViewModel();
            formModel.uneSortieStock = uneSortie;

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

            if(IdMateriel == null)
            {
                formModel.listStock = this.selectListHelper.AddFirstItemSelectList(new SelectList(Enumerable.Empty<SelectListItem>(), "ID", "Nom"), IdStock, "Aucun");
            }
            else
            {
                formModel.listStock = this.selectListHelper.AddFirstItemSelectList(this.selectListHelper.CreateSelectList(this.stockService.CreateListStockByMaterielId(Convert.ToInt32(IdMateriel))), IdStock, "Sélectionner");
            }


            return formModel;
        }


        private Materiel GetIdsForList(int? idMaterielParametre, ref int? IdInventaire, ref int? IdBloc, ref int? IdMateriel)
        {
            Materiel unMateriel = this.materielService.GetMaterielById(idMaterielParametre);
            if (unMateriel != null)
            {
                IdMateriel = unMateriel.ID;
                IdBloc = unMateriel.BlocInventaireID;
                IdInventaire = unMateriel.BlocInventaire.InventaireID;
            }

            return unMateriel;
        }
    }
}
