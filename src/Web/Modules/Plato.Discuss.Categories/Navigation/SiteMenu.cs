﻿using System;
using Microsoft.Extensions.Localization;
using PlatoCore.Navigation.Abstractions;

namespace Plato.Discuss.Categories.Navigation
{
    public class SiteMenu : INavigationProvider
    {
        public SiteMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, INavigationBuilder builder)
        {
            if (!String.Equals(name, "site", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            
            // Removed

        }

    }

}
