namespace Libanon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BorrowerTemps", "ConfirmBorrower", c => c.Boolean(nullable: false));
            DropColumn("dbo.Books", "ConfirmBorrower");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "ConfirmBorrower", c => c.Boolean(nullable: false));
            DropColumn("dbo.BorrowerTemps", "ConfirmBorrower");
        }
    }
}
