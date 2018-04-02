using System;
using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IEvenementService : IDisposable
    {
        void DeleteEvenement(int id);
        Evenement GetEvenementById(object id);
        IEnumerable<Evenement> GetEvenements();
        IEnumerable<Evenement> GetEvenementsByDate(DateTime dateEvenement);
        IEnumerable<Evenement> GetEvenementsByType(EVENEMENT_TYPE type);
        Evenement IsDesinfectionToday(DateTime dateNow);
        Constantes.ActionControllerResult InsertEvenement(Evenement unEvenement, string userId = null);
        Constantes.ActionControllerResult UpdateEvenement(Evenement unEvenement, string userId = null);
    }
}