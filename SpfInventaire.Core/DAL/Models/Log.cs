using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL.Models
{
    public enum LOG_TYPE_OBJECT
    {
        [Display(Name = "Inventaire")]
        Inventaire = 1,
        [Display(Name = "Utilisateur")]
        User = 2,
        Role = 3,
        [Display(Name = "Ajout Role")]
        AssociationRoleToUser = 4,
        [Display(Name = "Emplacement")]
        BlocInventaire = 5,
        Materiel = 6,
        Ticket = 7,
        Evenement = 8,
        Email = 9,
        ValidationInventaire = 10,
        TypeMateriel = 11,
        StockMateriel = 12,
        SortieStockMateriel = 13,
        Engin = 14,
        PleinEssence = 15
    }

    public enum LOG_TYPE_EVENT
    {
        [Display(Name = "Création")]
        Create = 1,
        [Display(Name = "Modification")]
        Edit = 2,
        [Display(Name = "Suppression")]
        Delete = 3,
        Erreur = 4,
        Connexion = 5,
        [Display(Name = "Première Connexion")]
        PremiereConnexion = 6,
        TransfertMateriel = 7
    }

    public class Log
    {
        public int ID { get; set; }

        [Display(Name = "Type évenement")]
        public LOG_TYPE_EVENT TypeEvt { get; set; }

        [Display(Name = "Type objet")]
        public LOG_TYPE_OBJECT? TypeObject { get; set; }

        [Display(Name = "Id Objet")]
        public int? IdObject { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Exception")]
        public string Exception { get; set; }

        [Display(Name = "Date évenement")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Utilisateur")]
        public virtual ApplicationUser Utilisateur { get; set; }



    }
}
