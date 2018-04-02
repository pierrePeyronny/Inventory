using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class TicketIncidentService : ITicketIncidentService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<TicketIncident> ticketRepository;
        private ILoggerService logService;
        private IUserService userService;
        private IEmailService emailService;

        public TicketIncidentService(IUnitOfWork unitOfWork, IGenericRepository<TicketIncident> ticketRepository, ILoggerService logService, IUserService userService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.ticketRepository = ticketRepository;
            this.logService = logService;
            this.userService = userService;
            this.emailService = emailService;
        }


        public IEnumerable<TicketIncident> GetTickets()
        {
            return this.ticketRepository.Get();
        }

        public IEnumerable<TicketIncident> GetTicketsByStatut(TICKET_INCIDENT_STATUT statut)
        {
            return this.ticketRepository.Get(
                filter: m =>m.Statut == statut
                );
        }

        public TicketIncident GetTicketById(object id)
        {
            TicketIncident unTicket = this.ticketRepository.GetByID(id);
            return unTicket;
        }

        public ActionControllerResult MarquerLu(object id, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                TicketIncident unTicket = this.GetTicketById(id);
                if(unTicket != null)
                {
                    unTicket.Statut = TICKET_INCIDENT_STATUT.Lu;
                    unTicket.DateModification = DateTime.Now;

                    this.ticketRepository.Update(unTicket);
                    this.unitOfWork.Save();
                    result = ActionControllerResult.SUCCESS;
                }
                else
                {
                    result = ActionControllerResult.FAILURE;
                }

            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Ticket, null, "Erreur Lors de la modification d'un Ticket d'incident", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public ActionControllerResult InsertTicket(TicketIncident unTicket, string userId = null, bool isAdminTicket = false)
        {
            ActionControllerResult result;
            try
            {
                unTicket.UtilisateurCreateur = this.userService.GetUserById(userId);
                unTicket.DateCreation = DateTime.Now;
                unTicket.DateModification = DateTime.Now;            
                this.ticketRepository.Insert(unTicket);
                this.unitOfWork.Save();

                if(!isAdminTicket)
                {
                    IEnumerable<ApplicationUser> listAdmin = this.userService.GetUsersByRole(ROLE_ADMIN_TICKET);

                    foreach(ApplicationUser unUser in listAdmin)
                    {
                        if(unUser.EmailConfirmed)
                        {
                            this.emailService.SendEmailCreationTicket(unUser.Email);
                        }                 
                    }
                }

                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Ticket, null, "Erreur Lors de la création d'un Ticket d'incident", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }


        public ActionControllerResult UpdateTicket(TicketIncident unTicket, bool userIsAdminTicket, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unTicket.DateModification = DateTime.Now;

                if(userIsAdminTicket)
                {
                    unTicket.UtilisateurAdministrateur = this.userService.GetUserById(userId);
                }
                
                this.ticketRepository.Update(unTicket);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Ticket, null, "Erreur Lors de la modification d'un Ticket d'incident", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public void DeleteTicket(int id)
        {
            this.ticketRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public ActionControllerResult DeleteTicketByMaterielId(int id)
        {
            ActionControllerResult result;
            try
            {
                IEnumerable<TicketIncident> ticketsToDelete = this.ticketRepository.Get(
                            filter: f => f.Materiel.ID == id
                        );

                foreach (TicketIncident unTicket in ticketsToDelete)
                {
                    this.ticketRepository.Delete(unTicket.ID);
                }
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Ticket, null, "Erreur Lors de la suppression d'un Ticket d'incident", ex.Message, null);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
