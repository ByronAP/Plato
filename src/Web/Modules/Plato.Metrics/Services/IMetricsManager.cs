﻿using PlatoCore.Abstractions;

namespace Plato.Metrics.Services
{
    public interface IMetricsManager<TReaction> : ICommandManager<TReaction> where TReaction : class
    {
    }

}
