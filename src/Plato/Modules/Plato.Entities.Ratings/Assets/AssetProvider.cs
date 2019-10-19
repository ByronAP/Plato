﻿using System.Collections.Generic;
using Plato.Internal.Assets.Abstractions;

namespace Plato.Entities.Ratings.Assets
{
    public class AssetProvider : IAssetProvider
    {

        public IEnumerable<AssetEnvironment> GetAssetEnvironments()
        {

            return new List<AssetEnvironment>
            {

                // Development
                new AssetEnvironment(TargetEnvironment.Development, new List<Asset>()
                {
                    new Asset()
                    {
                        Url = "/plato.entities.ratings/content/css/ratings.css",
                        Type = AssetType.IncludeCss,
                        Section = AssetSection.Header
                    },
                    new Asset()
                    {
                        Url = "/plato.entities.ratings/content/js/ratings.js",
                        Type = AssetType.IncludeJavaScript,
                        Section = AssetSection.Footer
                    }
                }),

                // Staging
                new AssetEnvironment(TargetEnvironment.Staging, new List<Asset>()
                {
                      new Asset()
                    {
                        Url = "/plato.entities.ratings/content/css/ratings.css",
                        Type = AssetType.IncludeCss,
                        Section = AssetSection.Header
                    },
                    new Asset()
                    {
                        Url = "/plato.entities.ratings/content/js/ratings.js",
                        Type = AssetType.IncludeJavaScript,
                        Section = AssetSection.Footer
                    }
                }),

                // Production
                new AssetEnvironment(TargetEnvironment.Production, new List<Asset>()
                {
                  new Asset()
                    {
                        Url = "/plato.entities.ratings/content/css/ratings.css",
                        Type = AssetType.IncludeCss,
                        Section = AssetSection.Header
                    },
                    new Asset()
                    {
                        Url = "/plato.entities.ratings/content/js/ratings.js",
                        Type = AssetType.IncludeJavaScript,
                        Section = AssetSection.Footer
                    }
                })

            };

        }

    }

}
