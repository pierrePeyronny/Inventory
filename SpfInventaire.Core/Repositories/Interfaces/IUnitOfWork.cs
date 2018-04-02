using SpfInventaire.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        InventaireContext Dbcontext { get; }

        void Save();
    }
}
