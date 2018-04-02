using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public enum MATERIEL_TYPE_DOMAINE
    {
        [Display(Name = "Secourisme")]
        Secourisme = 1,

        [Display(Name = "Incendie")]
        Incendie = 2,

        [Display(Name = "Divers")]
        Divers = 3
    }

    public class TypeMateriel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Domaine")]
        [Range(1, int.MaxValue, ErrorMessage = "Le domaine est requis")]
        public MATERIEL_TYPE_DOMAINE Domaine { get; set; }

        public virtual ICollection<Materiel> Materiels { get; set; }

    }
}
