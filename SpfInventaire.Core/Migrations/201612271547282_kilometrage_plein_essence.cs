namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kilometrage_plein_essence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PleinEssence", "Kilometrage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PleinEssence", "Kilometrage");
        }
    }
}
