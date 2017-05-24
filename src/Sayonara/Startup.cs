using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sayonara.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sayonara.Utilities;

namespace Sayonara
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
			services.AddDbContext<SayonaraContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SayonaraDB")));
			
			services.Configure<SayonaraOptions>(Configuration.GetSection("SayonaraOptions"));

			/*
			// Add Authentication services.
			services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
			*/

			// Add framework services.
			services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SayonaraContext context)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
			loggerFactory.AddAzureWebAppDiagnostics();

			if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

			/*
			// Configure the OWIN pipeline to use cookie auth.
			app.UseCookieAuthentication(new CookieAuthenticationOptions());

			// Configure the OWIN pipeline to use OpenID Connect auth.
			app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
			{
				ClientId = Configuration["AzureAD:ClientId"],
				Authority = String.Format(Configuration["AzureAd:AadInstance"], Configuration["AzureAd:Tenant"]),
				ResponseType = OpenIdConnectResponseType.IdToken,
				PostLogoutRedirectUri = Configuration["AzureAd:PostLogoutRedirectUri"],
				Events = new OpenIdConnectEvents
				{
					OnRemoteFailure = OnAuthenticationFailed,
				}
			});
			*/

			app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

			SayonaraDBInitialize.Initialize(context);
    }

		// Handle sign-in errors differently than generic errors.
		private Task OnAuthenticationFailed(FailureContext context)
		{
			context.HandleResponse();
			context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
			return Task.FromResult(0);
		}
	}
}
