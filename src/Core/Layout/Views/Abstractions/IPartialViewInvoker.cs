﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PlatoCore.Layout.Views.Abstractions
{
    public interface IPartialViewInvoker
    {

        ViewContext ViewContext { get; set; }

        void Contextualize(ViewContext viewContext);

        Task<IHtmlContent> InvokeAsync(string viewName, object model, ViewDataDictionary viewData);

    }

}