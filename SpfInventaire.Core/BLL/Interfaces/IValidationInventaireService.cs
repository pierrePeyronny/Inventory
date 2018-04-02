using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IValidationInventaireService : IDisposable
    {
        void DeleteValidation(int id);
        ValidationInventaire GetValidationById(object id);
        IEnumerable<ValidationInventaire> GetValidationByInventaireId(int id);
        IEnumerable<ValidationInventaire> GetValidations();
        Constantes.ActionControllerResult InsertValidation(ValidationInventaire uneValidation, string userId = null);
    }
}