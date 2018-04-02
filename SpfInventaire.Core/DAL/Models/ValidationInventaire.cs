using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class ValidationInventaire
    {
        public int ID { get; set; }

        [Required]
        public int InventaireID { get; set; }
        public virtual Inventaire Inventaire { get; set; }

        [Required]
        [Display(Name ="Utilisateur")]
        public virtual ApplicationUser Utilisateur { get; set; }

        [Display(Name = "Date inventaire")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }
    }
}
