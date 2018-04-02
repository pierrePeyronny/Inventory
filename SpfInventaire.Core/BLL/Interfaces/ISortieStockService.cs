using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface ISortieStockService : IDisposable
    {
        void DeleteSortieStock(int id);
        IEnumerable<SortieStockMateriel> GetSortieStock();
        SortieStockMateriel GetSortieStockById(object id);
        IEnumerable<SortieStockMateriel> GetSortieStockByRaisonSortie(MATERIEL_RAISON_SORTIE uneRaison);
        Constantes.ActionControllerResult InsertSortieStock(SortieStockMateriel uneSortie, string userId = null);
        Constantes.ActionControllerResult InsertSortieStock(int stockSortieID, int stockEntreeID, int quantite, string userId = null);
    }
}