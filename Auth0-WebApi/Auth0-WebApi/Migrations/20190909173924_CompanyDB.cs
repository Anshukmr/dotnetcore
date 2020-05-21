using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth0.WebApi.Migrations
{
    public partial class CompanyDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Auth0Domain = table.Column<string>(nullable: true),
                    Auth0ApiIdentifier = table.Column<string>(nullable: true),
                    Auth0ClientId = table.Column<string>(nullable: true),
                    Auth0ClientSecret = table.Column<string>(nullable: true),
                    AzureSQLCompanyDB = table.Column<string>(nullable: true),
                    AzureStorageAccount = table.Column<string>(nullable: true),
                    BlobContainer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocType = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    URI = table.Column<string>(nullable: true),
                    SASToken = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Published = table.Column<int>(nullable: true),
                    Shared = table.Column<int>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    CustomPropertyX = table.Column<string>(nullable: true),
                    IsAnimating = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Auth0ApiIdentifier", "Auth0ClientId", "Auth0ClientSecret", "Auth0Domain", "AzureSQLCompanyDB", "AzureStorageAccount", "BlobContainer", "Name" },
                values: new object[] { 100, "https://mentalfish.eu.auth0.com/api/v2/", "VPrWFdD1eyJZZJfJXR6JUdlaJKO5Od5b", "iGGr983NbzVpqWDcYWHt3GUzZxJkil9vZKGM46MVDaukmdz4ooW5IDEpG7xs9AtW", "mentalfish.eu.auth0.com", "Server=tcp:mentalversedbserver.database.windows.net,1433;Initial Catalog=MentalFishCompanyDB;Persist Security Info=False;User ID=Admin17;Password=Azure@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", "DefaultEndpointsProtocol=https;AccountName=mentalversestorage;AccountKey=j30iXmeuQJZnysPfS3RuZNXzt18rrBNjSWzrmxZck8ASfR+2X/JEsAuTjivY3cSBNvMaveIf3MPnQMbj60b9WQ==;EndpointSuffix=core.windows.net", "company-100", "MentalFish.no" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[,]
                {
                    { 1, "Editor", "RW" },
                    { 2, "Viewer", "R" },
                    { 3, "Admin", "RWD" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
