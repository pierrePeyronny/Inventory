using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public enum MATERIEL_RAISON_SORTIE
    {
        [Display(Name = "Intervention")]
        Intervention = 1,

        [Display(Name = "Prise de Garde")]
        PriseDeGarde = 2
    }

    public enum MATERIEL_TYPE_SORTIE
    {
        [Display(Name = "Sortie")]
        Intervention = 1,

        [Display(Name = "Ajout")]
        PriseDeGarde = 2
    }

    public class SortieStockMateriel
    {
        public int ID { get; set; }

        [Display(Name = "Date Sortie Materiel")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateSortie { get; set; }

        [Required]
        [Display(Name = "Raison")]
        [Range(1, int.MaxValue, ErrorMessage = "Raison Obligatoire")]
        public MATERIEL_RAISON_SORTIE Raison { get; set; }

        [Required]
        [Display(Name = "Type de la sortie")]
        public MATERIEL_TYPE_SORTIE TypeSortie { get; set; }

        [Required]
        [Display(Name = "Quantité")]
        [Range(1, 50)]
        public int Quantite { get; set; }

        [Display(Name = "Créateur")]
        public virtual ApplicationUser Utilisateur { get; set; }

        [Required]
        [Display(Name = "Stock Sortie")]
        [Range(1, int.MaxValue, ErrorMessage = "Lot Obligatoire")]
        public int UsedStockMaterielID { get; set; }
        public virtual StockMateriel UsedStockMateriel { get; set; }

        [Required]
        [Display(Name = "Stock Source")]
        [Range(1, int.MaxValue, ErrorMessage = "Lot Obligatoire")]
        public int IdSourceStock { get; set; }
    }
}
