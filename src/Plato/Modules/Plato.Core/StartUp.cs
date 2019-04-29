﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Plato.Core.Assets;
using Plato.Internal.Abstractions.SetUp;
using Plato.Core.Handlers;
using Plato.Core.Middleware;
using Plato.Core.Models;
using Plato.Core.ViewFeatures;
using Plato.Core.ViewProviders;
using Plato.Internal.Assets.Abstractions;
using Plato.Internal.Models.Shell;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Layout.ViewFeatures;
using Plato.Internal.Layout.ViewProviders;
using Plato.Internal.Localization.Abstractions.Models;

namespace Plato.Core
{
    public class Startup : StartupBase
    {
        private readonly IShellSettings _shellSettings;
        private readonly string _tenantPrefix;
        private readonly string _cookieSuffix;

        public Startup(IShellSettings shellSettings)
        {
            _shellSettings = shellSettings;
            _tenantPrefix = shellSettings.RequestedUrlPrefix;
            _cookieSuffix = shellSettings.AuthCookieName;
        }

        public override void ConfigureServices(IServiceCollection services)
        {

            // Set-up event handler
            services.AddScoped<ISetUpEventHandler, SetUpEventHandler>();

            // Register client resources
            services.AddScoped<IAssetProvider, AssetProvider>();

            // Register view providers
            services.AddScoped<IViewProviderManager<HomeIndex>, ViewProviderManager<HomeIndex>>();
            services.AddScoped<IViewProvider<HomeIndex>, HomeViewProvider>();
            
            // Add theming conventions - configures theme layout based on controller prefix
            services.AddSingleton<IModularViewsFeatureProvider<ViewsFeature>, ModularThemingViewsFeatureProvider>();

            // Configure current culture
            services.Configure<LocaleOptions>(options =>
            {
                var contextFacade = services.BuildServiceProvider().GetService<IContextFacade>();
                options.WatchForChanges = false;
                options.Culture = contextFacade.GetCurrentCultureAsync().Result;
            });

        }

        public override void Configure(
            IApplicationBuilder app,
            IRouteBuilder routes,
            IServiceProvider serviceProvider)
        {

            // Register client options middleware 
            app.UseMiddleware<SettingsClientOptionsMiddleware>();
        
            // Homepage
            routes.MapAreaRoute(
                name: "Homepage",
                areaName: "Plato.Core",
                template: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            // Unauthorized - 401
            routes.MapAreaRoute(
                name: "UnauthorizedPage",
                areaName: "Plato.Core",
                template: "denied",
                defaults: new { controller = "Home", action = "Denied" }
            );

            // Moved - 404
            routes.MapAreaRoute(
                name: "MovedPage",
                areaName: "Plato.Core",
                template: "moved",
                defaults: new { controller = "Home", action = "Moved" }
            );

            // Error page - 500
            routes.MapAreaRoute(
                name: "ErrorPage",
                areaName: "Plato.Core",
                template: "error",
                defaults: new { controller = "Home", action = "Error" }
            );
            
        }

    }

}