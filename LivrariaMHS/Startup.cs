using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LivrariaMHS.Models;
using LivrariaMHS.Models.Service;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using LivrariaMHS.Data;
using System.Threading.Tasks;

namespace LivrariaMHS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=LivrariaMHS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<LivrariaMHSContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ConfigDataBase>();

            services.AddScoped<ClienteServico>();
            services.AddScoped<CidadeServico>();
            services.AddScoped<BairroServico>();
            services.AddScoped<RuaServico>();
            services.AddScoped<LivroServico>();
            services.AddScoped<AutorServico>();
            services.AddScoped<CategoriaServico>();
            services.AddScoped<LivroCategoriaServico>();
            services.AddScoped<VendaServico>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ConfigDataBase configDataBase)
        {
            var brasil = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(brasil),
                SupportedCultures = new List<CultureInfo> { brasil },
                SupportedUICultures = new List<CultureInfo> { brasil }
            };
            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                configDataBase.DataBaseInitializer();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Livros}/{action=Index}/{id?}");
            });
        }
    }
}
