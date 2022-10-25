using AutoMapper;
using FileUploader.Domain.Context;
using FileUploader.Domain.Repositories;
using FileUploader.Domain.Repositories.Interfaces;
using FileUploader.Services.Helpers.Mapper;
using FileUploader.Services.Services;
using FileUploader.Services.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FileUploader.MVC
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
            services.AddDbContext<FileUploaderDbContext>
                (options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                        ServiceLifetime.Transient
                );
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FileMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddAutoMapper(typeof(FileMapper));
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IFilesRepository, FilesRepository>();
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=File}/{action=Index}/{id?}");
            });
        }
    }
}
