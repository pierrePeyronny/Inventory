using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public class Materiel
    {
        public int ID { get; set; }

        [StringLength(70)]
        public string Nom { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 50)]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; }

        [Display(Name = "A tester")]
        public bool Tester { get; set; }

        [Range(0, 50)]
        [Display(Name = "Ordre")]
        public int Rank { get; set; }

        [Display(Name = "Activer")]
        public bool Active { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }

        [Display(Name = "Bloc Inventaire")]
        public int BlocInventaireID { get; set; }
        public virtual BlocInventaire BlocInventaire { get; set; }

        public virtual ICollection<TicketIncident> Tickets { get; set; }

        [Display(Name = "Type Matériel")]
        [Range(1, int.MaxValue, ErrorMessage = "Ce champs est requis")]
        public int? TypeMaterielID { get; set; }
        public virtual TypeMateriel TypeMateriel { get; set; }

        public virtual ICollection<StockMateriel> StockMateriels { get; set; }

    }
}
