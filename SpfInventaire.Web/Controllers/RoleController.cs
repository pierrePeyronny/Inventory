using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.BLL;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Web.Controllers
{
    [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
    public class RoleController : Controller
    {

        private IRoleService roleService;
        private ILoggerService logService;

        public RoleController(IRoleService roleService, ILoggerService logService)
        {
            this.roleService = roleService;
            this.logService = logService;
        }


        // GET: Role
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.roleService.GetRoles());
        }

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole unRole)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.roleService.InsertRole(unRole);

                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(unRole);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Role, null, "Création du role: " + unRole.Name, null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }

            return View(unRole);
        }



        public ActionResult Edit(string id = null)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Role manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            IdentityRole unRole = this.roleService.GetRoleById(id);
            if (unRole == null)
            {
                return HttpNotFound();
            }
            return View(unRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,")] IdentityRole unRole)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.roleService.UpdateRole(unRole);
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(unRole);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Inventaire, null, "Modification du Role: " + unRole.Name, null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(unRole);
        }


        public ActionResult Delete(string id = null)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Role manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            IdentityRole unRole = this.roleService.GetRoleById(id);
            if (unRole == null)
            {
                return HttpNotFound();
            }
            return View(unRole);
        }

        // POST: Inventaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.roleService.DeleteRole(id);
            return RedirectToAction("Index");
        }




    }
}