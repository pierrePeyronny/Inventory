using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface ITicketIncidentService : IDisposable
    {
        ActionControllerResult DeleteTicketByMaterielId(int id);
        void DeleteTicket(int id);
        TicketIncident GetTicketById(object id);
        IEnumerable<TicketIncident> GetTickets();
        IEnumerable<TicketIncident> GetTicketsByStatut(TICKET_INCIDENT_STATUT statut);
        ActionControllerResult MarquerLu(object id, string userId = null);
        ActionControllerResult InsertTicket(TicketIncident unTicket, string userId = null, bool isAdminTicket = false);
        ActionControllerResult UpdateTicket(TicketIncident unTicket, bool userIsAdminTicket, string userId = null);
    }
}