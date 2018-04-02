namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajout_Type_Sortie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SortieStockMateriel", "TypeSortie", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SortieStockMateriel", "TypeSortie");
        }
    }
}
