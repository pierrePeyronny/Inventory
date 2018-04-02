using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface ITypeMaterielService : IDisposable
    {
        IEnumerable<TypeMateriel> GetTypeMaterielByDomaine(MATERIEL_TYPE_DOMAINE domaine);
        IEnumerable<TypeMateriel> GetTypeMateriels();
        TypeMateriel GetTypeMaterielById(object id);
        Constantes.ActionControllerResult InsertTypeMateriel(TypeMateriel unTypeMateriel, string userId = null);
        Constantes.ActionControllerResult UpdateTypeMateriel(TypeMateriel unTypeMateriel, string userId = null);
        void DeleteTypeMateriel(int id);
        Constantes.ActionControllerResult GenerationTypeMaterielFromMateriel();
    }
}