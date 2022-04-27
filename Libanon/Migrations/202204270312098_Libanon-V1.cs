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
                        ConfirmOwner = c.Boolean(nullable: false),
                        CurrentISBNId = c.Int(nullable: false),
                        CurrentOwnerId = c.Int(nullable: false),
                        CurrentBorrowerId = c.Int(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Users", t => t.CurrentBorrowerId)
                .ForeignKey("dbo.ISBN", t => t.CurrentISBNId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CurrentOwnerId, cascadeDelete: true)
                .Index(t => t.CurrentISBNId)
                .Index(t => t.CurrentOwnerId)
                .Index(t => t.CurrentBorrowerId);
            
            CreateTable(
                "dbo.Borrowers",
                c => new
                    {
                        BorrowerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        ConfirmBorrower = c.Boolean(nullable: false),
                        CurrentBookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BorrowerId)
                .ForeignKey("dbo.Books", t => t.CurrentBookId)
                .Index(t => t.CurrentBookId);
            
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
                "dbo.ISBN",
                c => new
                    {
                        ISBNId = c.Int(nullable: false, identity: true),
                        ISBNCode = c.String(),
                    })
                .PrimaryKey(t => t.ISBNId);
            
            CreateTable(
                "dbo.Rate",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        RateValue = c.Int(nullable: false),
                        CurrentISBNId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RateId)
                .ForeignKey("dbo.ISBN", t => t.CurrentISBNId, cascadeDelete: true)
                .Index(t => t.CurrentISBNId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CurrentOwnerId", "dbo.Users");
            DropForeignKey("dbo.Books", "CurrentISBNId", "dbo.ISBN");
            DropForeignKey("dbo.Rate", "CurrentISBNId", "dbo.ISBN");
            DropForeignKey("dbo.Books", "CurrentBorrowerId", "dbo.Users");
            DropForeignKey("dbo.Borrowers", "CurrentBookId", "dbo.Books");
            DropIndex("dbo.Rate", new[] { "CurrentISBNId" });
            DropIndex("dbo.Borrowers", new[] { "CurrentBookId" });
            DropIndex("dbo.Books", new[] { "CurrentBorrowerId" });
            DropIndex("dbo.Books", new[] { "CurrentOwnerId" });
            DropIndex("dbo.Books", new[] { "CurrentISBNId" });
            DropTable("dbo.Rate");
            DropTable("dbo.ISBN");
            DropTable("dbo.Users");
            DropTable("dbo.Borrowers");
            DropTable("dbo.Books");
        }
    }
}
