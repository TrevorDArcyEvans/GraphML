using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GraphML.Datastore.Database.Importer.CSV
{
  public sealed class Program
  {
    public static void Main(string[] args)
    {
      // check import spec file
      if (args.Length == 1 && !File.Exists(args[0]))
      {
        Usage();
        return;
      }

      var importSpecPath = args.Length == 0 ? "import.json" : args[0];
      var config = new ConfigurationBuilder()
        .AddJsonFile(importSpecPath)
        .Build();
      var importSpec = new ImportSpecification();
      config.GetSection("ImportSpecification").Bind(importSpec);
      using var stream = File.OpenRead(importSpec.DataFile);
      new Importer(importSpec, config, stream).Run();
    }

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  GraphML.Datastore.Database.Importer.CSV.exe <import.json>");
      Console.WriteLine();
      Console.WriteLine("Notes:");
      Console.WriteLine("  Database connection is contained in import.json");
    }
  }
}
