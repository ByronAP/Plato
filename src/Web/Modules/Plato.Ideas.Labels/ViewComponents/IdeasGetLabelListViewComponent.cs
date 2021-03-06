﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plato.Ideas.Labels.Models;
using Plato.Labels.Services;
using Plato.Labels.ViewModels;
using PlatoCore.Navigation.Abstractions;

namespace Plato.Ideas.Labels.ViewComponents
{

    public class IdeasGetLabelListViewComponent : ViewComponent
    {
        
        private readonly ILabelService<Label> _labelService;

        public IdeasGetLabelListViewComponent(
            ILabelService<Label> labelService)
        {
            _labelService = labelService;
        }

        public async Task<IViewComponentResult> InvokeAsync(LabelIndexOptions options, PagerOptions pager)
        {

            if (options == null)
            {
                options = new LabelIndexOptions();
            }

            if (pager == null)
            {
                pager = new PagerOptions();
            }

            return View(await GetViewModel(options, pager));

        }

        async Task<LabelIndexViewModel<Label>> GetViewModel(LabelIndexOptions options, PagerOptions pager)
        {

            var results = await _labelService
                .GetResultsAsync(options, pager);

            // Set total on pager
            pager.SetTotal(results?.Total ?? 0);

            // Return view model
            return new LabelIndexViewModel<Label>
            {
                Results = results,
                Options = options,
                Pager = pager
            };

        }

    }


}
