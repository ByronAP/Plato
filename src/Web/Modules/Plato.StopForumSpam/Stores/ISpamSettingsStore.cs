﻿using PlatoCore.Stores.Abstractions;

namespace Plato.StopForumSpam.Stores
{
    public interface ISpamSettingsStore<T> : ISettingsStore<T> where T : class
    {
    }

}
