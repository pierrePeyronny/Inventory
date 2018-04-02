namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validationInventaire : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ValidationInventaire",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InventaireID = c.Int(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        Utilisateur_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Inventaire", t => t.InventaireID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Utilisateur_Id, cascadeDelete: true)
                .Index(t => t.InventaireID)
                .Index(t => t.Utilisateur_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ValidationInventaire", "Utilisateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ValidationInventaire", "InventaireID", "dbo.Inventaire");
            DropIndex("dbo.ValidationInventaire", new[] { "Utilisateur_Id" });
            DropIndex("dbo.ValidationInventaire", new[] { "InventaireID" });
            DropTable("dbo.ValidationInventaire");
        }
    }
}
