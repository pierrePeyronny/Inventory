using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class PleinEssence
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Litrage obligatoire ex: 10.2")]
        [Display(Name = "Litrage")]
        [Range(0, 100, ErrorMessage = "Saisir un Litrage valide")]
        public float Litrage { get; set; }

        [Required(ErrorMessage = "Prix obligatoire ex: 10.2")]
        [DataType(DataType.Currency)]
        [Range(0, 200, ErrorMessage = "Saisir un Prix valide")]
        public float Prix { get; set; }


        [Required(ErrorMessage = "Le Kilométrage est obligatoire")]
        [Display(Name = "Kilométrage")]
        [Range(0, int.MaxValue, ErrorMessage = "Saisir un Kilomètrage valide")]
        public int Kilometrage { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Créateur")]
        public virtual ApplicationUser UtilisateurCreateur { get; set; }

        [Display(Name = "Engin")]
        public int EnginID { get; set; }
        public virtual Engin Engin { get; set; }

    }
}
