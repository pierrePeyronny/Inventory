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
    public class MaterielService : IMaterielService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Materiel> materielRepository;
        private ITicketIncidentService ticketService;
        private ILoggerService logService;

        public MaterielService(IUnitOfWork unitOfWork, IGenericRepository<Materiel> materielRepository, ITicketIncidentService ticketService, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.materielRepository = materielRepository;
            this.ticketService = ticketService;
            this.logService = logService;
        }

        public IEnumerable<Materiel> GetMateriels()
        {
            return this.materielRepository.Get(includeProperties: "BlocInventaire");
        }

        public IEnumerable<Materiel> GetMaterielsListe()
        {
            return this.materielRepository.Get(
                orderBy: q => q.OrderBy(f => f.Nom),
                filter: f => f.Active == true
                );
        }

        public IEnumerable<Materiel> GetMaterielsBySearch(string search)
        {
            return this.materielRepository.Get(
                filter: f => f.TypeMateriel.Nom.Contains(search) && f.BlocInventaire.Inventaire.IsInventaireStock == false
                );
        }

        public IEnumerable<Materiel> GetMaterielsByBlocInventaire(int blocInventaireID)
        {
            return this.materielRepository.Get(
                orderBy: q => q.OrderBy(f => f.Rank),
                filter: f => f.BlocInventaireID == blocInventaireID && f.Active == true
                );
        }

        public IEnumerable<Materiel> GetMaterielsByInventaire(int inventaireID)
        {
            return this.materielRepository.Get(
                orderBy: q => q.OrderBy(f => f.TypeMateriel.Nom),
                filter: f => f.BlocInventaire.InventaireID == inventaireID && f.Active == true
                );
        }

        public Materiel GetMaterielById(object id)
        {
            return this.materielRepository.GetByID(id);
        }

        public ActionControllerResult InsertMateriel(Materiel unMateriel, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unMateriel.DateCreation = DateTime.Now;
                unMateriel.DateModification = DateTime.Now;
                this.materielRepository.Insert(unMateriel);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Materiel, null, "Erreur Lors de la création d'un matériel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult UpdateMateriel(Materiel unMateriel, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unMateriel.DateModification = DateTime.Now;
                this.materielRepository.Update(unMateriel);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Materiel, null, "Erreur Lors de la modification d'un materiel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult ChangeOrdreMateriel(int Id, int newOrdre, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                Materiel unMateriel = this.materielRepository.GetByID(Id);
                if (unMateriel == null)
                {
                    return ActionControllerResult.FAILURE;
                }

                unMateriel.DateModification = DateTime.Now;
                unMateriel.Rank = newOrdre;
                this.materielRepository.Update(unMateriel);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Materiel, null, "Erreur Lors de la modification de l'ordre d'un matériel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult DeleteMateriel(int id)
        {
            ActionControllerResult result;
            try
            {
                ActionControllerResult resultTicket = this.ticketService.DeleteTicketByMaterielId(id);

                if(resultTicket == ActionControllerResult.SUCCESS)
                {
                    this.materielRepository.Delete(id);
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
                this.logService.LogErreur(LOG_TYPE_OBJECT.Materiel, null, "Erreur Lors de la suppression d'un materiel", ex.Message, null);
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
