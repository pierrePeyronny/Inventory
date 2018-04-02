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

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class ValidationInventaireController : Controller
    {
        private IValidationInventaireService validationService;
        private IInventaireService inventaireService;
        private ILoggerService logService;

        public ValidationInventaireController(IValidationInventaireService validationService, IInventaireService inventaireService, ILoggerService logService)
        {
            this.validationService = validationService;
            this.inventaireService = inventaireService;
            this.logService = logService;
        }

        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.validationService.GetValidations());
        }


        [HttpPost]
        public ActionResult AjaxCreate(int inventaireId)
        {
            ActionControllerResult result = ActionControllerResult.FAILURE;
            Inventaire unInventaire = this.inventaireService.GetInventaireById(inventaireId);
            
            if(unInventaire != null)
            {
                ValidationInventaire uneValidation = new ValidationInventaire();
                uneValidation.Inventaire = unInventaire;

                result = this.validationService.InsertValidation(uneValidation, User.Identity.GetUserId());
            }

            return Json(result);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Validation manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            ValidationInventaire uneValidation = this.validationService.GetValidationById(id);
            if (uneValidation == null)
            {
                return HttpNotFound();
            }
            return View(uneValidation);
        }

        [Authorize(Roles = Constantes.ROLE_ADMINISTRATEUR)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.validationService.DeleteValidation(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.ValidationInventaire, id, "Suppression d'une validation d'inventaire", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

    }
}
