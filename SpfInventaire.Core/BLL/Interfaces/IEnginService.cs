using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IEnginService :IDisposable
    {
        void DeleteEngin(int id);
        void DeletePlein(int id);
        Engin GetEnginById(object enginId);
        Constantes.ActionControllerResult InsertEngin(Engin unEngin, string userId = null);
        Constantes.ActionControllerResult InsertPlein(PleinEssence unPlein, string userId = null);
        Constantes.ActionControllerResult UpdatePlein(PleinEssence unPlein, string userId = null);
        IEnumerable<Engin> ListEngins();
        IEnumerable<PleinEssence> ListPleinByEnginID(int enginID);
        PleinEssence GetPleinById(object id);
        Constantes.ActionControllerResult UpdateEngin(Engin unEngin, string userId = null);
    }
}