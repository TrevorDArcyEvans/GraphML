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
using System.Runtime.Loader;
using Dapper;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using System.Collections.Generic;

namespace GraphML.API
{
	internal sealed class Startup
	{
		private static readonly object _lock = new object();

		private IServiceProvider ServiceProvider { get; set; }
		private IConfiguration Configuration { get; }
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
			  .AddEnvironmentVariables()
			  .AddUserSecrets<Program>();

			Configuration = builder.Build();

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
								AuthorizationUrl = new Uri("https://localhost:5000/connect/authorize"),
								TokenUrl = new Uri("https://localhost:5000/connect/token"),
								Scopes = new Dictionary<string, string>
						{
				  { "api1", "Full access to API #1" }
						}
							}
						}
					});

					options.OperationFilter<AuthorizeCheckOperationFilter>();
				});
			}

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			  .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.Authority = Configuration.IdentityServer_Authority();
				options.ApiName = Configuration.IdentityServer_ApiName();

				options.JwtBackChannelHandler = new HttpClientHandler
				{
					// accept (all) self-signed ssl certs for development
					ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
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

			var assys = assyPaths.Select(filePath => Assembly.LoadFrom(filePath)).ToList();
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
