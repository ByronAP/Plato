﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PlatoCore.Models.Shell;
using PlatoCore.Hosting.Abstractions;
using PlatoCore.Messaging.Abstractions;
using Plato.Questions.Slack.Subscribers;

namespace Plato.Questions.Slack
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

            // Broker subscriptions
            services.AddScoped<IBrokerSubscriber, EntitySubscriber>();
            services.AddScoped<IBrokerSubscriber, EntityReplySubscriber>();

        }

        public override void Configure(
            IApplicationBuilder app,
            IRouteBuilder routes,
            IServiceProvider serviceProvider)
        {
        }
    }
}