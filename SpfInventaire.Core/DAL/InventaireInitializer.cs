using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpfInventaire.Core.BLL;
using SpfInventaire.Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace SpfInventaire.Core.DAL
{
    public class InventaireInitializer : System.Data.Entity.DropCreateDatabaseAlways<InventaireContext>
    {
        protected override void Seed(InventaireContext context)
        {
            ////Gestion SEED Inventaire
            //var inventaire = new List<Inventaire>
            //{
            //    new Inventaire { ID=1, Nom="FPT", Active= true, Rank= 1, DateCreation= DateTime.Now, DateModification= DateTime.Now },
            //    new Inventaire { ID=2, Nom="VSAV", Active= true, Rank= 2, DateCreation= DateTime.Now, DateModification= DateTime.Now },
            //    new Inventaire { ID=3, Nom="VTUT", Active= true, Rank= 3, DateCreation= DateTime.Now, DateModification= DateTime.Now }
            //};
            //inventaire.ForEach(i => context.Inventaires.Add(i));


            //var blocInventaire = new List<BlocInventaire>
            //{
            //    new BlocInventaire { Nom="Intérieur Cabine", Active= true, InventaireID=1, Rank= 1, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Banquette", Active= true, InventaireID=1, Rank= 2, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Coffre Gauche", Active= true, InventaireID=1, Rank= 3, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Arrière FPT", Active= true, InventaireID=1, Rank= 4, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Dessus FPT", Active= true, InventaireID=1, Rank= 5, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Coffre Droit", Active= true, InventaireID=1, Rank= 6, DateCreation= DateTime.Now, DateModification= DateTime.Now  },

            //    new BlocInventaire { Nom="Cabine de conduite", Active= true, InventaireID=2, Rank= 1, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Capucine", Active= true, InventaireID=2, Rank= 2, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Intérieur Cellule", Active= true, InventaireID=2, Rank= 3, DateCreation= DateTime.Now, DateModification= DateTime.Now  },
            //    new BlocInventaire { Nom="Tiroir Bleu", Active= true, InventaireID=2, Rank= 4, DateCreation= DateTime.Now, DateModification= DateTime.Now  }
            //};
            //blocInventaire.ForEach(i => context.BlocInventaires.Add(i));



            //var passwordHash = new PasswordHasher();
            //string password = passwordHash.HashPassword("chorasoumam");

            //var applicationsUsers = new List<ApplicationUser>
            //{
            //    new ApplicationUser { Id="242f160f-1809-4448-9d9f-ca89574a2dea", Nom="Peyronny", Prenom="Pierre", Surnom="Popeye", Email="peyronnypierre@gmail.com", UserName="20150", PremiereConnexion=true, PasswordHash= password, SecurityStamp= Guid.NewGuid().ToString()}
            //};
            //applicationsUsers.ForEach(i => context.Users.AddOrUpdate(n => n.Id, i));

            ////Role
            //var roleAdmin = new IdentityRole { Id = "7ffec384-0248-4c36-9989-09bf5104e344", Name = Constantes.ROLE_ADMINISTRATEUR };
            //var dbSetRoles = context.Set<IdentityRole>();
            //dbSetRoles.AddOrUpdate(n => n.Id, roleAdmin);

            //var roleAdminInventaire = new IdentityRole { Id = "d1b09fa3-988d-4fc7-87da-ca58565eecd3", Name = Constantes.ROLE_ADMIN_GESTION_INVENTAIRE };
            //dbSetRoles.AddOrUpdate(n => n.Id, roleAdminInventaire);

            ////Association Role User
            //var CompteAdmin = new IdentityUserRole { RoleId = "7ffec384-0248-4c36-9989-09bf5104e344", UserId = "242f160f-1809-4448-9d9f-ca89574a2dea" };
            //var dbSetRoleUser = context.Set<IdentityUserRole>();
            //dbSetRoleUser.AddOrUpdate(n => n.UserId, CompteAdmin);


            //base.Seed(context);
            //context.SaveChanges();

        }
    }
}
