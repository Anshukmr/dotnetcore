using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MentalVerse.Web.Api.BusinessServices;
using MentalVerse.Web.Api.DAL;
using MentalVerse.Web.Api.DAL.Repository;
using MentalVerse.Web.Api.Helper;
using MentalVerse.Web.Api.Interfaces;
using MentalVerse.Web.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace MentalVerse.Web.Api
{
    public class Startup
    {
        
        // public Startup(IConfiguration configuration)
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static string AuthoDomain { get; private set; }
        public static string Auth0ApiIdentifier { get; private set; }
        public static string Client_id { get; private set; }
        public static string Client_secret { get; private set; }
        public static string CompanyDBConnectionString {get;private set;}
        public static string CompanyStorageAccount { get; private set; }
        public static string CompanyContainerName { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2); //Need to check core 2.2 

            //1. DI
            services.AddDbContext<CompanyDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("AzureSQLCompanyDB")));//TO DO :not working
            services.AddScoped<IDocumentRepository<Document>, DocumentRepository>();
            services.AddScoped<IUserRepository<User>, UserRepository>();
            services.AddScoped<ICompanyRepository<Company>, CompanyRepository>();
            services.AddScoped<IDocumentsService, DocumentsService>();

            //2. AuthO - Authentication Middleware.
            string domain = $"https://{Configuration["Auth0Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0ApiIdentifier"];

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Grab the raw value of the token, and store it as a claim so we can retrieve it again later in the request pipeline
                        // Have a look at the ValuesController.UserInformation() method to see how to retrieve it and use it to retrieve the
                        // user's information from the /userinfo endpoint
                        if (context.SecurityToken is JwtSecurityToken token)
                        {
                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", token.RawData));
                            }
                        }

                        return Task.FromResult(0);
                    }
                };
            });

            //3. Swagger
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Document Management API",
                    Description = "Mental Fish Backend Web APIs",
                    TermsOfService = "None"

                });
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("Bearer",
                new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();  
            app.UseAuthentication(); // TODO should be before UseMVC.

            app.UseMvc();
      
            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MentalVerse Web API V1");
            });

            AuthoDomain = Configuration["Auth0Domain"];
            Auth0ApiIdentifier = Configuration["Auth0ApiIdentifier"];
            Client_id = Configuration["Client_id"];
            Client_secret = Configuration["Client_secret"];
            //Connection strings
            CompanyDBConnectionString = Configuration.GetConnectionString("AzureSQLCompanyDB");
            CompanyStorageAccount = Configuration.GetConnectionString("AzureStorageAccount");
            CompanyContainerName = Configuration.GetConnectionString("BlobContainerName");

        }
    }
}
