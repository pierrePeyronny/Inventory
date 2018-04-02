namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajout_TypeMateriel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeMateriel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 70),
                        Domaine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Materiel", "TypeMaterielID", c => c.Int());
            CreateIndex("dbo.Materiel", "TypeMaterielID");
            AddForeignKey("dbo.Materiel", "TypeMaterielID", "dbo.TypeMateriel", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Materiel", "TypeMaterielID", "dbo.TypeMateriel");
            DropIndex("dbo.Materiel", new[] { "TypeMaterielID" });
            DropColumn("dbo.Materiel", "TypeMaterielID");
            DropTable("dbo.TypeMateriel");
        }
    }
}
