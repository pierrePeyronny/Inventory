using System.Collections.Generic;
using SpfInventaire.Core.DAL.Models;
using System;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        void DeleteUser(string id);
        ApplicationUser GetUserById(object id);
        IEnumerable<ApplicationUser> GetUsers();
        IEnumerable<ApplicationUser> GetUsersByRole(string role);
        Constantes.ActionControllerResult InsertUser(ApplicationUser unUser, string userId);
        Constantes.ActionControllerResult UpdateUser(ApplicationUser unUser, string userId);
        ActionControllerResult ChangeEmail(string newMail, string userId);
    }
}