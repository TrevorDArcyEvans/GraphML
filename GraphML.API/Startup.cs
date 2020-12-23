using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using Flurl;

namespace GraphML.API
{
  internal sealed class Startup
  {
    private IWebHostEnvironment CurrentEnvironment { get; }
    private IConfiguration Configuration { get; }

    public Startup(
    IWebHostEnvironment env,
    IConfiguration conf)
    {
      // Environment variable:
      //    ASPNETCORE_ENVIRONMENT == Development
      CurrentEnvironment = env;
      Configuration = conf;

      // database connection string for nLog
      GlobalDiagnosticsContext.Set("LOG_CONNECTION_STRING", Configuration.LOG_CONNECTION_STRING());

      Settings.DumpSettings(Configuration);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(sp => Configuration);
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      SqlMapper.AddTypeHandler(new GuidTypeHandler());
      SqlMapper.RemoveTypeMap(typeof(Guid));
      SqlMapper.RemoveTypeMap(typeof(Guid?));

      // Add controllers as services so they'll be resolved.
      services
        .AddMvc(o =>
        {
          o.RespectBrowserAcceptHeader = true;
          o.EnableEndpointRouting = false;

          var settings = new JsonSerializerSettings()
          {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
          };
          var sp = services.BuildServiceProvider();
          var logger = sp.GetService<ILoggerFactory>();
          var objectPoolProvider = sp.GetService<ObjectPoolProvider>();
        })
        .AddControllersAsServices()
        .AddNewtonsoftJson(options =>
          options.SerializerSettings.Converters.Add(new StringEnumConverter()));

      if (CurrentEnvironment.IsDevelopment())
      {
        services.AddSwaggerGenNewtonsoftSupport();

        // Register the Swagger generator, defining one or more Swagger documents
        services.AddSwaggerGen(options =>
        {
          options.SwaggerDoc("v1",
        new OpenApiInfo
        {
          Title = "GraphML API",
          Version = "v1",
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

          //Set the comments path for the Swagger JSON and UI.
          var xmlPath = Path.Combine(AppContext.BaseDirectory, "GraphML.API.xml");
          options.IncludeXmlComments(xmlPath);

          options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
          {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
              AuthorizationCode = new OpenApiOAuthFlow
              {
                AuthorizationUrl = new Uri(Url.Combine(Configuration.IDENTITY_SERVER_BASE_URL(), Configuration.IDENTITY_SERVER_AUTHORIZATION_REL_URL())),
                TokenUrl = new Uri(Url.Combine(Configuration.IDENTITY_SERVER_BASE_URL(), Configuration.IDENTITY_SERVER_TOKEN_REL_URL())),
                Scopes = new Dictionary<string, string>
            {
          { "identityApi", "Full access to API #1" } // TODO settings
            }
              }
            }
          });

          options.OperationFilter<AuthorizeCheckOperationFilter>();
        });
      }

      services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
          options.Authority = Configuration.IDENTITY_SERVER_BASE_URL();
          options.RequireHttpsMetadata = false;

          options.Audience = Configuration.IDENTITY_SERVER_AUDIENCE();
        });

      services
        .AddCors(options =>
        {
          // this defines a CORS policy called "default"
          options.AddPolicy("default", policy =>
          {
            policy.WithOrigins(Configuration.API_URI())
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
        });

#region Autofac

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

      var assys = assyPaths.Select(filePath => Assembly.LoadFrom(filePath)).ToList();
      assys.Add(exeAssy);
      builder
        .RegisterAssemblyTypes(assys.ToArray())
        .PublicOnly()
        .AsImplementedInterfaces()
        .SingleInstance();

      builder.Register(cc => Configuration).As<IConfiguration>();

      var applicationContainer = builder.Build();

      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(applicationContainer);

#endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
      if (CurrentEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseCors("default");

      app.UseAuthentication();
      app.UseAuthorization();

      if (CurrentEnvironment.IsDevelopment())
      {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(opts =>
        {
          opts.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphML API v1");

          opts.OAuthClientId("graphml_api_swagger");
          opts.OAuthAppName("Swagger UI for GraphML API");
          opts.OAuthUsePkce();

          opts.DocExpansion(DocExpansion.None);
        });
      }

      app.UseStaticFiles();
      app.UseMvc();
    }
  }
}
