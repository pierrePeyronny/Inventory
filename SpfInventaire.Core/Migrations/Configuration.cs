namespace SpfInventaire.Core.Migrations
{
    using BLL;
    using DAL.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SpfInventaire.Core.DAL.InventaireContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SpfInventaire.Core.DAL.InventaireContext";
        }

        protected override void Seed(SpfInventaire.Core.DAL.InventaireContext context)
        {
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("chorasoumam");

            var applicationsUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id="242f160f-1809-4448-9d9f-ca89574a2dea", Nom="Peyronny", Prenom="Pierre", Surnom="Popeye", Email="peyronnypierre@gmail.com", EmailConfirmed=true, UserName="20150", PremiereConnexion=true, PasswordHash= password, SecurityStamp= Guid.NewGuid().ToString()}
            };
            applicationsUsers.ForEach(i => context.Users.AddOrUpdate(n => n.Id, i));

            //Role
            var roleAdmin = new IdentityRole { Id = "7ffec384-0248-4c36-9989-09bf5104e344", Name = Constantes.ROLE_ADMINISTRATEUR };
            var dbSetRoles = context.Set<IdentityRole>();
            dbSetRoles.AddOrUpdate(n => n.Id, roleAdmin);

            var roleAdminInventaire = new IdentityRole { Id = "d1b09fa3-988d-4fc7-87da-ca58565eecd3", Name = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE };
            dbSetRoles.AddOrUpdate(n => n.Id, roleAdminInventaire);

            //Association Role User
            var CompteAdmin = new IdentityUserRole { RoleId = "7ffec384-0248-4c36-9989-09bf5104e344", UserId = "242f160f-1809-4448-9d9f-ca89574a2dea" };
            var dbSetRoleUser = context.Set<IdentityUserRole>();
            dbSetRoleUser.AddOrUpdate(n => n.UserId, CompteAdmin);


            base.Seed(context);
            context.SaveChanges();
        }
    }
}
