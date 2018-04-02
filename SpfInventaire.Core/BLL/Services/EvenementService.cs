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
    public class EvenementService : IEvenementService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Evenement> evenementRepository;
        private ILoggerService logService;

        public EvenementService(IUnitOfWork unitOfWork, IGenericRepository<Evenement> evenementRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.evenementRepository = evenementRepository;
            this.logService = logService;
        }

        public IEnumerable<Evenement> GetEvenements()
        {
            return this.evenementRepository.Get();
        }

        public IEnumerable<Evenement> GetEvenementsByType(EVENEMENT_TYPE type)
        {
            return this.evenementRepository.Get(
                filter: f =>f.Type == type
                );
        }

        public IEnumerable<Evenement> GetEvenementsByDate(DateTime dateEvenement)
        {
            return this.evenementRepository.Get(
                filter: f =>f.DateEvenement.Date == dateEvenement.Date
                );
        }

        public Evenement GetEvenementById(object id)
        {
            return this.evenementRepository.GetByID(id);
        }

        public Evenement IsDesinfectionToday(DateTime dateNow)
        {
            return this.evenementRepository.Get(
                filter: f =>f.DateEvenement == dateNow && f.Type == EVENEMENT_TYPE.Desinfection
                )
                .SingleOrDefault();
        } 

        public ActionControllerResult InsertEvenement(Evenement unEvenement, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unEvenement.DateCreation = DateTime.Now;
                unEvenement.DateModification = DateTime.Now;
                this.evenementRepository.Insert(unEvenement);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Evenement, null, "Erreur Lors de la création d'un événement", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult UpdateEvenement(Evenement unEvenement, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unEvenement.DateModification = DateTime.Now;
                this.evenementRepository.Update(unEvenement);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Evenement, unEvenement.ID, "Erreur Lors de la modification d'un événement", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void DeleteEvenement(int id)
        {
            this.evenementRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }


    }
}
