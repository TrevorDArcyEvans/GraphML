using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GraphML.UI.Web
{
  internal static class WebHostBuilder
	{
		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
    		.UseStartup<Startup>()
			  .UseKestrel()
			  .Build();
		}
	}
}
