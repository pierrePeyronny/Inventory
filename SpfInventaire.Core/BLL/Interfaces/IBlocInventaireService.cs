using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IBlocInventaireService : IDisposable
    {
        Constantes.ActionControllerResult ChangeOrdreBlocInventaire(int Id, int newOrdre, string userId = null);
        void DeleteBlocInventaire(int id);
        BlocInventaire GetBlocInventaireById(object id);
        IEnumerable<BlocInventaire> GetBlocsInventaire();
        IEnumerable<BlocInventaire> GetBlocsInventaireByInventaire(int inventaireID);
        IEnumerable<BlocInventaire> GetBlocsInventaireListe();
        Constantes.ActionControllerResult InsertBlocInventaire(BlocInventaire unBlocInventaire, string userId = null);
        Constantes.ActionControllerResult UpdateBlocInventaire(BlocInventaire unBlocInventaire, string userId = null);
    }
}