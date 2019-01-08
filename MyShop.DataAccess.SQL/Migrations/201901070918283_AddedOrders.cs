namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderId = c.String(maxLength: 128),
                        ProductId = c.String(),
                        ProductName = c.String(),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        Quantity = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Street = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        PostCode = c.String(),
                        OrderStatus = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Customers", "Province", c => c.String());
            AddColumn("dbo.Customers", "PostCode", c => c.String());
            DropColumn("dbo.Customers", "State");
            DropColumn("dbo.Customers", "ZipCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "ZipCode", c => c.String());
            AddColumn("dbo.Customers", "State", c => c.String());
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropColumn("dbo.Customers", "PostCode");
            DropColumn("dbo.Customers", "Province");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
