﻿using Microsoft.Extensions.Localization;
using System;
using PlatoCore.Navigation;
using PlatoCore.Navigation.Abstractions;

namespace Plato.Admin.Navigation
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, INavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(T["Home"], "0", home => home
                    .IconCss("fal fa-home")
                    .Action("Index", "Admin", "Plato.Admin")
                    //.Permission(Permissions.ManageRoles)
                    .LocalNav()
                );


        }
    }

}
