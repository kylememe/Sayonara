using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sayonara
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
			// Add Entity Framework services to the services container.
			/*
			services.AddEntityFramework()
				.AddSqlServer()
				.AddDbContext<SayonaraContext>(options =>
				{
					options.UseSqlServer(Configuration.Get<string>("Data:ConnectionString"));
				});
			*/

			// Add MVC services to the services container.
			services.AddMvc();		
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();			

			loggerFactory.MinimumLevel = LogLevel.Information;
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			// Configure the HTTP request pipeline.

			// Add the following to the request pipeline only in development environment.
			if (env.IsDevelopment())
			{			
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// sends the request to the following path or controller action.
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseIISPlatformHandler();

			// Add static files to the request pipeline.		
			app.UseStaticFiles();			

			// Add MVC to the request pipeline.
			app.UseMvc(routes =>
			{
				routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");				
			});

			//SampleData.Initialize(app.ApplicationServices);
		}

		public IConfiguration Configuration { get; set; }

		// Entry point for the application.
		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
  }
}
