using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GraphML.UI.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebHostBuilder.BuildWebHost(args).Run();
		}
	}
}
