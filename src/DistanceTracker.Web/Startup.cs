using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Authentication.Cookies;
using DistanceTracker.Data.UnitOfWork;
using DistanceTracker.Data.EntityFramework;
using Microsoft.Data.Entity;
using System.IO;

namespace DistanceTracker
{
    public class Startup
    {
        private readonly IApplicationEnvironment _appEnv;
        public IConfiguration Configuration { get; set; }

        public Startup(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;

            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();

            services
                .AddEntityFramework()
                .AddSqlite();

            services.AddScoped<IUnitOfWork, UnitOfWork>(x =>
            {
                var options = new DbContextOptionsBuilder();
                options.UseSqlite(ConnectionString);

                var context = new DataContext(options.Options);
                context.Database.EnsureCreated();

                return new UnitOfWork(context);
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIISPlatformHandler();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.AutomaticAuthenticate = true;
                options.CookieName = Constants.Cookies.AuthenticationToken;
                options.ExpireTimeSpan = DateTime.Now.AddDays(Constants.AuthExpireDays) - DateTime.Now;
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        #region Helper functions

        private string GetConfigurationValue(string key)
        {
            if (string.IsNullOrEmpty(Configuration[key]))
                throw new ArgumentNullException(string.Format("Configuration value for key '{0}' not found.", key));

            return Configuration[key];
        }

        private string ConnectionString
        {
            get
            {
                var filePath = GetConfigurationValue(Constants.Settings.Keys.SQLiteFilePath);

                // Convert relative path to absolute if needed
                if (!Path.IsPathRooted(filePath))
                    filePath = Path.Combine(_appEnv.ApplicationBasePath, filePath);

                return string.Format("Data Source={0}", filePath);
            }
        }

        #endregion
    }
}
