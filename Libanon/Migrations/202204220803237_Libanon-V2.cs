namespace Libanon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonV2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Books", new[] { "CurrentBorrowerId" });
            AlterColumn("dbo.Books", "CurrentBorrowerId", c => c.Int());
            CreateIndex("dbo.Books", "CurrentBorrowerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Books", new[] { "CurrentBorrowerId" });
            AlterColumn("dbo.Books", "CurrentBorrowerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "CurrentBorrowerId");
        }
    }
}
