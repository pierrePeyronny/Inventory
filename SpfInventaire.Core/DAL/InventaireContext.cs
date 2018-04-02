using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpfInventaire.Core.DAL
{
    public class InventaireContext : ApplicationDbContext
    {
        public InventaireContext()
        {

        }

        public DbSet<Inventaire> Inventaires { get; set; }
        public DbSet<BlocInventaire> BlocInventaires { get; set; }
        public DbSet<Materiel> Materiels { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<TicketIncident> TicketIncidents { get; set; }
        public DbSet<Evenement> Evenements { get; set; }

        public DbSet<ValidationInventaire> ValidationInventaires { get; set; }
        public DbSet<TypeMateriel> TypeMateriels { get; set; }
        public DbSet<StockMateriel> StockMateriels { get; set; }
        public DbSet<SortieStockMateriel> SortieStockMateriels { get; set; }
        public DbSet<Engin> Engins { get; set; }
        public DbSet<PleinEssence> PleinEssences { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Database.SetInitializer<InventaireContext>(new InventaireInitializer());
        }

    }
}
