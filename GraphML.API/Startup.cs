using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Interfaces.Authentications;
using GraphML.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace GraphML.API
{
  internal sealed class Startup
  {
    private static readonly object _lock = new object();

    private IServiceProvider ServiceProvider { get; set; }
    public IConfiguration Configuration { get; }
    private IHostingEnvironment CurrentEnvironment { get; }
    private IContainer ApplicationContainer { get; set; }

    public Startup(IHostingEnvironment env)
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
        .AddEnvironmentVariables()
        .AddUserSecrets<Program>();

      Configuration = builder.Build();

      // database connection string for nLog
      GlobalDiagnosticsContext.Set("LOG_CONNECTION_STRING", Settings.LOG_CONNECTION_STRING(Configuration));

      DumpSettings();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(sp => Configuration);
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      // Add controllers as services so they'll be resolved.
      services
        .AddMvc()
        .AddControllersAsServices();

      services.Configure<MvcOptions>(options =>
      {
        options.Filters.Add(new RequireHttpsAttribute());
      });

      if (CurrentEnvironment.IsDevelopment())
      {
        // Register the Swagger generator, defining one or more Swagger documents
        services.AddSwaggerGen(options =>
        {
          options.SwaggerDoc("v1",
            new Info
            {
              Title = "GraphML API",
              Version = "v1",
              Description = "GraphML API"
            });
          options.SwaggerDoc("porcelain",
            new Info
            {
              Title = "GraphML API",
              Version = "porcelain",
              Description = "GraphML API"
            });

          options.DocInclusionPredicate((docName, apiDesc) =>
          {
            var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
            {
              return false;
            }

            var versions = controllerActionDescriptor.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);
            var tags = controllerActionDescriptor.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<ApiTagAttribute>();

            return versions.Any(
              v => $"v{v.ToString()}" == docName) ||
              tags.Any(tag => tag.Tag == docName);
          });

          // Set the comments path for the Swagger JSON and UI.
          var xmlPath = Path.Combine(AppContext.BaseDirectory, "GraphML.API.xml");
          options.IncludeXmlComments(xmlPath);
          options.DescribeAllEnumsAsStrings();
          options.OperationFilter<ExamplesOperationFilter>();
        });
      }

      services
        .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
        .AddBasicAuthentication(
            options =>
            {
              options.Realm = "GraphML";
              options.Events = new BasicAuthenticationEvents
              {
                OnValidatePrincipal = context =>
                {
                  var auth = ServiceProvider.GetService<IBasicAuthentication>();
                  return auth.Authenticate(context);
                }
              };
            });

      services
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.Authority = Settings.OIDC_ISSUER_URL(Configuration);
          options.Audience = Settings.OIDC_AUDIENCE(Configuration);
          options.RequireHttpsMetadata = !CurrentEnvironment.IsDevelopment();
          options.Events = new JwtBearerEvents
          {
            OnTokenValidated = async context =>
            {
              var auth = ServiceProvider.GetService<IBearerAuthentication>();
              await auth.Authenticate(context);
            }
          };
        });

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

      // load all assemblies in same directory and register classes with interfaces
      // Note that we have to explicitly add this (executing) assembly
      var exeAssy = Assembly.GetExecutingAssembly();
      var exeAssyPath = exeAssy.Location;
      var exeAssyDir = Path.GetDirectoryName(exeAssyPath);
      var assyPaths = Directory.EnumerateFiles(exeAssyDir, "GraphML.*.dll");

      var assys = assyPaths.Select(filePath => Assembly.LoadFile(filePath)).ToList();
      assys.Add(exeAssy);
      builder
        .RegisterAssemblyTypes(assys.ToArray())
        .PublicOnly()
        .AsImplementedInterfaces()
        .SingleInstance();

      builder.Register(cc => Configuration).As<IConfiguration>();

      ApplicationContainer = builder.Build();

      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(ApplicationContainer);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory logging)
    {
      ServiceProvider = app.ApplicationServices;

      if (CurrentEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      var httpsPort = Settings.KESTREL_HTTPS_PORT(Configuration);
      var options = new RewriteOptions()
                      .AddRedirectToHttps((int)HttpStatusCode.MovedPermanently, httpsPort);

      app.UseRewriter(options);

      if (CurrentEnvironment.IsDevelopment())
      {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(opts =>
        {
          opts.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphML API V1");
          opts.SwaggerEndpoint("/swagger/porcelain/swagger.json", "GraphML API V1 Porcelain");

          opts.DocExpansion(DocExpansion.None);
        });
      }

      app.UseStaticFiles();
      app.UseMvc();

      if (CurrentEnvironment.IsDevelopment())
      {
        var logConfig = Configuration.GetSection("Logging");
        logging.AddConsole(logConfig); //log levels set in your configuration
        logging.AddDebug(); //does all log levels
        logging.AddFile(logConfig.GetValue<string>("PathFormat"));
      }
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

    private void DumpSettings()
    {
      Console.WriteLine("Settings:");
      Console.WriteLine($"  DATASTORE:");
      Console.WriteLine($"    DATASTORE_CONNECTION         : {Settings.DATASTORE_CONNECTION(Configuration)}");
      Console.WriteLine($"    DATASTORE_CONNECTION_TYPE    : {Settings.DATASTORE_CONNECTION_TYPE(Configuration, Settings.DATASTORE_CONNECTION(Configuration))}");
      Console.WriteLine($"    DATASTORE_CONNECTION_STRING  : {Settings.DATASTORE_CONNECTION_STRING(Configuration, Settings.DATASTORE_CONNECTION(Configuration))}");

      Console.WriteLine($"  LOG:");
      Console.WriteLine($"    LOG_CONNECTION_STRING : {Settings.LOG_CONNECTION_STRING(Configuration)}");
      Console.WriteLine($"    LOG_BEARER_AUTH       : {Settings.LOG_BEARER_AUTH(Configuration)}");

      Console.WriteLine($"  OIDC:");
      Console.WriteLine($"    OIDC_USERINFO_URL : {Settings.OIDC_USERINFO_URL(Configuration)}");
      Console.WriteLine($"    OIDC_ISSUER_URL   : {Settings.OIDC_ISSUER_URL(Configuration)}");
      Console.WriteLine($"    OIDC_AUDIENCE     : {Settings.OIDC_AUDIENCE(Configuration)}");

      Console.WriteLine($"  CACHE:");
      Console.WriteLine($"    CACHE_HOST : {Settings.CACHE_HOST(Configuration)}");

      Console.WriteLine($"  KESTREL:");
      Console.WriteLine($"    KESTREL_CERTIFICATE_FILENAME  : {Settings.KESTREL_CERTIFICATE_FILENAME(Configuration)}");
      Console.WriteLine($"    KESTREL_CERTIFICATE_PASSWORD  : {Settings.KESTREL_CERTIFICATE_PASSWORD(Configuration)}");
      Console.WriteLine($"    KESTREL_URLS                  : {Settings.KESTREL_URLS(Configuration)}");
      Console.WriteLine($"    KESTREL_HTTPS_PORT            : {Settings.KESTREL_HTTPS_PORT(Configuration)}");
    }
  }
}
