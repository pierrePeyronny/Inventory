using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class Inventaire
    {
        public int ID { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Nom { get; set; }

        [Range(0, 50)]
        [Display(Name = "Ordre")]
        public int Rank { get; set; }

        [Display(Name = "Activer")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Inventaire de Stock")]
        public bool IsInventaireStock { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }

        public virtual ICollection<BlocInventaire> BlocInventaires { get; set; }

        public virtual ICollection<ValidationInventaire> ValidationInventaires { get; set; }
    }
}
