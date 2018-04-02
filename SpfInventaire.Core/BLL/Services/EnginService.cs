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
    public class EnginService : IEnginService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Engin> enginRepository;
        private IGenericRepository<PleinEssence> pleinRepository;
        private ILoggerService logService;

        public EnginService(IUnitOfWork unitOfWork, IGenericRepository<Engin> enginRepository, IGenericRepository<PleinEssence> pleinRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.enginRepository = enginRepository;
            this.pleinRepository = pleinRepository;
            this.logService = logService;
        }


        public IEnumerable<Engin> ListEngins()
        {
            return this.enginRepository.Get();
        }

        public Engin GetEnginById(object enginId)
        {
            return this.enginRepository.GetByID(enginId);
        }


        public ActionControllerResult InsertEngin(Engin unEngin, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unEngin.DateCreation = DateTime.Now;
                unEngin.DateModification = DateTime.Now;
                this.enginRepository.Insert(unEngin);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Engin, null, "Erreur Lors de la création d'un Engin", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }


        public ActionControllerResult UpdateEngin(Engin unEngin, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unEngin.DateModification = DateTime.Now;
                this.enginRepository.Update(unEngin);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Engin, null, "Erreur Lors de la modification d'un Engin", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }


        public IEnumerable<PleinEssence> ListPleinByEnginID(int enginID)
        {
            return this.pleinRepository.Get(
                filter: f => f.EnginID == enginID
                );
        }

        public PleinEssence GetPleinById(object id)
        {
            return this.pleinRepository.GetByID(id);
        }

        public ActionControllerResult InsertPlein(PleinEssence unPlein, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unPlein.DateCreation = DateTime.Now;
                this.pleinRepository.Insert(unPlein);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.PleinEssence, null, "Erreur Lors de la création d'un Plein d'essence", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public ActionControllerResult UpdatePlein(PleinEssence unPlein, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                this.pleinRepository.Update(unPlein);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.PleinEssence, null, "Erreur Lors de la modification d'un Plein d'essence", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }


        public void DeleteEngin(int id)
        {
            this.enginRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void DeletePlein(int id)
        {
            this.pleinRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
