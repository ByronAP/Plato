﻿using PlatoCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Entities.Repositories
{

    public interface IEntityRepository<T> : IRepository<T> where T : class
    {
        Task<IEnumerable<T>> SelectByFeatureIdAsync(int featureId);
    } 

}
