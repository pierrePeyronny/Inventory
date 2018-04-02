using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpfInventaire.Core.DAL.ViewModels
{
    public class FormTicketIncidentViewModels
    {
        public TicketIncident unTicket { get; set; }

        public bool isEdit { get; set; }

        public bool isAdminTicket { get; set; }

        public SelectList listInventaire { get; set; }
        public SelectList listBlocInventaire { get; set; }
        public SelectList listMateriel { get; set; }
    }
}
