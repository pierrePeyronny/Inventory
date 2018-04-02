using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{

    public enum EVENEMENT_TYPE
    {
        [Display(Name = "Désinfection")]
        Desinfection = 1,

        [Display(Name = "Cérémonie")]
        Ceremonie = 2,

        [Display(Name = "FMPA")]
        FMPA = 3
    }

    public class Evenement
    {
        public int ID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sélectionner un événement")]
        [Display(Name = "Evénement")]
        public EVENEMENT_TYPE Type { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Durée (heure)")]
        public int Duree { get; set; }

        [Required]
        [Display(Name = "Date de l'événement")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime, ErrorMessage = "Ce n'est pas une Date Valide")]
        public DateTime DateEvenement { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }

    }
}
