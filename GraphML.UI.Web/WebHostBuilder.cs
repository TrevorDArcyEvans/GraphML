using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphML.UI.Web
{
	internal static class WebHostBuilder
	{
		public static IHost BuildWebHost(string[] args)
		{
			//var config = new ConfigurationBuilder()
			//  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			//  .AddJsonFile("hosting.json")
			//  .AddJsonFile($"autofac.json", optional: true)
			//  .AddEnvironmentVariables()
			//  .AddUserSecrets<Program>()
			//  .Build();

			return Host.CreateDefaultBuilder(args)
    		  .ConfigureWebHostDefaults(webBuilder =>
					  {
						  webBuilder.UseStartup<Startup>();
					  })
			  //.UseKestrel()
			  //.ConfigureServices(services => services.AddAutofac())
			  //.UseConfiguration(config)
			  //.ConfigureLogging(logging =>
			  //{
				 // logging.SetMinimumLevel(LogLevel.Trace);
			  //})
			  //.UseNLog()  // NLog: setup NLog for Dependency injection
			  .Build();
		}
	}
}
