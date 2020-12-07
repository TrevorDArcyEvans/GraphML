using System;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
	public static class Settings
	{
		public static string KESTREL_URL(this IConfiguration config) =>
			Environment.GetEnvironmentVariable("KESTREL_URL") ??
			config["Kestrel:EndPoints:Http:Url"] ??
			"http://localhost:5000";

		public static void DumpSettings(IConfiguration config)
		{
			Console.WriteLine("Settings:");
			Console.WriteLine($"  KESTREL:");
			Console.WriteLine($"    KESTREL_URL       : {config.KESTREL_URL()}");
		}
	}
}