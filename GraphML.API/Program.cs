using Microsoft.AspNetCore.Hosting;

namespace GraphML.API
{
  public sealed class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        WebHostBuilder.BuildWebHost(args).Run();
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }
  }
}
