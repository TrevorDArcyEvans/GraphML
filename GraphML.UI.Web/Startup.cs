using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
using GraphML.UI.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace GraphML.UI.Web
{
  public class Startup
  {
    private static readonly object _lock = new object();

    private IServiceProvider ServiceProvider { get; set; }
    public IConfiguration Configuration { get; }
    private IWebHostEnvironment CurrentEnvironment { get; }
    private IContainer ApplicationContainer { get; set; }

    public Startup(IWebHostEnvironment env)
    {
      // Environment variable:
      //    ASPNETCORE_ENVIRONMENT == Development
      CurrentEnvironment = env;

      AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;

      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddJsonFile($"autofac.json", optional: true)
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>();

      Configuration = builder.Build();

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

      services.AddSignalR();
      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddSingleton<WeatherForecastService>();

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

      // load configuration from autofac.json
      var module = new ConfigurationModule(Configuration);
      builder.RegisterModule(module);

      ApplicationContainer = builder.Build();

      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(ApplicationContainer);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      ServiceProvider = app.ApplicationServices;

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }

    private Assembly OnAssemblyResolve(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
    {
      lock (_lock)
      {
        AssemblyLoadContext.Default.Resolving -= OnAssemblyResolve;
        try
        {
          var currAssyPath = Assembly.GetExecutingAssembly().Location;
          var assyPath = Path.Combine(Path.GetDirectoryName(currAssyPath), $"{assemblyName.Name}.dll");
          var assembly = File.Exists(assyPath) ? Assembly.LoadFile(assyPath) : null;
          return assembly;
        }
        finally
        {
          AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;
        }
      }
    }
  }
}
