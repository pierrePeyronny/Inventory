using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{

    public enum TICKET_INCIDENT_TYPE
    {
        [Display(Name = "N'est pas présent")]
        Manquant = 1,

        [Display(Name = "Ne fonctionne pas")]
        Dysfonctionnement = 2,

        [Display(Name = "Détérioré")]
        Casser = 3,

        [Display(Name = "Péremption")]
        Peremption = 4,

        [Display(Name = "Pression Bouteille")]
        PressionBouteille = 5
    }

    public enum TICKET_INCIDENT_STATUT
    {
        [Display(Name = "Nouveau")]
        Nouveau = 1,

        [Display(Name = "Lu")]
        Lu = 2,

        [Display(Name = "En cours")]
        PrisEnCompte = 3,

        [Display(Name = "Résolu")]
        Resolu = 4
    }

    public class TicketIncident
    {

        public int ID { get; set; }

        public virtual Inventaire Inventaire { get; set; }

        [Display(Name = "Emplacement")]
        public virtual BlocInventaire BlocInventaire { get; set; }

        [ForeignKey("Materiel")]
        [Display(Name = "Matériel")]
        [Range(1, int.MaxValue, ErrorMessage = "Sélectionner un Matériel")]
        [Required(ErrorMessage = "Sélectionner un Matériel")]
        public int MaterielID { get; set; }
        public virtual Materiel Materiel { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sélectionner un Problème")]
        [Display(Name = "Type")]
        public TICKET_INCIDENT_TYPE Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sélectionner un Statut")]
        [Display(Name = "Statut")]
        public TICKET_INCIDENT_STATUT Statut { get; set; }

        [StringLength(50)]
        [Display(Name = "Numéro de FEB")]
        public String NumeroFeb { get; set; }

        [Display(Name = "Créateur")]
        public virtual ApplicationUser UtilisateurCreateur { get; set; }

        [Display(Name = "Responsable")]
        public virtual ApplicationUser UtilisateurAdministrateur { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Date création")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Date modification")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModification { get; set; }
    }
}
