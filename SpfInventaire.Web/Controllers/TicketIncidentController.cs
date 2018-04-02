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
using SpfInventaire.Core.BLL;
using Microsoft.AspNet.Identity;
using SpfInventaire.Core.DAL.ViewModels;
using SpfInventaire.Web.Helpers.Interfaces;

namespace SpfInventaire.Web.Controllers
{
    [Authorize]
    public class TicketIncidentController : Controller
    {
        private ITicketIncidentService ticketService;
        private IInventaireService inventaireService;
        private IBlocInventaireService blocInventaireService;
        private IMaterielService materielService;
        private ILoggerService logService;
        private ISelectListHelper selectListHelper;


        public TicketIncidentController(ITicketIncidentService ticketService, IInventaireService inventaireService, IBlocInventaireService blocInventaireService, IMaterielService materielService, ILoggerService logService, ISelectListHelper selectListHelper)
        {
            this.ticketService = ticketService;
            this.inventaireService = inventaireService;
            this.blocInventaireService = blocInventaireService;
            this.materielService = materielService;
            this.logService = logService;
            this.selectListHelper = selectListHelper;
        }

        
        public ActionResult Index(string MessageErreur = null)
        {
            if (!String.IsNullOrEmpty(MessageErreur))
            {
                ViewBag.ErrorMessage = MessageErreur;
            }

            ViewBag.isAdminTicket = User.IsInRole(Constantes.ROLE_ADMIN_TICKET);

            return View(this.ticketService.GetTickets());
        }


        [Authorize(Roles = Constantes.ROLE_ADMIN_TICKET)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Ticket manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            TicketIncident unTicket = this.ticketService.GetTicketById(id);
            if (unTicket == null)
            {
                return HttpNotFound();
            }

            return View(unTicket);
        }

        [HttpPost]
        public ActionResult GetFormDetail(int materielId)
        {
            Materiel unMateriel = this.materielService.GetMaterielById(materielId);
            TicketIncident unTicket = unMateriel.Tickets.Where(w => w.Statut != TICKET_INCIDENT_STATUT.Resolu).OrderByDescending(f => f.DateCreation).FirstOrDefault();

            return PartialView("_FormDetailTicket", unTicket);
        }

        // GET: TicketIncident/Create
        public ActionResult Create()
        {
            FormTicketIncidentViewModels formModel = GetFormTicket(false);
            return View("Create", formModel);
        }

        [HttpPost]
        public ActionResult GetFormCreate(int? materielID = null)
        {
            FormTicketIncidentViewModels formModel = GetFormTicket(false, materielId: materielID);
            return PartialView("_AjaxAddTicket", formModel);
        }

        // POST: TicketIncident/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaterielID, Type, statut, Message, NumeroFeb")]TicketIncident unTicket)
        {
            FormTicketIncidentViewModels formModel = this.GetFormTicket(false, unTicket);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.ticketService.InsertTicket(unTicket, User.Identity.GetUserId(), User.IsInRole(ROLE_ADMIN_TICKET));
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                return RedirectToAction("Index");
            }

            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate([Bind(Include = "MaterielID, Type, statut, Message, NumeroFeb")]TicketIncident unTicket)
        {
            FormTicketIncidentViewModels formModel = this.GetFormTicket(false, unTicket);
            if (ModelState.IsValid)
            {
                ActionControllerResult result = this.ticketService.InsertTicket(unTicket, User.Identity.GetUserId(), User.IsInRole(ROLE_ADMIN_TICKET));
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return PartialView("_FormContenuTicket", formModel);
                }
                return Json(string.Empty);
            }

            return PartialView("_FormContenuTicket", formModel);
        }




