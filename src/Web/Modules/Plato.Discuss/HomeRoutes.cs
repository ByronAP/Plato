﻿using System.Collections.Generic;
using PlatoCore.Abstractions.Routing;

namespace Plato.Discuss
{

    public class HomeRoutes : IHomeRouteProvider
    {
        public IEnumerable<HomeRoute> GetRoutes()
        {
            return new[]
            {
                new HomeRoute("Plato.Discuss", "Home", "Index")
            };
        }

    }

}
