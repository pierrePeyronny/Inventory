using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class BlocInventaireService : IBlocInventaireService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<BlocInventaire> blocInventaireRepository;
        private ILoggerService logService;

        public BlocInventaireService(IUnitOfWork unitOfWork, IGenericRepository<BlocInventaire> blocInventaireRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.blocInventaireRepository = blocInventaireRepository;
            this.logService = logService;
        }

        public IEnumerable<BlocInventaire> GetBlocsInventaire()
        {
            return this.blocInventaireRepository.Get(includeProperties: "Inventaire");
        }

        public IEnumerable<BlocInventaire> GetBlocsInventaireListe()
        {
            return this.blocInventaireRepository.Get(
                orderBy: q => q.OrderBy(f => f.Rank),
                filter: f => f.Active == true
                );
        }

        public IEnumerable<BlocInventaire> GetBlocsInventaireByInventaire(int inventaireID)
        {
            return this.blocInventaireRepository.Get(
                orderBy: q =>q.OrderBy(f => f.Rank),
                filter: f => f.InventaireID == inventaireID && f.Active == true
                );
        }

        public BlocInventaire GetBlocInventaireById(object id)
        {
            return this.blocInventaireRepository.GetByID(id);
        }

        public ActionControllerResult InsertBlocInventaire(BlocInventaire unBlocInventaire, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unBlocInventaire.DateCreation = DateTime.Now;
                unBlocInventaire.DateModification = DateTime.Now;
                this.blocInventaireRepository.Insert(unBlocInventaire);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.BlocInventaire, null, "Erreur Lors de la création d'un bloc inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult UpdateBlocInventaire(BlocInventaire unBlocInventaire, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unBlocInventaire.DateModification = DateTime.Now;
                this.blocInventaireRepository.Update(unBlocInventaire);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.BlocInventaire, null, "Erreur Lors de la modification d'un bloc inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult ChangeOrdreBlocInventaire(int Id, int newOrdre, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                BlocInventaire unBlocInventaire = this.blocInventaireRepository.GetByID(Id);
                if (unBlocInventaire == null)
                {
                    return ActionControllerResult.FAILURE;
                }

                unBlocInventaire.DateModification = DateTime.Now;
                unBlocInventaire.Rank = newOrdre;
                this.blocInventaireRepository.Update(unBlocInventaire);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.BlocInventaire, null, "Erreur Lors de la modification de l'ordre d'un bloc inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void DeleteBlocInventaire(int id)
        {
            this.blocInventaireRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
