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
using OTSSite.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OTSSite.Repositories;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;
using OTSSite.ExtensionMethod;
using OTSSite.Configurations;

namespace OTSSite
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationIdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<IRepository<Article>, ArticleRepository>();
            services.AddTransient<IRepository<Comment>, CommentRepository>();
            services.AddScoped(typeof(ArticleFileRepository));

            services.AddOptions();
            services.Configure<FileWriteOptions>(Configuration.GetSection("FileWriteOptions"));

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Article, Models.ViewModels.ArticleViewModel>()
                    .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<Entities.Comment, Models.ViewModels.CommentViewModel>()
                    .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<Models.CreateCommentDto, Entities.Comment>()
                    .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now));
                cfg.CreateMap<Models.CreateArticleDto, Entities.Article>()
                    .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now));
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
