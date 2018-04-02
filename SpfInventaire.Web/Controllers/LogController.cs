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
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity;
using SpfInventaire.Core.DAL.ViewModels;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Web.Controllers
{
    [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
    public class LogController : Controller
    {
        private ILoggerService logService;

        public LogController(ILoggerService logService)
        {
            this.logService = logService;
        }

        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.logService.GetLogs());
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult IndexSuppression()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Log manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Log unLog = this.logService.GetLogById(id);
            if (unLog == null)
            {
                return HttpNotFound();
            }
            return View(unLog);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            this.logService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(LogSuppressionViewModel model)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.logService.DeleteLogsAnterieurDate(model.DateSuppression);
            }else
            {
                return View("IndexSuppression", model);
            }
            return Redirect("Index");
        }


    }
}
