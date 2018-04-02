using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class StockMaterielFormViewModel
    {
        public StockMateriel unStock { get; set; }
        public bool isEdit { get; set; }
        public SelectList listInventaire { get; set; }
        public SelectList listBlocInventaire { get; set; }
        public SelectList listMateriel { get; set; }

        public SelectList ListTypeMateriel { get; set; }
    }

    public class FormTransfertStockMaterielViewModel
    {
        public StockMaterielFormViewModel FormStock { get; set; }
        public int stockSourceId { get; set; }
    }

    public class ListingStockMaterielViewModel
    {
        public SelectList listMateriel { get; set; }
    }

    public class NewFormSortieStockViewModel
    {
        public SelectList listStockSortie { get; set; }
        public int stockSortieID { get; set; }

        public SelectList listStockEntree { get; set; }
        public int stockEntreeID { get; set; }

        [Required]
        [Display(Name = "Quantité")]
        [Range(1, 50)]
        public int quantite { get; set; }
        public int idMateriel { get; set; }
        public string NomMateriel { get; set; }
    }

    public class FormSortieStockViewModel
    {
        public bool isEdit { get; set; }

        //Où a ton enlever le matériel
        public sortieStockMaterielViewModel sortieStock { get; set; }
        public int? idMaterielParametre { get; set; }

        //A quel endroit on à pris en stock pour reremplir
        public sortieStockMaterielViewModel sourceStock { get; set; }
    }

    public class sortieStockMaterielViewModel
    {
        public SortieStockMateriel uneSortieStock { get; set; }
        public SelectList listInventaire { get; set; }
        public SelectList listBlocInventaire { get; set; }
        public SelectList listMateriel { get; set; }
        public SelectList listStock { get; set; }
    }

}
