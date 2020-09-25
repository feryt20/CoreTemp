using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTemp.Data.Migrations.DbMain
{
    public partial class CoreTemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderExpireDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    DiscCode = table.Column<string>(nullable: true),
                    DiscVar = table.Column<long>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    PostalCost = table.Column<long>(nullable: false),
                    FinalPrice = table.Column<long>(nullable: false),
                    IsFinalized = table.Column<bool>(nullable: false),
                    TrackingCode = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSend = table.Column<bool>(nullable: false),
                    IsDelivered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    ProductGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    ProductGroupTitle = table.Column<string>(maxLength: 100, nullable: false),
                    ProductGroupTitleEnglish = table.Column<string>(maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 150, nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Row = table.Column<bool>(nullable: false),
                    SubGroup = table.Column<bool>(nullable: false),
                    MinSubGroup = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.ProductGroupId);
                    table.ForeignKey(
                        name: "FK_ProductGroups_ProductGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentLogs",
                columns: table => new
                {
                    PaymentLogId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: true),
                    TrackingCode = table.Column<string>(maxLength: 150, nullable: true),
                    PaymentResponseCode = table.Column<string>(maxLength: 150, nullable: true),
                    PaymentResponseMessage = table.Column<string>(maxLength: 150, nullable: true),
                    IsSuccessful = table.Column<bool>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLogs", x => x.PaymentLogId);
                    table.ForeignKey(
                        name: "FK_PaymentLogs_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentUniqueNumbers",
                columns: table => new
                {
                    PaymentUniqueId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentUniqueNumbers", x => x.PaymentUniqueId);
                    table.ForeignKey(
                        name: "FK_PaymentUniqueNumbers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId2 = table.Column<string>(maxLength: 20, nullable: true),
                    ProductGroupId = table.Column<int>(nullable: false),
                    BrandGroupId = table.Column<int>(nullable: false),
                    ProductStatus = table.Column<bool>(nullable: false),
                    ProductUrl = table.Column<string>(maxLength: 400, nullable: false),
                    ProductTitle = table.Column<string>(maxLength: 400, nullable: false),
                    ProductShortTitle = table.Column<string>(maxLength: 400, nullable: false),
                    ProductEnglishTitle = table.Column<string>(maxLength: 400, nullable: false),
                    ProductSummery = table.Column<string>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: false),
                    ProductPrice = table.Column<long>(nullable: false),
                    ProductPoint = table.Column<int>(nullable: false),
                    HaveDiscount = table.Column<bool>(nullable: false),
                    ProductDiscount = table.Column<long>(nullable: false),
                    DiscountTime = table.Column<DateTime>(nullable: false),
                    CommentIsActive = table.Column<bool>(nullable: false),
                    ProductQtty = table.Column<int>(nullable: false),
                    KeyWords = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 150, nullable: true),
                    IsFile = table.Column<bool>(nullable: false),
                    FileUrl = table.Column<string>(maxLength: 150, nullable: true),
                    FilePassword = table.Column<string>(nullable: true),
                    OtherImageUrl = table.Column<string>(maxLength: 150, nullable: true),
                    ProductThumbnailImageUrl = table.Column<string>(maxLength: 150, nullable: true),
                    ProductThumbnailImageUrl2 = table.Column<string>(maxLength: 150, nullable: true),
                    ProductThumbnailImageUrl3 = table.Column<string>(maxLength: 150, nullable: true),
                    ProductThumbnailImageUrl4 = table.Column<string>(maxLength: 150, nullable: true),
                    ProductThumbnailImageUrl5 = table.Column<string>(maxLength: 150, nullable: true),
                    ProductViews = table.Column<int>(nullable: false),
                    ProductComments = table.Column<int>(nullable: false),
                    ProductLike = table.Column<int>(nullable: false),
                    ProductSells = table.Column<int>(nullable: false),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false),
                    ProductBuy = table.Column<bool>(nullable: false),
                    ProductVip = table.Column<bool>(nullable: false),
                    ProductVip2 = table.Column<bool>(nullable: false),
                    VipDate = table.Column<DateTime>(nullable: false),
                    IsDiscount = table.Column<bool>(nullable: false),
                    OtherUrl = table.Column<string>(maxLength: 250, nullable: true),
                    IsOtherUrl = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: false),
                    OrderedCount = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Discount = table.Column<long>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    ProducerId = table.Column<string>(nullable: true),
                    TrackingCode = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_OrderId",
                table: "PaymentLogs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUniqueNumbers_OrderId",
                table: "PaymentUniqueNumbers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ParentId",
                table: "ProductGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupId",
                table: "Products",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductId2",
                table: "Products",
                column: "ProductId2",
                unique: true,
                filter: "[ProductId2] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PaymentLogs");

            migrationBuilder.DropTable(
                name: "PaymentUniqueNumbers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductGroups");
        }
    }
}
