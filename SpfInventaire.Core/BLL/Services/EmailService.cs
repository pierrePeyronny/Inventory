using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using SpfInventaire.Core.BLL.Interfaces;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Services
{
    public class EmailService : IEmailService
    {
        private ILoggerService logService;

        public EmailService()
        {
        }

        public EmailService(ILoggerService logService)
        {
            this.logService = logService;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await SendEmailAsyncViaSendGrid(message.Destination, message.Subject, message.Body);
        }


        private async Task SendEmailAsyncViaSendGrid(string to, string subject, string body)
        {
            SendGridAPIClient sendGridClient;
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["KeySendGrid"];
            sendGridClient = new SendGridAPIClient(apiKey);

            Mail mail = ConfigMail(to, subject, body);
            await sendGridClient.client.mail.send.post(requestBody: mail.Get());
        }


        private ActionControllerResult SendEmailViaSendGrid(string to, string subject, string body)
        {
            ActionControllerResult result;
            try
            {
                SendGridAPIClient sendGridClient;
                string apiKey = System.Configuration.ConfigurationManager.AppSettings["KeySendGrid"];
                sendGridClient = new SendGridAPIClient(apiKey);

                Mail mail = ConfigMail(to, subject, body);
                sendGridClient.client.mail.send.post(requestBody: mail.Get());

                result = ActionControllerResult.SUCCESS;
            }
            catch (Exception ex)
            {
                this.logService.LogErreur(LOG_TYPE_OBJECT.Email, null, "Erreur lors de l'envoi d'un mail", ex.Message, null);
                result = ActionControllerResult.FAILURE;
            }
            return result;
        }

        private Mail ConfigMail(string to, string subject, string body)
        {
            Email fromEmail = new Email(System.Configuration.ConfigurationManager.AppSettings["EmailFrom"]);
            Email toEmail = new Email(to);
            Content content = new Content("text/plain", body);
            return new Mail(fromEmail, subject, toEmail, content);
        }


        public void SendEmailCreationTicket(string email)
        {
            string subject = "SP2F Inventaire - Création d'un ticket";
            string body = "Un ticket d'incident vient d'être créé.";

            this.SendEmailViaSendGrid(email, subject, body);
        }


    }
}
