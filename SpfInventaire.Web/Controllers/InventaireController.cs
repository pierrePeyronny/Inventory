using System;
using System.Web.Mvc;
using SpfInventaire.Core.DAL.Models;
using SpfInventaire.Core.BLL.Interfaces;
using Microsoft.AspNet.Identity;
using static SpfInventaire.Core.BLL.Constantes;
using SpfInventaire.Core.BLL;
using SpfInventaire.Core.DAL.ViewModels;
using System.Collections.Generic;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class InventaireController : Controller
    {
        private IInventaireService inventaireService;
        private IMaterielService materielService;
        private IEvenementService evenementService;
        private ILoggerService logService;
        
        public InventaireController(IInventaireService inventaireService, IMaterielService materielService, IEvenementService evenementService, ILoggerService logService)
        {
            this.inventaireService = inventaireService;
            this.materielService = materielService;
            this.evenementService = evenementService;
            this.logService = logService;
        }

        
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }
            return View(this.inventaireService.GetInventaires());
        }


        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult DetailStockInventaire(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsByInventaire(Convert.ToInt32(id));
            if (listMateriel == null)
            {
                return HttpNotFound();
            }
            return View(listMateriel);
        }
        
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Inventaire unInventaire = this.inventaireService.GetInventaireById(id);
            if (unInventaire == null)
            {
                return HttpNotFound();
            }
            return View(unInventaire);
        }

        
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Create()
        {
            return View();
        }

        
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventaire unInventaire)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.inventaireService.InsertInventaire(unInventaire, User.Identity.GetUserId());
                if(result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(unInventaire);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Create, LOG_TYPE_OBJECT.Inventaire, null, "Création d'un inventaire", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            
            return View(unInventaire);
        }

        
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Inventaire unInventaire = this.inventaireService.GetInventaireById(id);
            if (unInventaire == null)
            {
                return HttpNotFound();
            }
            return View(unInventaire);
        }

        // POST: Inventaire/Edit/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nom,Rank,Active,IsInventaireStock,DateCreation,DateModification")] Inventaire unInventaire)
        {
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.inventaireService.UpdateInventaire(unInventaire, User.Identity.GetUserId());
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(unInventaire);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Inventaire, unInventaire.ID, "Modification d'un inventaire", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }            
            return View(unInventaire);
        }

        // GET: Inventaire/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Inventaire manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            Inventaire unInventaire = this.inventaireService.GetInventaireById(id);
            if (unInventaire == null)
            {
                return HttpNotFound();
            }
            return View(unInventaire);
        }

        // POST: Inventaire/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.inventaireService.DeleteInventaire(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.Inventaire, id, "Suppression d'un Inventaire", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult IndexSaisieInventaire()
        {
            SaisieInventaireViewModels formModel = new SaisieInventaireViewModels();

            formModel.listInventaires = new SelectList(this.inventaireService.GetActiveNotStockInventaires(), "ID", "Nom");
            formModel.desinfection = this.evenementService.IsDesinfectionToday(DateTime.Now.Date);
            return View(formModel);
        }

        [Authorize]
        public ActionResult GetFormSaisieInventaire(int id)
        {
            return PartialView("_FormSaisieInventaire", this.inventaireService.GetInventaireById(id));
        }


    }
}