        // GET: TicketIncident/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string ErrorMessage = "Id Ticket manquant";
                return RedirectToAction("Index", new { MessageErreur = ErrorMessage });
            }
            TicketIncident unTicket = this.ticketService.GetTicketById(id);
            if (unTicket == null)
            {
                return HttpNotFound();
            }

            FormTicketIncidentViewModels formModel = this.GetFormTicket(true, unTicket);
            return View(formModel);
        }

        // POST: TicketIncident/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, MaterielID, Type, statut, Message, NumeroFeb, DateCreation, DateModification")] TicketIncident unTicket)
        {
            FormTicketIncidentViewModels formModel = this.GetFormTicket(true, unTicket);
            if (ModelState.IsValid)
            {

                ActionControllerResult result = this.ticketService.UpdateTicket(unTicket, User.IsInRole(Constantes.ROLE_ADMIN_TICKET), User.Identity.GetUserId());
                
                if (result == ActionControllerResult.FAILURE)
                {
                    ViewBag.ErrorMessage = Constantes.MESSAGE_ERR_NOTIFICATIONS;
                    return View(formModel);
                }
                //Réussi
                this.logService.LogEvenement(LOG_TYPE_EVENT.Edit, LOG_TYPE_OBJECT.Ticket, unTicket.ID, "Modification d'un Ticket", null, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }

            return View(formModel);
        }

        // POST: TicketIncident/Delete/5
        [Authorize(Roles = Constantes.ROLE_ADMIN_TICKET)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.ticketService.DeleteTicket(id);
            this.logService.LogEvenement(LOG_TYPE_EVENT.Delete, LOG_TYPE_OBJECT.Ticket, id, "Suppression d'un Ticket", null, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }


        private FormTicketIncidentViewModels GetFormTicket(bool isEdit, TicketIncident unTicket = null, int ? materielId = null)
        {

            //Gestion du statu du ticket
            if (unTicket != null)
            {
                if(unTicket.Statut == 0)
                {
                    unTicket.Statut = TICKET_INCIDENT_STATUT.Nouveau;
                }
            }
            else
            {
                unTicket = new TicketIncident();
                unTicket.Statut = TICKET_INCIDENT_STATUT.Nouveau;
            }

            //Gestion de la récupératio des ID
            Materiel unMateriel = null;
            int? IdInventaire = null;
            int? IdBloc = null;
            int? IdMateriel = null;

            if (unTicket.MaterielID > 0)
            {
                unMateriel = this.materielService.GetMaterielById(unTicket.MaterielID);
                IdMateriel = unTicket.MaterielID;
            }
            else if(materielId != null)
            {
                unMateriel = this.materielService.GetMaterielById(materielId);
                IdMateriel = materielId;
            }

            if (IdMateriel != null)
            {
                IdInventaire = unMateriel.BlocInventaire.InventaireID;
                IdBloc = unMateriel.BlocInventaireID;
                unTicket.MaterielID = Convert.ToInt32(IdMateriel);
            }
           

            //Création Form
            FormTicketIncidentViewModels formModel = new FormTicketIncidentViewModels();
            formModel.unTicket = unTicket;

            //Gestion des listes
            formModel.listInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.inventaireService.GetActiveNotStockInventaires(), "ID", "Nom"), IdInventaire, "Sélectionner");

            if(IdInventaire != null)
            {
                formModel.listBlocInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(this.blocInventaireService.GetBlocsInventaireByInventaire(Convert.ToInt32(IdInventaire)), "ID", "Nom"), IdBloc, "Sélectionner");
            }
            else
            {
                formModel.listBlocInventaire = this.selectListHelper.AddFirstItemSelectList(new SelectList(Enumerable.Empty<SelectListItem>(), "ID", "Nom"), IdBloc, "Aucun");
            }

            if (IdInventaire != null)
            {
                IEnumerable<Materiel> listMateriel = this.materielService.GetMaterielsByBlocInventaire(Convert.ToInt32(IdBloc));
                Dictionary<string, string> dictionaryItem = new Dictionary<string, string>();

                foreach (Materiel unMat in listMateriel)
                {
                    if (unMat.TypeMateriel != null)
                    {
                        dictionaryItem.Add(unMat.ID.ToString(), unMat.TypeMateriel.Nom);
                    }
                    else
                    {
                        dictionaryItem.Add(unMat.ID.ToString(), unMat.Nom);
                    }
                }
                formModel.listMateriel = this.selectListHelper.AddFirstItemSelectList(this.selectListHelper.CreateSelectList(dictionaryItem), IdMateriel, "Sélectionner");
            }
            else
            {
                formModel.listMateriel = this.selectListHelper.AddFirstItemSelectList(new SelectList(Enumerable.Empty<SelectListItem>(), "ID", "Nom"), IdMateriel, "Aucun");
            }


            //Gestion du mode Edit et du mode Ajax
            formModel.isEdit = isEdit;
            formModel.isAdminTicket = User.IsInRole(Constantes.ROLE_ADMIN_TICKET);
            return formModel;
        }

    }
}
