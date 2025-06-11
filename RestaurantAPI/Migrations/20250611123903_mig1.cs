using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    AboutID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutCompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AboutImage1 = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AboutImage2 = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AboutDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutWhyChoose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AboutReportImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AboutRezervationImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.AboutID);
                });

            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogUserIPAdress = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LogUserInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LogUserGeo = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    LogDate = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogs", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Chefs",
                columns: table => new
                {
                    ChefID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChefNameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ChefDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChefTwitterLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefFacebookLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefInstagramLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefLinkedinLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChefImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ChefStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chefs", x => x.ChefID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInfos",
                columns: table => new
                {
                    CompanyInfoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyInfoAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyInfoPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyInfoMail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoOpenClosed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoGithubLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoWebSiteLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoInstagramLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoLinkedinLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyInfoIFrame = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfos", x => x.CompanyInfoID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EventDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EventStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    FeatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FeatureSubTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FeatureDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FeatureVideoUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FeatureImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.FeatureID);
                });

            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageByte = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryImages", x => x.ImageID);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageNameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessageEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessagePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MessageSubject = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MessageDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageSendDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    MessageIsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationNameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReservationEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReservationPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ReservationCountOfPeople = table.Column<int>(type: "int", nullable: false),
                    ReservationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServiceDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ServiceIconUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    TestimonialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestimonialNameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TestimonialTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TestimonialComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TestimonialImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TestimonialStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.TestimonialID);
                });

            migrationBuilder.CreateTable(
                name: "WebLogs",
                columns: table => new
                {
                    WebLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebLogUserIPAdress = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    WebLogUserInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    WebLogUserGeo = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    WebLogDate = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebLogs", x => x.WebLogID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProductStatus = table.Column<bool>(type: "bit", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "Chefs");

            migrationBuilder.DropTable(
                name: "CompanyInfos");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "GalleryImages");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "WebLogs");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
