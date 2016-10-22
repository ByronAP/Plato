﻿using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plato.Abstractions.Settings
{
    public sealed class SiteSettings : ISettingValue, ISiteSettings
    {

        public string BaseUrl { get; set; }

        public string Calendar { get; set; }

        public string Culture { get; set; }

        public int MaxPagedCount { get; set; }

        public int MaxPageSize { get; set; }

        public int PageSize { get; set; }

        public string PageTitleSeparator { get; set; }
   
        public string SiteName { get; set; }

        public string SiteSalt { get; set; }

        public string SuperUser { get; set; }

        public string TimeZone { get; set; }

        public bool UseCdn { get; set; }

        public RouteValueDictionary HomeRoute { get; set; }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public SiteSettings Deserialize<T>(string json)
        {
            throw new NotImplementedException();
        }
    }
}
