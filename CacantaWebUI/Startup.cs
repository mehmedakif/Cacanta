using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Cacanta.WebUI.Repository.Concrete.EntityFramework;
using Cacanta.WebUI.Repository.Abstract;
using Microsoft.Extensions.Hosting;
using Cacanta.WebUI.IdentityCore;
using Microsoft.AspNetCore.Identity;

namespace CacantaWebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<CacantaContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Myconnection")));

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EfProductRepository>();
            services.AddTransient<ICategoryRepository, EfCategoryRepository>();
            services.AddTransient<IOrderRepository, EfOrderRepository>();
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();
            services.AddDistributedMemoryCache();
            services.AddMvc(options => options.EnableEndpointRouting = false)  
            .AddSessionStateTempDataProvider();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider.GetService<CacantaContext>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "products",
                     template: "products/{category?}",
                     defaults: new { controller = "Product", action = "List" });

                routes.MapRoute(
                       name: "default",
                       template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
