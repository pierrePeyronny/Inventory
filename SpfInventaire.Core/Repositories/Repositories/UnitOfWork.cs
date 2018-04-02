using SpfInventaire.Core.DAL;
using SpfInventaire.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private InventaireContext context;

        public InventaireContext Dbcontext
        {
            get
            {
                if (this.context == null)
                {
                    this.context = new InventaireContext();
                }
                return this.context;
            }
        }

        public void Save()
        { 
            this.context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
