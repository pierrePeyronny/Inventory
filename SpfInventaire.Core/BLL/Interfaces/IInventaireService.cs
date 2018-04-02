using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IInventaireService : IDisposable
    {
        void DeleteInventaire(int id);
        Inventaire GetInventaireById(object id);
        IEnumerable<Inventaire> GetActiveNotStockInventaires();
        IEnumerable<Inventaire> GetStockInventaires();
        IEnumerable<Inventaire> GetInventaires();
        ActionControllerResult InsertInventaire(Inventaire unInventaire, string userId = null);
        ActionControllerResult UpdateInventaire(Inventaire unInventaire, string userId = null);
    }
}