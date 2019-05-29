using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTSSiteMVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OTSSiteMVC.Entities;
using OTSSiteMVC.Configurations;
using OTSSiteMVC.Repositories;
using System.IO;
using Microsoft.Extensions.FileProviders;
using UsefulExtensionMethods.IdentityExtensions;

namespace OTSSiteMVC
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
            // Nonessential cookie consent.
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //DB Connection
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Add Configureation Options
            services.AddOptions();
            services.Configure<FileWriteOptions>(Configuration.GetSection("FileWriteOptions"));
            services.Configure<AboutInfoOptions>(Configuration.GetSection("AboutInfoOptions"));
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            //Add identity services with roles.
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //Transients, Scopes, and Singletons
            services.AddTransient(typeof(SiteFileRepository));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                //DTO's to Entities
                cfg.CreateMap<Models.CreateUserDto, Entities.AppIdentityUser>()
                    .ForMember(dest => dest.JoinDateTime, opt => opt.MapFrom(src => DateTime.Now));
                cfg.CreateMap<Models.CreateArticleDto, Entities.Article>();
                cfg.CreateMap<Models.UploadImageDto, Entities.ImageData>()
                    .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
                    .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
                    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ImageMapHelper(src.File)));                    
                //Entities to DTO's
                cfg.CreateMap<Entities.AppIdentityUser, Models.GetUserProfileDto>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<Entities.AppIdentityUser, Models.UserConfigDto>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<Entities.Article, Models.GetArticleDto>()
                    .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<Entities.Article, Models.ArticleInfoDto>()
                    .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.UserName));
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "Images")),
                RequestPath = new PathString("/Images")
            });
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{action}");
            });
        }

        private byte[] ImageMapHelper(IFormFile file)
        {
            byte[] streamcont;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                streamcont = ms.ToArray();
            }
            return streamcont;
        }
    }
}
