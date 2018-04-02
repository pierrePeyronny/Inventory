namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_sortieStock_and_Stock_liaison : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SortieStockMaterielStockMateriel", "SortieStockMateriel_ID", "dbo.SortieStockMateriel");
            DropForeignKey("dbo.SortieStockMaterielStockMateriel", "StockMateriel_ID", "dbo.StockMateriel");
            DropIndex("dbo.SortieStockMaterielStockMateriel", new[] { "SortieStockMateriel_ID" });
            DropIndex("dbo.SortieStockMaterielStockMateriel", new[] { "StockMateriel_ID" });
            AddColumn("dbo.SortieStockMateriel", "UsedStockMaterielID", c => c.Int(nullable: false));
            AddColumn("dbo.SortieStockMateriel", "IdSourceStock", c => c.Int(nullable: false));
            CreateIndex("dbo.SortieStockMateriel", "UsedStockMaterielID");
            AddForeignKey("dbo.SortieStockMateriel", "UsedStockMaterielID", "dbo.StockMateriel", "ID", cascadeDelete: true);
            DropTable("dbo.SortieStockMaterielStockMateriel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SortieStockMaterielStockMateriel",
                c => new
                    {
                        SortieStockMateriel_ID = c.Int(nullable: false),
                        StockMateriel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SortieStockMateriel_ID, t.StockMateriel_ID });
            
            DropForeignKey("dbo.SortieStockMateriel", "UsedStockMaterielID", "dbo.StockMateriel");
            DropIndex("dbo.SortieStockMateriel", new[] { "UsedStockMaterielID" });
            DropColumn("dbo.SortieStockMateriel", "IdSourceStock");
            DropColumn("dbo.SortieStockMateriel", "UsedStockMaterielID");
            CreateIndex("dbo.SortieStockMaterielStockMateriel", "StockMateriel_ID");
            CreateIndex("dbo.SortieStockMaterielStockMateriel", "SortieStockMateriel_ID");
            AddForeignKey("dbo.SortieStockMaterielStockMateriel", "StockMateriel_ID", "dbo.StockMateriel", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SortieStockMaterielStockMateriel", "SortieStockMateriel_ID", "dbo.SortieStockMateriel", "ID", cascadeDelete: true);
        }
    }
}
