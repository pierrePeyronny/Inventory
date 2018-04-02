using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class StockMateriel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Quantité")]
        [Range(0, int.MaxValue, ErrorMessage = "Ce champs est requis")]
        public int Quantite { get; set; }

        [Display(Name = "Date de Péremption")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DatePeremption { get; set; }

        [Display(Name = "Supprimé")]
        public bool Supprime { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }

        [Required]
        [Display(Name = "Matériel")]
        [Range(1, int.MaxValue, ErrorMessage = "Ce champs est requis")]
        public int? MaterielID { get; set; }
        public virtual Materiel Materiel { get; set; }

        [Display(Name = "Type Matériel")]
        public int TypeMaterielID { get; set; }
        public virtual TypeMateriel TypeMateriel { get; set; }

    }
}
