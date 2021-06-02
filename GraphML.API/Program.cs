using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GraphML.API
{
  public sealed class Program
  {
    public static async Task Main(string[] args)
    {
      try
      {
        await WebHostBuilder.BuildWebHost(args).Build().RunAsync();
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }
  }
}
