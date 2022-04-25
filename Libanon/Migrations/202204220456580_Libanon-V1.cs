namespace Libanon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        ImageUrl = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        CurrentISBNId = c.Int(nullable: false),
                        CurrentOwnerId = c.Int(nullable: false),
                        CurrentBorrowerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Users", t => t.CurrentBorrowerId, cascadeDelete: false)
                .ForeignKey("dbo.ISBNs", t => t.CurrentISBNId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CurrentOwnerId, cascadeDelete: true)
                .Index(t => t.CurrentISBNId)
                .Index(t => t.CurrentOwnerId)
                .Index(t => t.CurrentBorrowerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ISBNs",
                c => new
                    {
                        ISBNId = c.Int(nullable: false, identity: true),
                        ISBNCode = c.String(),
                    })
                .PrimaryKey(t => t.ISBNId);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        RateValue = c.Int(nullable: false),
                        CurrentISBNId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RateId)
                .ForeignKey("dbo.ISBNs", t => t.CurrentISBNId, cascadeDelete: true)
                .Index(t => t.CurrentISBNId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CurrentOwnerId", "dbo.Users");
            DropForeignKey("dbo.Books", "CurrentISBNId", "dbo.ISBNs");
            DropForeignKey("dbo.Rates", "CurrentISBNId", "dbo.ISBNs");
            DropForeignKey("dbo.Books", "CurrentBorrowerId", "dbo.Users");
            DropIndex("dbo.Rates", new[] { "CurrentISBNId" });
            DropIndex("dbo.Books", new[] { "CurrentBorrowerId" });
            DropIndex("dbo.Books", new[] { "CurrentOwnerId" });
            DropIndex("dbo.Books", new[] { "CurrentISBNId" });
            DropTable("dbo.Rates");
            DropTable("dbo.ISBNs");
            DropTable("dbo.Users");
            DropTable("dbo.Books");
        }
    }
}
