using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WFEngine.Application.Common.Extensions;
using WFEngine.Application.Common.Logging;
using WFEngine.Application.Common.Options;
using WFEngine.Domain.Common.ValueObjects;
using WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.Caching;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Interceptors;
using WFEngine.Infrastructure.Common.Interceptors.Extensions;
using WFEngine.Infrastructure.Common.IoC.Extensions;
using WFEngine.Infrastructure.Common.ValueObjects;
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
                .AddViewLocalization();

            @this.Host.UseSerilog();

            @this.Services.AddMediatR(configure =>
            {
                var mediatorOptions = configuration.GetOptions<MediatorOptions>();
                configure.RegisterServicesFromAssemblies(mediatorOptions.Assemblies);
            });

            var mapperOptions = configuration.GetOptions<MapperOptions>();
            @this.Services.AddAutoMapper(mapperOptions.Assemblies);

            @this.Services.AddFluentValidation(configure =>
            {
                var validatorOptions = configuration.GetOptions<FluentValidationOptions>();
                configure.RegisterValidatorsFromAssemblies(validatorOptions.Assemblies);
            });

            @this.Services.InjectServices();
            @this.Services.AddRepositoryInterceptors(configuration);
            @this.Services.AddCache(@this.Configuration);

            @this.Services.AddDbContext<AuthorizationPersistedGrantDbContext>((sp, configure) =>
            {
                var connectionStringsOptions = configuration.GetOptions<ConnectionStringOptions>();

                configure.UseMySQL(connectionStringsOptions.AuthorizationPersistedGrantDbContext);
                configure.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
            });

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
