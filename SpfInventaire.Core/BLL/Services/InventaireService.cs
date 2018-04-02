using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class InventaireService : IInventaireService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Inventaire> inventaireRepository;
        private ILoggerService logService;


        public InventaireService(IUnitOfWork unitOfWork, IGenericRepository<Inventaire> inventaireRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.inventaireRepository = inventaireRepository;
            this.logService = logService;
        }

        public IEnumerable<Inventaire> GetInventaires()
        {
            return this.inventaireRepository.Get();
        }

        public IEnumerable<Inventaire> GetActiveNotStockInventaires()
        {
            return this.inventaireRepository.Get(
                filter: f =>f.Active == true && f.IsInventaireStock != true
                );
        }

        public IEnumerable<Inventaire> GetStockInventaires()
        {
            return this.inventaireRepository.Get(
                filter: f => f.IsInventaireStock == true
                );
        }

        public Inventaire GetInventaireById(object id)
        {
            return this.inventaireRepository.GetByID(id);
        }

        public ActionControllerResult InsertInventaire(Inventaire unInventaire, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unInventaire.DateCreation = DateTime.Now;
                unInventaire.DateModification = DateTime.Now;
                this.inventaireRepository.Insert(unInventaire);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Inventaire, null, "Erreur Lors de la création d'un inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public ActionControllerResult UpdateInventaire(Inventaire unInventaire, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unInventaire.DateModification = DateTime.Now;
                this.inventaireRepository.Update(unInventaire);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Inventaire, unInventaire.ID, "Erreur Lors de la modification d'un inventaire", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public void DeleteInventaire(int id)
        {
            this.inventaireRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
