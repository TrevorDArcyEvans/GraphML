using GraphML.API.Authentications;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
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

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      AssemblyLoadContext.Default.Resolving += OnAssemblyResolve;
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

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var jsonServices = JObject.Parse(File.ReadAllText("appsettings.json"))["Services"];
      var requiredServices = JsonConvert.DeserializeObject<List<Service>>(jsonServices.ToString());

      foreach (var service in requiredServices)
      {
        var serviceType = Type.GetType(service.ServiceType, true);
        var implementationType = Type.GetType(service.ImplementationType, true);
        services.Add(new ServiceDescriptor(
          serviceType: serviceType,
          implementationType: implementationType,
          lifetime: service.Lifetime));
      }

      services.AddSingleton(sp => Configuration);
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddMvc();
      services.Configure<MvcOptions>(options =>
      {
        options.Filters.Add(new RequireHttpsAttribute());
      });

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
                  return BasicAuthentication.Authenticate(context);
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
        options.Authority = Configuration["Jwt:Authority"];
        options.Audience = Configuration["Jwt:Audience"];
        options.Events = new JwtBearerEvents
        {
          OnTokenValidated = async context =>
          {
            await BearerAuthentication.Authenticate(Configuration, context);
          }
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logging)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      var httpsPort = Configuration.GetValue<int>(Constants.HttpsPortKey);
      var options = new RewriteOptions()
                      .AddRedirectToHttps((int)HttpStatusCode.MovedPermanently, httpsPort);

      app.UseRewriter(options);

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(opts =>
      {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphML API V1");
        opts.SwaggerEndpoint("/swagger/porcelain/swagger.json", "GraphML API V1 Porcelain");
      });

      app.UseStaticFiles();
      app.UseMvc();

      var logConfig = Configuration.GetSection("Logging");
      logging.AddConsole(logConfig); //log levels set in your configuration
      logging.AddDebug(); //does all log levels
      logging.AddFile(logConfig.GetValue<string>("PathFormat"));
    }
  }
}
