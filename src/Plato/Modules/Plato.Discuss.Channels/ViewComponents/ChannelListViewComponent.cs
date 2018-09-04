﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plato.Categories.Stores;
using Plato.Discuss.Channels.Models;
using Plato.Discuss.Channels.ViewModels;
using Plato.Discuss.ViewModels;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Models.Shell;

namespace Plato.Discuss.Channels.ViewComponents
{

    public class ChannelListViewComponent : ViewComponent
    {
        private readonly ICategoryStore<Channel> _channelStore;
        private readonly IContextFacade _contextFacade;

        public ChannelListViewComponent(
            ICategoryStore<Channel> channelStore,
            IContextFacade contextFacade)
        {
            _channelStore = channelStore;
            _contextFacade = contextFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            FilterOptions filterOpts)
        {

            if (filterOpts == null)
            {
                filterOpts = new FilterOptions();
            }

            var model = await GetIndexModel(filterOpts);
            model.SelectedChannelId = filterOpts.ChannelId;
            return View(model);

        }
        
        async Task<ChannelListViewModel> GetIndexModel(FilterOptions filterOpts)
        {
            var feature = await GetcurrentFeature();
            var categories = await _channelStore.GetByFeatureIdAsync(feature.Id);
            return new ChannelListViewModel()
            {
                Channels = categories?.Where(c => c.ParentId == filterOpts.ChannelId)
            };
        }

        async Task<ShellModule> GetcurrentFeature()
        {
            var featureId = "Plato.Discuss.Channels";
            var feature = await _contextFacade.GetFeatureByModuleIdAsync(featureId);
            if (feature == null)
            {
                throw new Exception($"No feature could be found for the Id '{featureId}'");
            }
            return feature;
        }

        private async Task<IList<Selection<Channel>>> BuildSelectionsAsync(
            IEnumerable<int> selected)
        {

            var feature = await _contextFacade.GetFeatureByModuleIdAsync("Plato.Discuss.Channels");
            var channels = await _channelStore.GetByFeatureIdAsync(feature.Id);

            var selections = channels?.Select(c => new Selection<Channel>
                {
                    IsSelected = selected.Any(v => v == c.Id),
                    Value = c
                })
                .ToList();

            return selections;
        }
    }


}
