using Devmoba.ToolManager.BackgroundWorkers;
using Devmoba.ToolManager.EntityFrameworkCore;
using Devmoba.ToolManager.Localization;
using Devmoba.ToolManager.MultiTenancy;
using Devmoba.ToolManager.Permissions;
using Devmoba.ToolManager.Web.BundleContributors;
using Devmoba.ToolManager.Web.Menus;
using Devmoba.ToolManager.Web.Pages.Shared.Components.FluidContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Devmoba.ToolManager.Web
{
    [DependsOn(
        typeof(ToolManagerHttpApiModule),
        typeof(ToolManagerApplicationModule),
        typeof(ToolManagerEntityFrameworkCoreDbMigrationsModule),
        typeof(ToolManagerBackgroundWorkersModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreSignalRModule)
        )]
    public class ToolManagerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(ToolManagerResource),
                    typeof(ToolManagerDomainModule).Assembly,
                    typeof(ToolManagerDomainSharedModule).Assembly,
                    typeof(ToolManagerApplicationModule).Assembly,
                    typeof(ToolManagerApplicationContractsModule).Assembly,
                    typeof(ToolManagerWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/RunScripts/Index", ToolManagerPermissions.Scripts.RunScript);
                options.Conventions.AuthorizePage("/Scripts", ToolManagerPermissions.Scripts.Default);
                options.Conventions.AuthorizePage("/Scripts/Create", ToolManagerPermissions.Scripts.Create);
                options.Conventions.AuthorizePage("/Scripts/Edit", ToolManagerPermissions.Scripts.Edit);

                options.Conventions.AuthorizePage("/ClientMachines", ToolManagerPermissions.ClientMachines.Default);
                options.Conventions.AuthorizePage("/Tools", ToolManagerPermissions.Tools.Default);
            });

            ConfigureUrls(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);

            Configure<AbpLayoutHookOptions>(options =>
            {
                options.Add(LayoutHooks.Body.Last, typeof(FluidContainerViewComponent));
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    StandardBundles.Styles.Global, //The bundle name!
                    bundleConfiguration =>
                    {
                        bundleConfiguration.AddContributors(typeof(GlobalStyleBundleContributor));
                    }
                );

                options.ScriptBundles.Configure(
                    StandardBundles.Scripts.Global,
                    bundleConfig =>
                    {
                        bundleConfig.AddContributors(typeof(GlobalScriptBundleContributor));
                    }
                );

                options.ScriptBundles.Add("knockout", bundle => bundle.AddFiles("/libs/knockout-js/knockout.js"));
                options.ScriptBundles.Add("chosen",
                    bundle => bundle.AddFiles(
                        "/libs/chosen/chosen.jquery.js",
                        "/libs/chosen/chosen.ko.js"
                    ));
                options.ScriptBundles.Add("code-mirror",
                    bundle => bundle.AddFiles(
                        "/libs/codemirror/js/codemirror.js",
                        "/libs/codemirror/js/mode/javascript.js",
                        "/libs/codemirror/js/active-line.js",
                        "/libs/codemirror/js/comment.js",
                        "/libs/codemirror/js/continuecomment.js",
                        "/libs/codemirror/js/matchbrackets.js",
                        "/libs/codemirror/js/hint/javascript-hint.js",
                        "/libs/codemirror/js/hint/show-hint.js"
                    ));
                
                options.StyleBundles.Add("chosen", bundle => bundle.AddFiles("/styles/chosen/component-chosen.css"));
                options.StyleBundles.Add("code-mirror",
                    bundle => bundle.AddFiles(
                        "/libs/codemirror/css/codemirror.css",
                        "/libs/codemirror/css/monokai.css",
                        "/libs/codemirror/css/show-hint.css"
                    ));

               
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "ToolManager";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ToolManagerWebModule>();

            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<ToolManagerDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Devmoba.ToolManager.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ToolManagerDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Devmoba.ToolManager.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ToolManagerApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Devmoba.ToolManager.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ToolManagerApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Devmoba.ToolManager.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ToolManagerWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("vi", "vi", "Vietnamese"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new ToolManagerMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ToolManagerApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToolManager API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToolManager API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
