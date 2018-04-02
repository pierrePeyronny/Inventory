namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Engin_PleinEssence : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Engin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        CodeConf = c.String(nullable: false),
                        CodeChauff = c.String(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PleinEssence",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Litrage = c.Single(nullable: false),
                        Prix = c.Single(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        EnginID = c.Int(nullable: false),
                        UtilisateurCreateur_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Engin", t => t.EnginID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UtilisateurCreateur_Id)
                .Index(t => t.EnginID)
                .Index(t => t.UtilisateurCreateur_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PleinEssence", "UtilisateurCreateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PleinEssence", "EnginID", "dbo.Engin");
            DropIndex("dbo.PleinEssence", new[] { "UtilisateurCreateur_Id" });
            DropIndex("dbo.PleinEssence", new[] { "EnginID" });
            DropTable("dbo.PleinEssence");
            DropTable("dbo.Engin");
        }
    }
}
