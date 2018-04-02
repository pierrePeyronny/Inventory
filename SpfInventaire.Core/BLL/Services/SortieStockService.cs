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
    public class SortieStockService : ISortieStockService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<SortieStockMateriel> sortieStockRepository;
        private IStockMaterielService stockService;
        private IUserService userService;
        private ILoggerService logService;

        public SortieStockService(IUnitOfWork unitOfWork, IGenericRepository<SortieStockMateriel> sortieStockRepository, IStockMaterielService stockService, IUserService userService, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.sortieStockRepository = sortieStockRepository;
            this.stockService = stockService;
            this.userService = userService;
            this.logService = logService;
        }


        public IEnumerable<SortieStockMateriel> GetSortieStock()
        {
            return this.sortieStockRepository.Get(
                orderBy: o => o.OrderByDescending(u => u.DateSortie)
                ).Take(50);
        }

        public IEnumerable<SortieStockMateriel> GetSortieStockByRaisonSortie(MATERIEL_RAISON_SORTIE uneRaison)
        {
            return this.sortieStockRepository.Get(
                filter: f => f.Raison == uneRaison,
                orderBy: o => o.OrderBy(u => u.DateSortie)
                );
        }

        public SortieStockMateriel GetSortieStockById(object id)
        {
            return this.sortieStockRepository.GetByID(id);
        }


        public ActionControllerResult InsertSortieStock(SortieStockMateriel uneSortie, string userId = null)
        {
            ActionControllerResult result = this.executeSortieStock(uneSortie.UsedStockMaterielID, uneSortie.IdSourceStock, uneSortie.Quantite, userId);          
            return result;
        }

        public ActionControllerResult InsertSortieStock(int stockSortieID, int stockEntreeID, int quantite, string userId = null)
        {
            ActionControllerResult result = executeSortieStock(stockSortieID, stockEntreeID, quantite, userId);
            return result;
        }

        private ActionControllerResult executeSortieStock(int stockSortieID, int stockEntreeID, int quantite, string userId)
        {
            ActionControllerResult result;
            SortieStockMateriel uneSortie = new SortieStockMateriel();

            try
            {
                StockMateriel stockSortie = this.stockService.GetStockMaterielById(stockSortieID);
                StockMateriel stockSource = this.stockService.GetStockMaterielById(stockEntreeID);

                if (stockSortie.TypeMaterielID == stockSource.TypeMaterielID)
                {
                    //Suppression de la quantité du stock
                    result = this.stockService.UtilisationStockMateriel(stockSortie, quantite, userId);

                    if (result == ActionControllerResult.SUCCESS)
                    {
                        //Transfert du stock source vers le stock de sortie
                        StockMateriel tempSortieStock = new StockMateriel();
                        tempSortieStock.MaterielID = stockSortie.MaterielID;
                        tempSortieStock.Quantite = quantite;
                        tempSortieStock.TypeMaterielID = stockSortie.TypeMaterielID;

                        result = this.stockService.TransfertStockMateriel(stockSource.ID, tempSortieStock);

                        if (result == ActionControllerResult.SUCCESS)
                        {
                            //Si tout est ok, on créer la Sortie de Matériel
                            uneSortie.UsedStockMaterielID = stockSortieID;
                            uneSortie.IdSourceStock = stockEntreeID;
                            uneSortie.Quantite = quantite;
                            uneSortie.Utilisateur = this.userService.GetUserById(userId);
                            uneSortie.DateSortie = DateTime.Now;
                            uneSortie.TypeSortie = MATERIEL_TYPE_SORTIE.Intervention;
                            uneSortie.Raison = MATERIEL_RAISON_SORTIE.Intervention;
                            this.sortieStockRepository.Insert(uneSortie);
                            this.unitOfWork.Save();
                            result = ActionControllerResult.SUCCESS;
                        }
                        else
                        {
                            result = ActionControllerResult.FAILURE;
                        }
                    }
                }
                else
                {
                    result = ActionControllerResult.FAILURE;
                }
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.SortieStockMateriel, null, "Erreur Lors de la création d'une Sortie de Stock", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public void DeleteSortieStock(int id)
        {
            this.sortieStockRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }


    }
}
