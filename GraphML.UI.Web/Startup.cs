using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using BlazorTable;
using GraphML.Common;
using MatBlazor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using NLog;

namespace GraphML.UI.Web
{
  public class Startup
  {
    private IWebHostEnvironment CurrentEnvironment { get; }
    private IConfiguration Configuration { get; }

    public Startup(
      IWebHostEnvironment env,
      IConfiguration configuration)
    {
      // Environment variable:
      //    ASPNETCORE_ENVIRONMENT == Development
      CurrentEnvironment = env;

      Configuration = configuration;

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

      // MatTable needs this
      // NOTE:  have to disable ssl checking, esp on Linux
      services.AddScoped(sp =>
      {
        var handler = new HttpClientHandler
        {
          ServerCertificateCustomValidationCallback = delegate { return true; },
          UseDefaultCredentials = true
        };
        var client = new HttpClient(handler);
        var ctx = sp.GetService<IHttpContextAccessor>();
        var accessToken = ctx.HttpContext.GetTokenAsync("access_token").Result; // TODO async
        if (string.IsNullOrEmpty(accessToken))
        {
          throw new AuthenticationException("Access token is missing");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        return client;
      });

      services
        .AddBlazorise()
        .AddBootstrapProviders()
        .AddFontAwesomeIcons();
      services.AddBlazorContextMenu();
      services.AddMatBlazor();
      services.AddBlazorTable();
      services.AddMatToaster(config =>
      {
        config.Position = MatToastPosition.BottomRight;
        config.PreventDuplicates = true;
        config.NewestOnTop = true;
        config.ShowCloseButton = true;
        config.MaximumOpacity = 95;
        config.VisibleStateDuration = 3000;
      });
      services.AddRazorPages().AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
      services.AddServerSideBlazor();

      services.AddAuthentication(options =>
        {
          options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
          options =>
          {
            options.Authority = Configuration.IDENTITY_SERVER_BASE_URL();
            options.ClientId = Configuration.IDENTITY_SERVER_CLIENT_ID();
            options.ClientSecret = Configuration.IDENTITY_SERVER_CLIENT_SECRET();
            options.UsePkce = true;
            options.ResponseType = "code";

            Configuration
              .IDENTITY_SERVER_SCOPES()
              .ToList()
              .ForEach(scope => options.Scope.Add(scope));

            //options.CallbackPath = ...
            options.RequireHttpsMetadata = false;
            options.BackchannelHttpHandler = new HttpClientHandler
            {
              ServerCertificateCustomValidationCallback = delegate { return true; }
            };
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
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
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
