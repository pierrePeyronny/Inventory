using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.BLL
{
    public static class Constantes
    {
        public const string SESSION_ID_USER = "userId";
        public const string MESSAGE_ERR_NOTIFICATIONS = "Une erreur est survenue";

        public enum ActionControllerResult
        {
            SUCCESS = 1,
            FAILURE = 2
        }



        //Roles
        public const string ROLE_ADMINISTRATEUR = "Administrateur";
        public const string ROLE_ADMIN_TICKET = "AdminTicket";
        public const string ROLE_ADMIN_GESTION_INVENTAIRE = "AdminInventaire";
        public const string ROLE_CHEF_GARDE = "ChefGarde";

    }
}
