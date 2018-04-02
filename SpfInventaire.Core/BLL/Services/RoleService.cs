using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<IdentityRole> roleRepository;
        private ILoggerService logService;
        private IGenericRepository<IdentityUserRole> userRoleRepository;

        public RoleService(IUnitOfWork unitOfWork, IGenericRepository<IdentityRole> roleRepository, ILoggerService logService, IGenericRepository<IdentityUserRole> userRoleRepository)
        {
            this.unitOfWork = unitOfWork;
            this.roleRepository = roleRepository;
            this.logService = logService;
            this.userRoleRepository = userRoleRepository;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return this.roleRepository.Get(
                 orderBy: q => q.OrderBy(f => f.Name)
                 );
        }

        public ActionControllerResult AffecterRole(string userId, string roleId)
        {
            ActionControllerResult result;
            try
            {
                IdentityUserRole associationUserRole = new IdentityUserRole();
                associationUserRole.RoleId = roleId;
                associationUserRole.UserId = userId;
                this.userRoleRepository.Insert(associationUserRole);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.AssociationRoleToUser, null, "Erreur Lors de l'ajout d'un role à l'utilisateur", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public IEnumerable<IdentityRole> GetRoleByUserId(string id)
        {
            return this.roleRepository.Get(
                filter: q => q.Users.Any(a => a.UserId == id)
                );
        }

        public ActionControllerResult EnleverRole(string userId, string roleId)
        {
            ActionControllerResult result;
            try
            {
                IdentityUserRole associationUserRole = new IdentityUserRole();
                associationUserRole.RoleId = roleId;
                associationUserRole.UserId = userId;
                this.userRoleRepository.Delete(associationUserRole);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.AssociationRoleToUser, null, "Erreur Lors de l'enlévement d'un role à l'utilisateur", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public IdentityRole GetRoleById(object id)
        {
            return this.roleRepository.GetByID(id);
        }

        public IdentityRole GetRoleByName(string name)
        {
            return this.roleRepository.Get(
                filter: f =>f.Name == name
                ).SingleOrDefault();
        }


        public ActionControllerResult InsertRole(IdentityRole unRole)
        {
            ActionControllerResult result;
            try
            {
                this.roleRepository.Insert(unRole);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Role, null, "Erreur Lors de la création d'un inventaire", ex.Message, null);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public ActionControllerResult UpdateRole(IdentityRole unRole)
        {
            ActionControllerResult result;
            try
            {
                this.roleRepository.Update(unRole);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Role, null, "Erreur Lors de la création d'un inventaire", ex.Message, null);
                result = ActionControllerResult.FAILURE;
            }

            return result;
        }

        public void DeleteRole(string id)
        {
            this.roleRepository.Delete(id);
            this.unitOfWork.Save();
        }


        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
