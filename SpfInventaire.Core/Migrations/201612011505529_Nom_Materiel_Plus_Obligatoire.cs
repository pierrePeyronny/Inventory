namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nom_Materiel_Plus_Obligatoire : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Materiel", "Nom", c => c.String(maxLength: 70));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Materiel", "Nom", c => c.String(nullable: false, maxLength: 70));
        }
    }
}
