namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gestion_Stock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockMateriel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantite = c.Short(nullable: false),
                        DatePeremption = c.DateTime(),
                        Supprime = c.Boolean(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                        MaterielID = c.Int(nullable: false),
                        TypeMaterielID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Materiel", t => t.MaterielID, cascadeDelete: true)
                .ForeignKey("dbo.TypeMateriel", t => t.TypeMaterielID, cascadeDelete: true)
                .Index(t => t.MaterielID)
                .Index(t => t.TypeMaterielID);
            
            CreateTable(
                "dbo.SortieStockMateriel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateSortie = c.DateTime(nullable: false),
                        Raison = c.Int(nullable: false),
                        Quantite = c.Int(nullable: false),
                        Utilisateur_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Utilisateur_Id)
                .Index(t => t.Utilisateur_Id);
            
            CreateTable(
                "dbo.SortieStockMaterielStockMateriel",
                c => new
                    {
                        SortieStockMateriel_ID = c.Int(nullable: false),
                        StockMateriel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SortieStockMateriel_ID, t.StockMateriel_ID })
                .ForeignKey("dbo.SortieStockMateriel", t => t.SortieStockMateriel_ID, cascadeDelete: true)
                .ForeignKey("dbo.StockMateriel", t => t.StockMateriel_ID, cascadeDelete: true)
                .Index(t => t.SortieStockMateriel_ID)
                .Index(t => t.StockMateriel_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMateriel", "TypeMaterielID", "dbo.TypeMateriel");
            DropForeignKey("dbo.SortieStockMateriel", "Utilisateur_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SortieStockMaterielStockMateriel", "StockMateriel_ID", "dbo.StockMateriel");
            DropForeignKey("dbo.SortieStockMaterielStockMateriel", "SortieStockMateriel_ID", "dbo.SortieStockMateriel");
            DropForeignKey("dbo.StockMateriel", "MaterielID", "dbo.Materiel");
            DropIndex("dbo.SortieStockMaterielStockMateriel", new[] { "StockMateriel_ID" });
            DropIndex("dbo.SortieStockMaterielStockMateriel", new[] { "SortieStockMateriel_ID" });
            DropIndex("dbo.SortieStockMateriel", new[] { "Utilisateur_Id" });
            DropIndex("dbo.StockMateriel", new[] { "TypeMaterielID" });
            DropIndex("dbo.StockMateriel", new[] { "MaterielID" });
            DropTable("dbo.SortieStockMaterielStockMateriel");
            DropTable("dbo.SortieStockMateriel");
            DropTable("dbo.StockMateriel");
        }
    }
}
