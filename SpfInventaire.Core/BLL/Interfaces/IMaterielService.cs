using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IMaterielService
    {
        Constantes.ActionControllerResult ChangeOrdreMateriel(int Id, int newOrdre, string userId = null);
        ActionControllerResult DeleteMateriel(int id);
        Materiel GetMaterielById(object id);
        IEnumerable<Materiel> GetMateriels();
        IEnumerable<Materiel> GetMaterielsBySearch(string search);
        IEnumerable<Materiel> GetMaterielsByBlocInventaire(int blocInventaireID);
        IEnumerable<Materiel> GetMaterielsListe();
        IEnumerable<Materiel> GetMaterielsByInventaire(int inventaireID);
        Constantes.ActionControllerResult InsertMateriel(Materiel unMateriel, string userId = null);
        Constantes.ActionControllerResult UpdateMateriel(Materiel unMateriel, string userId = null);
    }
}