using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalConsultant.DAL;
using AnimalConsultant.DAL.Models;
using AnimalConsultant.DAL.Models.Identity;
using AnimalConsultant.Generic;
using AnimalConsultant.Services;
using AutoMapper;
using DemOffice.Email;
using DemOffice.Email.EmailService;
using DemOffice.GenericCrud;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AnimalConsultant
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



            string connection = Configuration.GetConnectionString("DefaultConnection");

            var smtpOptions = Configuration
                .GetSection("EmailOptions")
                .Get<SmtpOptions>();

            AnimalConsultantDbProvider.ConnectionString = connection;

            services.AddDbContext<AnimalConsultantDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddAuthentication();

            services.AddMvc()
                .AddRazorPagesOptions(c => c.RootDirectory = "/Views")
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddGenericCrud<AnimalConsultantDbProvider>(opt =>
                {
                    opt.Setup = GenericEntitiesSetup.Mappings;
                });

            services.AddEmailService(EmailServiceProviders.Smtp, smtpOptions);

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AnimalConsultantDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {

                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                            ctx.Response.Redirect("/login");

                        return Task.CompletedTask;
                    }
                };

            });

            services.AddTransient<IUserService, UserService>();

            services.AddAutoMapper();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseAuthentication();
         app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
