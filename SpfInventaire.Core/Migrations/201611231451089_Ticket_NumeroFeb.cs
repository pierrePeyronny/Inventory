namespace SpfInventaire.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ticket_NumeroFeb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketIncident", "NumeroFeb", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketIncident", "NumeroFeb");
        }
    }
}
