using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using NLog;

namespace GraphML.UI.Web
{
	public class Startup
	{
		private IWebHostEnvironment CurrentEnvironment { get; }
		private IConfiguration Configuration { get; }

		public Startup(
		IWebHostEnvironment env,
		IConfiguration configuration)
		{
			// Environment variable:
			//    ASPNETCORE_ENVIRONMENT == Development
			CurrentEnvironment = env;

			Configuration = configuration;

			// database connection string for nLog
			GlobalDiagnosticsContext.Set("LOG_CONNECTION_STRING", Settings.LOG_CONNECTION_STRING(Configuration));

			Settings.DumpSettings(Configuration);
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(sp => Configuration);
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddRazorPages().AddNewtonsoftJson(options =>
		  options.SerializerSettings.Converters.Add(new StringEnumConverter()));
			services.AddServerSideBlazor();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
			})
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
				options =>
				{
					options.Authority = "https://localhost:44387/";
					options.ClientId = "BlazorID_App";
					options.ClientSecret = "secret";
					options.UsePkce = true;
					options.ResponseType = "code";
					options.Scope.Add("openid");
					options.Scope.Add("profile");
					options.Scope.Add("email");
					options.Scope.Add("offline_access");

					// Scope for accessing API
					options.Scope.Add("identityApi"); //invalid scope for client

					// Scope for custom user claim
					options.Scope.Add("appuser_claim"); //invalid scope for client

					//options.CallbackPath = ...
					options.SaveTokens = true;
					options.GetClaimsFromUserInfoEndpoint = true;
				});

      // Create the container builder.
			var builder = new ContainerBuilder();

			// Register dependencies, populate the services from
			// the collection, and build the container.
			//
			// Note that Populate is basically a foreach to add things
			// into Autofac that are in the collection. If you register
			// things in Autofac BEFORE Populate then the stuff in the
			// ServiceCollection can override those things; if you register
			// AFTER Populate those registrations can override things
			// in the ServiceCollection. Mix and match as needed.
			builder.Populate(services);

			// load all assemblies in same directory and register classes with interfaces
			// Note that we have to explicitly add this (executing) assembly
			var exeAssy = Assembly.GetExecutingAssembly();
			var exeAssyPath = exeAssy.Location;
			var exeAssyDir = Path.GetDirectoryName(exeAssyPath);
			var assyPaths = Directory.EnumerateFiles(exeAssyDir, "GraphML.*.dll");

			var assys = assyPaths.Select(filePath => Assembly.LoadFrom(filePath)).ToList();
			assys.Add(exeAssy);
			builder
			  .RegisterAssemblyTypes(assys.ToArray())
			  .PublicOnly()
			  .AsImplementedInterfaces()
			  .SingleInstance();

			builder.Register(cc => Configuration).As<IConfiguration>();

			var applicationContainer = builder.Build();

			// Create the IServiceProvider based on the container.
			return new AutofacServiceProvider(applicationContainer);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			if (CurrentEnvironment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
				  {
					  endpoints.MapBlazorHub();
					  endpoints.MapFallbackToPage("/_Host");
				  });
		}
	}
}
