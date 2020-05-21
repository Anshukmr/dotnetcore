using MentalVerse.Web.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.DAL
{
    public partial class CompanyDbContext : DbContext
    {
        public CompanyDbContext() { }
        public CompanyDbContext(DbContextOptions<DbContext> options)
            : base(options) { }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:mentalversedbserver.database.windows.net,1433;Initial Catalog=MentalFishCompanyDB;Persist Security Info=False;User ID=Admin17;Password=Azure@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Editor", Permissions = "RW" },
                new Role { Id = 2, Name = "Viewer", Permissions = "R" },
                new Role { Id = 3, Name = "Admin", Permissions = "RWD" }
            );

            //Add Default company.
            modelBuilder.Entity<Company>().HasData(
                new Company {
                    Id = 100,
                    Name ="MentalFish.no",
                    Auth0Domain = "mentalfish.eu.auth0.com",
                    Auth0ApiIdentifier = "https://mentalfish.eu.auth0.com/api/v2/",
                    Auth0ClientId = "VPrWFdD1eyJZZJfJXR6JUdlaJKO5Od5b",
                    Auth0ClientSecret = "iGGr983NbzVpqWDcYWHt3GUzZxJkil9vZKGM46MVDaukmdz4ooW5IDEpG7xs9AtW",
                    AzureSQLCompanyDB= "Server=tcp:mentalversedbserver.database.windows.net,1433;Initial Catalog=MentalFishCompanyDB;Persist Security Info=False;User ID=Admin17;Password=Azure@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
                    AzureStorageAccount= "DefaultEndpointsProtocol=https;AccountName=mentalversestorage;AccountKey=j30iXmeuQJZnysPfS3RuZNXzt18rrBNjSWzrmxZck8ASfR+2X/JEsAuTjivY3cSBNvMaveIf3MPnQMbj60b9WQ==;EndpointSuffix=core.windows.net",
                    BlobContainer = "company-100"
                }
            );
        }
    }
}

