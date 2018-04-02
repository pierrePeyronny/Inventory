using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpfInventaire.Core.DAL.ViewModels;
using SpfInventaire.Core.BLL;

namespace SpfInventaire.Web.Controllers
{
    [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
    public class CreationInventaireController : Controller
    {
        private IInventaireService inventaireService;
        private IBlocInventaireService blocService;
        private IMaterielService materielService;

        public CreationInventaireController(IInventaireService inventaireService, IBlocInventaireService blocService, IMaterielService materielService)
        {
            this.inventaireService = inventaireService;
            this.blocService = blocService;
            this.materielService = materielService;
        }

        // GET: CreationInventaire
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }

            return View(this.inventaireService.GetInventaires());
        }



        [HttpPost]
        public ActionResult GetSortForm(int rechercheId, string objectList)
        {
            ListOrderCreationInventaireViewModels formModel = new ListOrderCreationInventaireViewModels();
            formModel.ListOrder = new List<ItemListOrderCreationInventaireViewModels>();
            ItemListOrderCreationInventaireViewModels itemModel;

            switch (objectList)
            {
                case "BlocInventaire":
                    formModel.entity = "BlocInventaire";
                    IEnumerable<BlocInventaire> listbloc= this.blocService.GetBlocsInventaireByInventaire(rechercheId);

                    foreach (BlocInventaire unBloc in listbloc)
                    {
                        itemModel = new ItemListOrderCreationInventaireViewModels();
                        itemModel.Id = unBloc.ID;
                        itemModel.Text = unBloc.Nom;
                        itemModel.Ordre = unBloc.Rank;

                        formModel.ListOrder.Add(itemModel);
                    }

                    break;

                case "Materiel":
                    formModel.entity = "Materiel";
                    IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsByBlocInventaire(rechercheId);

                    foreach (Materiel unMateriel in listMateriel)
                    {
                        itemModel = new ItemListOrderCreationInventaireViewModels();
                        itemModel.Id = unMateriel.ID;
                        if(unMateriel.TypeMateriel != null)
                        {
                            itemModel.Text = unMateriel.TypeMateriel.Nom;
                        }
                        else
                        {
                            itemModel.Text = unMateriel.Nom;
                        }                     
                        itemModel.Ordre = unMateriel.Rank;

                        formModel.ListOrder.Add(itemModel);
                    }

                    break;
            }

            return PartialView("_OrderListe", formModel);
        }


        public ActionResult ChangeSortList(ListOrderCreationInventaireViewModels listToUpdate)
        {
            foreach (ItemListOrderCreationInventaireViewModels item in listToUpdate.ListOrder)
            {
                switch (listToUpdate.entity)
                {
                    case "BlocInventaire":

                        this.blocService.ChangeOrdreBlocInventaire(item.Id, item.Ordre);

                        break;

                    case "Materiel":

                        this.materielService.ChangeOrdreMateriel(item.Id, item.Ordre);

                        break;
                }
            }

            return Json("success");
        }

    }
}