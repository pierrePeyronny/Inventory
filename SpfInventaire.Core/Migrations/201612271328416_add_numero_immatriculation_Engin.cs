namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_numero_immatriculation_Engin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Engin", "Numero", c => c.String(maxLength: 10));
            AddColumn("dbo.Engin", "Immatriculation", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Engin", "Immatriculation");
            DropColumn("dbo.Engin", "Numero");
        }
    }
}
