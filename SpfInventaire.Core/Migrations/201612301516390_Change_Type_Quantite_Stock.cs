namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Type_Quantite_Stock : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockMateriel", "Quantite", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockMateriel", "Quantite", c => c.Short(nullable: false));
        }
    }
}
