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
    public class StockMaterielService : IStockMaterielService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<StockMateriel> stockMaterielRepository;
        private IGenericRepository<Materiel> materielRepository;
        private ILoggerService logService;

        public StockMaterielService(IUnitOfWork unitOfWork, IGenericRepository<StockMateriel> stockMaterielRepository, IGenericRepository<Materiel> materielRepository, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.stockMaterielRepository = stockMaterielRepository;
            this.materielRepository = materielRepository;
            this.logService = logService;
        }


        public IEnumerable<StockMateriel> GetStockMateriels()
        {
            return this.stockMaterielRepository.Get(
                filter: f => f.Supprime == false
                );
        }

        public IEnumerable<StockMateriel> GetStockMaterielByTypeMateriel(int untypeMaterielID)
        {
            return this.stockMaterielRepository.Get(
                filter: f =>f.TypeMaterielID == untypeMaterielID && f.Supprime == false,
                orderBy: o =>o.OrderBy(u =>u.Materiel.BlocInventaire.InventaireID).ThenBy(u =>u.DatePeremption)
                );
        }


        public IEnumerable<StockMateriel> GetStockMaterielByMaterielId(int materielID)
        {
            return this.stockMaterielRepository.Get(
                filter: f =>f.MaterielID == materielID && f.Supprime == false,
                orderBy: o => o.OrderBy(u => u.DatePeremption)
                );
        }

        public StockMateriel GetStockMaterielById(object id)
        {
            return this.stockMaterielRepository.GetByID(id);
        }


        public Dictionary<string, string> CreateListStockByMaterielId(int materielId)
        {
            IEnumerable<StockMateriel> listStock = this.GetStockMaterielByMaterielId(materielId);
            Dictionary<string, string> dictionaryItem = new Dictionary<string, string>();
            DateTime tempDate = new DateTime();

            foreach (StockMateriel unStock in listStock)
            {
                if (unStock.DatePeremption != null)
                {
                    tempDate = Convert.ToDateTime(unStock.DatePeremption);
                    dictionaryItem.Add(unStock.ID.ToString(), "Date péremption: " + tempDate.ToString("yyyy/MM/dd"));
                }
                else
                {
                    dictionaryItem.Add(unStock.ID.ToString(), unStock.TypeMateriel.Nom + " (quantité: " + unStock.Quantite + ")");
                }
            }
            return dictionaryItem;
        }


        public ActionControllerResult InsertStockMateriel(StockMateriel unStock, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                Materiel unMateriel = this.materielRepository.GetByID(unStock.MaterielID);
                if(unMateriel != null)
                {
                    if(unMateriel.TypeMaterielID > 0)
                    {
                        unStock.TypeMaterielID = Convert.ToInt32(unMateriel.TypeMaterielID);
                    }else
                    {
                        unStock.TypeMaterielID = 0;
                    }
                }
                else
                {
                    unStock.TypeMaterielID = 0;
                }

                StockMateriel stockDejaExistant = this.stockMaterielRepository.Get(
                    filter: f =>f.MaterielID == unStock.MaterielID && f.DatePeremption == unStock.DatePeremption && f.Supprime == false
                    ).SingleOrDefault();

                if(stockDejaExistant != null)
                {
                    stockDejaExistant.Quantite += unStock.Quantite;
                    stockDejaExistant.DateModification = DateTime.Now;
                    this.stockMaterielRepository.Update(stockDejaExistant);
                }
                else
                {
                    unStock.DateCreation = DateTime.Now;
                    unStock.DateModification = DateTime.Now;
                    this.stockMaterielRepository.Insert(unStock);
                }

                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.StockMateriel, null, "Erreur Lors de la création d'un Stock Materiel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }


        public ActionControllerResult UpdateStockMateriel(StockMateriel unStock, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                unStock.DateModification = DateTime.Now;
                this.stockMaterielRepository.Update(unStock);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.StockMateriel, null, "Erreur Lors de la modification d'un Stock Materiel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }


        public ActionControllerResult UtilisationStockMateriel(StockMateriel stockSortie, int quantite, string userId = null)
        {
            ActionControllerResult result;

            try
            {
                //si quantité ne suffit pas alors on recherche un autre stock similaire
                if (stockSortie.Quantite < quantite)
                {
                    quantite -= stockSortie.Quantite;
                    stockSortie.Quantite = 0;
                    this.unitOfWork.Save();

                    StockMateriel autreStock = this.stockMaterielRepository.Get(
                        filter: f => f.ID != stockSortie.ID && f.MaterielID == stockSortie.MaterielID && f.Supprime == false,
                        orderBy: u => u.OrderBy(o => o.DatePeremption)
                        ).ToList().FirstOrDefault();

                    if (autreStock != null)
                    {
                        autreStock.Quantite -= quantite;
                        if(autreStock.Quantite <=0)
                        {
                            autreStock.Supprime = true;
                            autreStock.Quantite = 0;
                        }
                        autreStock.DateModification = DateTime.Now;
                    }
                }
                else
                {
                    stockSortie.Quantite -= quantite;                   
                }

                if(stockSortie.Quantite == 0)
                {
                    stockSortie.Supprime = true;
                }

                stockSortie.DateModification = DateTime.Now;
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.StockMateriel, null, "Erreur Lors du transfert d'un Stock Materiel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }



        public ActionControllerResult TransfertStockMateriel(int stockMaterielSourceId, StockMateriel stockTransfert, string userId = null)
        {
            ActionControllerResult result;
            try
            {
                //Récupération du stockSource
                StockMateriel stockSource = this.GetStockMaterielById(stockMaterielSourceId);
                Materiel materielDestination = this.materielRepository.GetByID(stockTransfert.MaterielID);

                if(stockSource != null && materielDestination != null)
                {
                    //Si le type matériel est le meme
                    if(stockSource.Materiel.TypeMaterielID == materielDestination.TypeMaterielID)
                    {
                        //Recherche de l'existance d'un stock similaire à la destination données
                        StockMateriel stockSimilaireDestination = this.stockMaterielRepository.Get(
                            filter: f =>f.MaterielID == stockTransfert.MaterielID && f.DatePeremption == stockSource.DatePeremption && f.Supprime == false
                            )
                            .SingleOrDefault();

                        if(stockSimilaireDestination != null)
                        {
                            //Si oui, transfert de la quantité donnée à la destinations donnée
                            //Update du stock Source
                            result = TakeFromSource(stockTransfert, stockSource);

                            //Mise à jour Stock de destination si Update source Réussi
                            if (result == ActionControllerResult.SUCCESS)
                            {
                                stockSimilaireDestination.Quantite += stockTransfert.Quantite;
                                stockSimilaireDestination.DateModification = DateTime.Now;

                                result = this.UpdateStockMateriel(stockSimilaireDestination);
                            }
                        }
                        else
                        {
                            //Si non, Création d'un nouveau stock à la destination
                            //Update du stock Source
                            result = TakeFromSource(stockTransfert, stockSource);

                            //Creation du nouveau stock si Update source Réussi
                            if (result == ActionControllerResult.SUCCESS)
                            {
                                StockMateriel newStock = new StockMateriel();
                                newStock.MaterielID = stockTransfert.MaterielID;
                                newStock.DatePeremption = stockSource.DatePeremption;
                                newStock.Quantite = stockTransfert.Quantite;
                                newStock.TypeMaterielID = stockSource.TypeMaterielID;
                                newStock.Supprime = false;
                                newStock.DateCreation = DateTime.Now;
                                newStock.DateModification = DateTime.Now;

                                result = this.InsertStockMateriel(newStock, userId);
                            }                                              
                        }
                    }
                    else
                    {
                        result = ActionControllerResult.FAILURE;
                    }
                }
                else
                {
                    result = ActionControllerResult.FAILURE;
                }
                
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.StockMateriel, null, "Erreur Lors du transfert d'un Stock Materiel", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        private ActionControllerResult TakeFromSource(StockMateriel stockTransfert, StockMateriel stockSource)
        {
            ActionControllerResult result;

            int quantiteRestante = 0;
            stockSource.Quantite -= stockTransfert.Quantite;
            if (stockSource.Quantite <= 0)
            {
                stockSource.Supprime = true;
                quantiteRestante = stockSource.Quantite * -1;
                stockSource.Quantite = 0;
            }
            result = this.UpdateStockMateriel(stockSource);

            //S'il reste une quantité à enlever
            if (quantiteRestante > 0)
            {
                StockMateriel stockPourQuantiteRestante = this.stockMaterielRepository.Get(
                        filter: f => f.MaterielID == stockSource.MaterielID && f.Supprime == false,
                        orderBy: u => u.OrderBy(o => o.DatePeremption)
                        )
                        .SingleOrDefault();

                if (stockPourQuantiteRestante != null)
                {
                    stockPourQuantiteRestante.Quantite -= quantiteRestante;
                    if (stockPourQuantiteRestante.Quantite <= 0)
                    {
                        stockPourQuantiteRestante.Supprime = true;
                        stockPourQuantiteRestante.Quantite = 0;
                    }
                    result = this.UpdateStockMateriel(stockPourQuantiteRestante);
                }
            }
            return result;
        }

        public void DeleteStockMateriel(int id)
        {
            this.stockMaterielRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }


    }
}
