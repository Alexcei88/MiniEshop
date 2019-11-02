using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MiniEshop.DAL;
using MiniEshop.Domain;
using MiniEshop.Domain.DTO;
using System;
using System.IO;

namespace MiniEshop
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
            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EshopDbContext>(options => options.UseSqlServer(connectionString));
            ApplyMigration(connectionString);

            // поскольку у нас только одна сущность для меппинга, файл профайла опускаю
            services.AddAutoMapper(c => c.CreateMap<GoodDTO, Good>()
                                        .ForMember(g => g.Id, member => member.MapFrom(s => s.Id ?? Guid.Empty))
                                        .ForMember(g => g.Category, member => member.Ignore())
                                        .ReverseMap(),
                                        AppDomain.CurrentDomain.GetAssemblies(), ServiceLifetime.Singleton);

            services.AddScoped<IEshopRepository, EshopRepository>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:59499/");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        /// <summary>
        /// Синхронизация схемы и модели приложения перед запуском
        /// </summary>
        /// <param name="connectionString"></param>
        private void ApplyMigration(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EshopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new EshopDbContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }
        }
    }
}
