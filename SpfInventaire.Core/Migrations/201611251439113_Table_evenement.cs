namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_evenement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Evenement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Description = c.String(maxLength: 100),
                        Duree = c.Int(nullable: false),
                        DateEvenement = c.DateTime(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        DateModification = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Evenement");
        }
    }
}
