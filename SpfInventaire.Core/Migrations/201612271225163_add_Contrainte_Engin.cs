namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Contrainte_Engin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Engin", "Nom", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Engin", "CodeConf", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Engin", "CodeChauff", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Engin", "CodeChauff", c => c.String(nullable: false));
            AlterColumn("dbo.Engin", "CodeConf", c => c.String(nullable: false));
            AlterColumn("dbo.Engin", "Nom", c => c.String(nullable: false));
        }
    }
}
