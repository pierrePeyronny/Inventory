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
    public class ValidationInventaireService : IValidationInventaireService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<ValidationInventaire> validationRepository;
        private IUserService userService;
        private ILoggerService logService;

        public ValidationInventaireService(IUnitOfWork unitOfWork, IGenericRepository<ValidationInventaire> validationRepository, IUserService userService, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.validationRepository = validationRepository;
            this.userService = userService;
            this.logService = logService;
        }

        public IEnumerable<ValidationInventaire> GetValidations()
        {
            return this.validationRepository.Get(
                orderBy: f =>f.OrderBy(q =>q.DateCreation)
                );
        }


        public IEnumerable<ValidationInventaire> GetValidationByInventaireId(int id)
        {
            return this.validationRepository.Get(
                filter: f =>f.InventaireID == id,
                orderBy: q =>q.OrderBy(u =>u.DateCreation)
                );
        }

        public ValidationInventaire GetValidationById(object id)
        {
            return this.validationRepository.GetByID(id);
        }

        public ActionControllerResult InsertValidation(ValidationInventaire uneValidation, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                uneValidation.Utilisateur = this.userService.GetUserById(userId);
                uneValidation.DateCreation = DateTime.Now;
                this.validationRepository.Insert(uneValidation);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.ValidationInventaire, null, "Erreur Lors de la validation d'un inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void DeleteValidation(int id)
        {
            this.validationRepository.Delete(id);
            this.unitOfWork.Save();
        }


        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
