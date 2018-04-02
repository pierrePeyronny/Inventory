using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        private IGenericRepository<ApplicationUser> userRepository;
        private IRoleService roleService;
        private ILoggerService logService;
        

        public UserService(IUnitOfWork unitOfWork, IGenericRepository<ApplicationUser> userRepository, IRoleService roleService, ILoggerService logService)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.roleService = roleService;
            this.logService = logService;
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return this.userRepository.Get(
                orderBy: q => q.OrderBy(f => f.Nom)
                );
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string role)
        {
            IdentityRole roleId = this.roleService.GetRoleByName(role);

            return this.userRepository.Get(
                filter: f =>f.Roles.Any(u =>u.RoleId == roleId.Id)
                );
        }

        public ApplicationUser GetUserById(object id)
        {
            return this.userRepository.GetByID(id);
        }

        public ActionControllerResult InsertUser(ApplicationUser unUser, string userId)
        {
            ActionControllerResult result;
            try
            {
                unUser.PremiereConnexion = true;
                unUser.EmailConfirmed = true;
                this.userRepository.Insert(unUser);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.User, null, "Erreur Lors de la création d'un Utilisateur", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult UpdateUser(ApplicationUser unUser, string userId)
        {
            ActionControllerResult result;
            try
            {
                ApplicationUser userToUpdate = this.GetUserById(unUser.Id);
                userToUpdate.Nom = unUser.Nom;
                userToUpdate.Prenom = unUser.Prenom;
                userToUpdate.Surnom = unUser.Surnom;
                userToUpdate.Email = unUser.Email;
                userToUpdate.UserName = unUser.UserName;
                userToUpdate.EmailConfirmed = true;

                this.userRepository.Update(userToUpdate);
                this.unitOfWork.Save();
                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.User, null, "Erreur Lors de la modification d'un Utilisateur", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public ActionControllerResult ChangeEmail(string newMail, string userId)
        {
            ActionControllerResult result;
            try
            {
                ApplicationUser userToUpdate = this.GetUserById(userId);

                if(userToUpdate != null)
                {
                    userToUpdate.Email = newMail;
                    userToUpdate.EmailConfirmed = true;

                    this.userRepository.Update(userToUpdate);
                    this.unitOfWork.Save();
                    result = ActionControllerResult.SUCCESS;
                }
                else
                {
                    result = ActionControllerResult.FAILURE;
                }

            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.User, null, "Erreur Lors de la modification d'un Utilisateur", ex.Message, userId);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        public void DeleteUser(string id)
        {
            this.userRepository.Delete(id);
            this.unitOfWork.Save();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }

    }
}
