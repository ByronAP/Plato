﻿using PlatoCore.Abstractions;

namespace Plato.Tags.Services
{

    public interface ITagManager<TTag> : ICommandManager<TTag> where TTag : class
    {
    }
    
}
