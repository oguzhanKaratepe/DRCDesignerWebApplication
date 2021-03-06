﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using DRCDesignerWebApplication.Helpers;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.Concrete;
using Microsoft.Extensions.FileProviders;

namespace DRCDesignerWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            services.AddScoped<ISubdomainUnitOfWork, SubdomainUnitOfWork>();
            services.AddScoped<IDrcUnitOfWork, DrcUnitOfWork>();
            services.AddScoped<IRoleUnitOfWork, RoleUnitOfWork>();
            services.AddScoped<IDocumentTransferUnitOfWork,DocumentTransferUnitOfWork>();
            services.AddScoped<DrcCardContext, DrcCardContext>();
            services.AddScoped<ISubdomainService, SubdomainManager>();
            services.AddScoped<ISubdomainVersionService, SubdomainVersionManager>();
            services.AddScoped<IRoleService, RoleManager>();
            services.AddScoped<IDrcCardService, DrcCardManager>();
            services.AddScoped<IFieldService, FieldManager>();
            services.AddScoped<IResponsibilityService, ResponsibilityManager>();
            services.AddScoped<IAuthorizationService, AuthorizationManager>();
            services.AddScoped<IDrcCardMoveService, DrcCardMoveManager>();
            services.AddScoped<IExportService, ExportManager>();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // Auto Mapper Configurations
           
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BusinessAutoMapperProfiles());
                mc.AddProfile(new AutoMapperProfiles());
            });

            IMapper Mapper = mappingConfig.CreateMapper();
          
           

            services.AddSingleton(Mapper);
        

            services.AddMvc();
            //DrcDesigner
            //StudentManagement
          
            //var connection = @"server=(localdb)\MSSQLLocalDB; Database=DrcDesigner; Trusted_Connection=true";
            //var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DrcDesigner;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var connection = Configuration.GetConnectionString("DRCDesigner");
            services.AddSession();

            services.AddDbContext<DrcCardContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("DRCDesignerWebApplication")));
           // services.AddDbContext<DrcCardContext>(context => { context.UseInMemoryDatabase("OguzDatabase"); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DrcCardContext>())
                {
                    context.Database.Migrate();
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseMvc(configureRoutes);
          
        }
        private void configureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
