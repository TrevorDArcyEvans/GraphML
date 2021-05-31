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
using Flurl;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Features;
using GraphML.Datastore.Database.Importer;
using Microsoft.AspNetCore.ResponseCompression;

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
        })
        .AddControllersAsServices()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          options.SerializerSettings.Formatting = Formatting.Indented;
          options.SerializerSettings.Converters.Add(new StringEnumConverter());

          // https://stackoverflow.com/questions/18193281/force-json-net-to-include-milliseconds-when-serializing-datetime-even-if-ms-com
          // https://stackoverflow.com/questions/10286204/what-is-the-right-json-date-format
          var dateConverter = new IsoDateTimeConverter
          {
            DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"
          };
          options.SerializerSettings.Converters.Add(dateConverter);
        });
      services.AddResponseCompression(options =>
      {
        options.EnableForHttps = true;
        options.Providers.Add<GzipCompressionProvider>();
      });

      services.Configure<FormOptions>(x =>
      {
        x.ValueLengthLimit = int.MaxValue;
        x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
      });

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
                Scopes = Configuration.IDENTITY_SERVER_SCOPES().ToDictionary(x => x)
              }
            }
          });

          options.OperationFilter<AuthorizeCheckOperationFilter>();
          options.DocumentFilter<ApiModelDocumentFilter<ImportSpecification>>();

          options.EnableAnnotations();
        });
      }

      services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
          options.Authority = Configuration.IDENTITY_SERVER_BASE_URL();
          options.RequireHttpsMetadata = false;
          options.BackchannelHttpHandler = new HttpClientHandler
          {
            ServerCertificateCustomValidationCallback = delegate { return true; }
          };
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
              .AllowAnyMethod()
              .AllowAnyOrigin();
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
      app.UseResponseCompression();
      app.UseMvc();
    }
  }
}
