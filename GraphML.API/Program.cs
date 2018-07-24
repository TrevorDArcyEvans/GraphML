using Microsoft.AspNetCore.Hosting;

namespace GraphML.API
{
#pragma warning disable CS1591
  public sealed class Program
  {
    public static void Main(string[] args)
    {
      WebHostBuilder.BuildWebHost(args).Run();
    }
  }
#pragma warning restore CS1591
}
