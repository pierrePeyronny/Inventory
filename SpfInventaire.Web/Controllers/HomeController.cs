using Microsoft.AspNet.Identity;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private IUserService userService;
        private ILoggerService logService;

        public HomeController(IUserService userService, ILoggerService logService)
        {
            this.userService = userService;
            this.logService = logService;
        }

        public ActionResult Index()
        {

            ApplicationUser unUser = this.userService.GetUserById(User.Identity.GetUserId());
            if(unUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //Si ce n'est pas la première connexion alors on renvoie sur le site, sinon on renvoi sur la page de première connexion
            if (!unUser.PremiereConnexion)
            {
                this.logService.LogEvenement(LOG_TYPE_EVENT.Connexion, LOG_TYPE_OBJECT.User, null, "Connexion de " + unUser.UserName, null, unUser.Id);
                return RedirectToAction("IndexSaisieInventaire", "Inventaire");
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        public ActionResult FirstConnexion()
        {
            ApplicationUser unUser = this.userService.GetUserById(User.Identity.GetUserId());
            if (unUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            unUser.PremiereConnexion = false;
            this.userService.UpdateUser(unUser, User.Identity.GetUserId());

            this.logService.LogEvenement(LOG_TYPE_EVENT.PremiereConnexion, LOG_TYPE_OBJECT.User, null, "Première Connexion de l'utilisateur " + unUser.UserName, null, unUser.Id);

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}