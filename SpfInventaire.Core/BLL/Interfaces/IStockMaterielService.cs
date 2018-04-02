using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IStockMaterielService : IDisposable
    {
        void DeleteStockMateriel(int id);
        StockMateriel GetStockMaterielById(object id);
        IEnumerable<StockMateriel> GetStockMaterielByMaterielId(int materielID);
        IEnumerable<StockMateriel> GetStockMaterielByTypeMateriel(int untypeMaterielID);
        IEnumerable<StockMateriel> GetStockMateriels();
        Dictionary<string, string> CreateListStockByMaterielId(int materielId);
        Constantes.ActionControllerResult InsertStockMateriel(StockMateriel unStock, string userId = null);
        Constantes.ActionControllerResult UpdateStockMateriel(StockMateriel unStock, string userId = null);
        Constantes.ActionControllerResult TransfertStockMateriel(int stockMaterielSourceId, StockMateriel stockTransfert, string userId = null);
        Constantes.ActionControllerResult UtilisationStockMateriel(StockMateriel unStock, int quantite, string userId = null);
    }
}