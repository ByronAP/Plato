﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Plato.Badges.Models;
using Plato.Badges.Services;
using Plato.Internal.Models.Shell;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Layout.ViewProviders;
using Plato.Internal.Models.Users;
using Plato.Internal.Navigation;
using Plato.Users.Badges.Navigation;
using Plato.Users.Badges.Providers;
using Plato.Users.Badges.ViewProviders;

namespace Plato.Users.Badges
{
    public class Startup : StartupBase
    {
        private readonly IShellSettings _shellSettings;

        public Startup(IShellSettings shellSettings)
        {
            _shellSettings = shellSettings;
        }

        public override void ConfigureServices(IServiceCollection services)
        {

            // Badge providers
            services.AddScoped<IBadgesProvider<Badge>, VisitBadges>();
            services.AddScoped<IBadgesProvider<Badge>, ProfileBadges>();

            // User profile view proviers
            services.AddScoped<IViewProviderManager<UserProfile>, ViewProviderManager<UserProfile>>();
            services.AddScoped<IViewProvider<UserProfile>, UserViewProvider>();

            // Badge view proviers
            services.AddScoped<IViewProviderManager<Badge>, ViewProviderManager<Badge>>();
            services.AddScoped<IViewProvider<Badge>, BadgeViewProvider>();

            // User badges view providers
            services.AddScoped<IViewProviderManager<UserBadge>, ViewProviderManager<UserBadge>>();
            services.AddScoped<IViewProvider<UserBadge>, UserBadgeViewProvider>();

            // Register navigation providers
            services.AddScoped<INavigationProvider, ProfileMenu>();
            services.AddScoped<INavigationProvider, SiteMenu>();

        }

        public override void Configure(
            IApplicationBuilder app,
            IRouteBuilder routes,
            IServiceProvider serviceProvider)
        {

            routes.MapAreaRoute(
                name: "BadgesIndex",
                areaName: "Plato.Users.Badges",
                template: "badges",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapAreaRoute(
                name: "DisplayUserBadges",
                areaName: "Plato.Users.Badges",
                template: "users/{id}/{alias?}/badges",
                defaults: new { controller = "Profile", action = "Index" }
            );

        }
    }
}