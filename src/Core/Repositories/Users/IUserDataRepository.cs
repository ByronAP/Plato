﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatoCore.Repositories.Users
{
    public interface IUserDataRepository<T> : IRepository<T> where T : class
    {
        Task<T> SelectByKeyAndUserIdAsync(string key, int userId);

        Task<IEnumerable<T>> SelectByUserIdAsync(int userId);
    }
}
