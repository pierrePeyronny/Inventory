using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpfInventaire.Core.DAL;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.BLL.Interfaces;
using static SpfInventaire.Core.BLL.Constantes;
using Microsoft.AspNet.Identity;
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity.Owin;
using SpfInventaire.Core.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.DAL.ViewModels;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService userService;
        private IRoleService roleService;
        private ILoggerService logService;

        public UserController(IUserService userService, ILoggerService logService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.logService = logService;
        }

        // GET: User
        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.userService.GetUsers());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            DetailUserViewModels detailModel = new DetailUserViewModels();

            if (id == null)
            {
                string ErrorMessage = "Id Utilisateur manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            detailModel.unUser = this.userService.GetUserById(id);
            if (detailModel.unUser == null)
            {
                return HttpNotFound();
            }

            detailModel.listRolesUser = this.roleService.GetRoleByUserId(detailModel.unUser.Id);
            detailModel.listAllRoles = new SelectList(this.roleService.GetRoles(), "Id", "Name");

            return View(detailModel);
        }

        public ActionResult MonCompte(string sModifier = "")
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                string ErrorMessage = "Id Utilisateur manquant";
                return RedirectToAction("Index","Home", new { MessageErreur = ErrorMessage });
            }
            ApplicationUser unUser = this.userService.GetUserById(id);
            if (unUser == null)
            {
                return HttpNotFound();
            }

            //Si le compte à été modifier on affiche un message
            if(!String.IsNullOrEmpty(sModifier))
            {
                ViewBag.StatusMessage = sModifier;
            }

            return View(unUser);
        }

        public ActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.userService.ChangeEmail(model.newEmail, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(model);
                }

                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.User, null, "Changement Email", null, User.Identity.GetUserId());
                return RedirectToAction("MonCompte");
            }
            return View(model);
        }


        // GET: User/Edit/5
        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Utilisateur manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            ApplicationUser unUser = this.userService.GetUserById(id);
            if (unUser == null)
            {
                return HttpNotFound();
            }
            return View(unUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Prenom,Surnom,Email,UserName")] ApplicationUser unUser)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.userService.UpdateUser(unUser, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(unUser);
                }
                
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.User, null, "Modification d'un Utilisateur", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(unUser);
        }

        // GET: User/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Utilisateur manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            ApplicationUser unUser = this.userService.GetUserById(id);
            if (unUser == null)
            {
                return HttpNotFound();
            }
            return View(unUser);
        }

        // POST: User/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ActionControllerResult resultLog = this.logService.DeleteLogsByUserId(id);

            if(resultLog == ActionControllerResult.SUCCESS)
            {
                this.userService.DeleteUser(id);
                return RedirectToAction("Index");
            }
            else
            {
                string ErrorMessage = "Erreur lors de la suppression de l'utilisateur";
                return RedirectToAction("Index", "Home", new { MessageErreur = ErrorMessage });
            }           
        }



        public ActionResult ManageUsersRoles()
        {
            this.PopulateRolesDropDownList();
            this.PopulateUsersDropDownList();
            return View();
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost]
        public ActionResult RoleAddToUser(string roleId, string userId)
        {
            ActionControllerResult result = this.roleService.AffecterRole(userId, roleId);
            if (result == ActionControllerResult.FAILURE)
            {
                return Json(string.Empty);
            }

            this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.AssociationRoleToUser, null, "Affectation d'un role à un utilisateur", null, User.Identity.GetUserId());
            return this.GetUserRoles(userId);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserRoles(string userId = null)
        {
            if(userId != null)
            {
                IEnumerable<IdentityRole> listRoles = this.roleService.GetRoleByUserId(userId);

                return PartialView("_IndexRolesUser", listRoles);
            }
            else
            {
                return Json(string.Empty);
            }
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost]
        public ActionResult DeleteRoleForUser(string roleId, string userId = null)
        {
            ActionControllerResult result = this.roleService.EnleverRole(userId, roleId);
            if (result == ActionControllerResult.FAILURE)
            {
                return Json(string.Empty);
            }

            //Réussi
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.AssociationRoleToUser, null, "Enlévement d'un role à un utilisateur", null, User.Identity.GetUserId());

            return this.GetUserRoles(userId);
        }


        private void PopulateRolesDropDownList()
        {
            ViewBag.Roles = new SelectList(this.roleService.GetRoles(), "Id", "Name");
        }

        private void PopulateUsersDropDownList()
        {
            IEnumerable<SelectListItem> listeUsers = from u in this.userService.GetUsers()
                                                     select new SelectListItem
                                                     {
                                                         Value = u.Id,
                                                         Text = u.Nom + " " + u.Prenom
                                                     };

            ViewBag.Users = new SelectList(listeUsers, "Value", "Text");
        }
    }
}
