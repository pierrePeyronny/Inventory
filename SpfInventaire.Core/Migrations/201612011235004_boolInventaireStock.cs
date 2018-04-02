namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boolInventaireStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventaire", "IsInventaireStock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventaire", "IsInventaireStock");
        }
    }
}
