﻿using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
	public class Startup
	{
		private IWebHostEnvironment CurrentEnvironment { get; }
		private IConfiguration Configuration { get; }

		public Startup(
		IWebHostEnvironment env,
		IConfiguration conf)
		{
			// Environment variable:
			//    ASPNETCORE_ENVIRONMENT == Development
			CurrentEnvironment = env;
			Configuration = conf;

			Settings.DumpSettings(Configuration);
		}
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddIdentityServer(options =>
				{
					options.Events.RaiseErrorEvents = true;
					options.Events.RaiseInformationEvents = true;
					options.Events.RaiseFailureEvents = true;
					options.Events.RaiseSuccessEvents = true;
				})
				.AddTestUsers(TestUsers.Users)
				.AddInMemoryIdentityResources(Config.IdentityResources)
				.AddInMemoryApiScopes(Config.ApiScopes)
				.AddInMemoryApiResources(Config.ApiResources)
				.AddInMemoryClients(Config.Clients)
				.AddDeveloperSigningCredential();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseDeveloperExceptionPage();

			app.UseStaticFiles();
			app.UseRouting();

			app.UseIdentityServer();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
		}
	}
}