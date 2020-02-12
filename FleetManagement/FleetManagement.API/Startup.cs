using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Entities;
using FleetManagement.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VMS
{
    public class Startup
    {
        public static DocumentDBRepository<Customer> _context;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _context = new DocumentDBRepository<Customer>(Configuration.GetValue<string>(
                "CosmosDB:AccountEndpoint"), Configuration.GetValue<string>(
                "CosmosDB:AccountKey"), Configuration.GetValue<string>(
                "CosmosDB:Database"), Configuration.GetValue<string>(
                "CosmosDB:Collection"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);                   

            // DependacyInjection
            services.AddSingleton<IDocumentDBRepository<Customer>>(_context);
            // for filling initial data
            FillInitialData();

            // Register the Swagger services
            services.AddSwaggerDocument();
        }

        private void FillInitialData()
        {
            var list = _context.GetItemsAsync(y => y.Name != "").Result.ToList();
            if (list.Count>0)
            {
                return;
            }
            var x = _context.CreateItemAsync(new Customer() { 
                Name = "Kalles Grustransporter AB",                
                Address = new Address() { City= "Södertälje", Details= "Cementvägen 8, 111 11" }, 
                Vehicles = new Vehicle[] { new Vehicle()
                {
                    VehicleId = "YS2R4X20005399401",
                    RegistrationNumber = "ABC123"
                },new Vehicle()
                {
                    VehicleId = "VLUR4X20009093588",
                    RegistrationNumber = "DEF456"
                },new Vehicle()
                {
                    VehicleId = "VLUR4X20009048066",
                    RegistrationNumber = "GHI789"
                }
                } });

            x = _context.CreateItemAsync(new Customer()
            {
                Name = "Johans Bulk AB",                
                Address = new Address() { City = "Stockholm", Details = "Balkvägen 12, 222 22" },
                Vehicles = new Vehicle[] { new Vehicle()
                {
                    VehicleId = "YS2R4X20005388011",
                    RegistrationNumber = "JKL012"
                },new Vehicle()
                {
                    VehicleId = "YS2R4X20005387949",
                    RegistrationNumber = "MNO345"
                }
                }
            });

            x = _context.CreateItemAsync(new Customer()
            {
                Name = "Haralds Värdetransporter AB",                
                Address = new Address() { City = "Uppsala", Details = "Budgetvägen 1, 333 33" },
                Vehicles = new Vehicle[] { new Vehicle()
                {
                    VehicleId = "YS2R4X20005387765",
                    RegistrationNumber = "PQR678"
                },new Vehicle()
                {
                    VehicleId = "YS2R4X20005387055",
                    RegistrationNumber = "STU901"
                }
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStaticFiles();

            // for swagger
            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
