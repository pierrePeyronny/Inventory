namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlocInventaire",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 70),
                        Rank = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                        InventaireID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Inventaire", t => t.InventaireID, cascadeDelete: true)
                .Index(t => t.InventaireID);
            
            CreateTable(
                "dbo.Inventaire",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 70),
                        Rank = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Materiel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 70),
                        Description = c.String(),
                        Quantite = c.Int(nullable: false),
                        Tester = c.Boolean(nullable: false),
                        Rank = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                        BlocInventaireID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BlocInventaire", t => t.BlocInventaireID, cascadeDelete: true)
                .Index(t => t.BlocInventaireID);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeEvt = c.Int(nullable: false),
                        TypeObject = c.Int(),
                        IdObject = c.Int(),
                        Description = c.String(),
                        Exception = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                        Utilisateur_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Utilisateur_Id)
                .Index(t => t.Utilisateur_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Surnom = c.String(),
                        PremiereConnexion = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TicketIncident",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaterielID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Statut = c.Int(nullable: false),
                        Message = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                        BlocInventaire_ID = c.Int(),
                        Inventaire_ID = c.Int(),
                        UtilisateurAdministrateur_Id = c.String(maxLength: 128),
                        UtilisateurCreateur_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BlocInventaire", t => t.BlocInventaire_ID)
                .ForeignKey("dbo.Inventaire", t => t.Inventaire_ID)
                .ForeignKey("dbo.Materiel", t => t.MaterielID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UtilisateurAdministrateur_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UtilisateurCreateur_Id)
                .Index(t => t.MaterielID)
                .Index(t => t.BlocInventaire_ID)
                .Index(t => t.Inventaire_ID)
                .Index(t => t.UtilisateurAdministrateur_Id)
                .Index(t => t.UtilisateurCreateur_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketIncident", "UtilisateurCreateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketIncident", "UtilisateurAdministrateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketIncident", "MaterielID", "dbo.Materiel");
            DropForeignKey("dbo.TicketIncident", "Inventaire_ID", "dbo.Inventaire");
            DropForeignKey("dbo.TicketIncident", "BlocInventaire_ID", "dbo.BlocInventaire");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Log", "Utilisateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Materiel", "BlocInventaireID", "dbo.BlocInventaire");
            DropForeignKey("dbo.BlocInventaire", "InventaireID", "dbo.Inventaire");
            DropIndex("dbo.TicketIncident", new[] { "UtilisateurCreateur_Id" });
            DropIndex("dbo.TicketIncident", new[] { "UtilisateurAdministrateur_Id" });
            DropIndex("dbo.TicketIncident", new[] { "Inventaire_ID" });
            DropIndex("dbo.TicketIncident", new[] { "BlocInventaire_ID" });
            DropIndex("dbo.TicketIncident", new[] { "MaterielID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Log", new[] { "Utilisateur_Id" });
            DropIndex("dbo.Materiel", new[] { "BlocInventaireID" });
            DropIndex("dbo.BlocInventaire", new[] { "InventaireID" });
            DropTable("dbo.TicketIncident");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Log");
            DropTable("dbo.Materiel");
            DropTable("dbo.Inventaire");
            DropTable("dbo.BlocInventaire");
        }
    }
}
