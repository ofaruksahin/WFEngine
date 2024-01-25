using Serilog;
using System.Reflection;
using WFEngine.Application.Common.Extensions;
using WFEngine.Application.Common.Logging;
using WFEngine.Application.Common.Resources;
using WFEngine.Domain.Common.ValueObjects;
using WFEngine.Presentation.AuthorizationServer.Middlewares;

namespace WFEngine.Presentation.AuthorizationServer.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddWFEngineAuthorizationDependencies(this WebApplicationBuilder @this, IConfiguration configuration)
        {
            Log.Logger = SerilogBootstrapper.GetLoggerConfiguration(configuration.GetOptions<LoggingOptions>());

            @this.Services.RegisterOptions(configuration);
            @this.Services.AddExceptionHandler<CustomExceptionHandler>();

            @this.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            @this.Services.Configure<RequestLocalizationOptions>(configure =>
            {
                var options = configuration.GetOptions<InternationalizationOptions>();

                configure.SetDefaultCulture(options.DefaultCulture);
                configure.AddSupportedUICultures(options.SupportedCultures);
                configure.FallBackToParentUICultures = true;
                configure.RequestCultureProviders.Clear();
            });

            @this.Services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    var options = configuration.GetOptions<CorsOptions>();

                    policy
                        .WithOrigins(options.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            @this
                .Services
                .AddControllersWithViews()
                .AddMvcLocalization()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(configure =>
                {
                    configure.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ValidationResources).GetTypeInfo().Assembly.FullName!);
                        return factory.Create(nameof(ValidationResources), assemblyName.Name!);
                    };
                });

            @this.Host.UseSerilog();

            return @this;
        }

        public static WebApplication AddWFEngineAuthorizationDependencies(this WebApplication @this)
        {
            @this.UseExceptionHandler("/Authorization/Error");
            @this.UseRequestLocalization();
            @this.UseCors();
            return @this;
        }
    }
}
