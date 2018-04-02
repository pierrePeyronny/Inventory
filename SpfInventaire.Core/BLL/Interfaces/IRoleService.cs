using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IRoleService : IDisposable
    {
        ActionControllerResult InsertRole(IdentityRole unRole);

        IEnumerable<IdentityRole> GetRoles();
        ActionControllerResult AffecterRole(string userId, string roleId);
        ActionControllerResult EnleverRole(string userId, string roleId);
        IEnumerable<IdentityRole> GetRoleByUserId(string id);
        IdentityRole GetRoleById(object id);
        IdentityRole GetRoleByName(string name);
        ActionControllerResult UpdateRole(IdentityRole unRole);
        void DeleteRole(string id);

    }
}