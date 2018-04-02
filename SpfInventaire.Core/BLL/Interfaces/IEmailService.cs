using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using static SpfInventaire.Core.BLL.Constantes;

namespace SpfInventaire.Core.BLL.Interfaces
{
    public interface IEmailService : IIdentityMessageService
    {
        Task SendAsync(IdentityMessage message);
        void SendEmailCreationTicket(string email);
    }
}