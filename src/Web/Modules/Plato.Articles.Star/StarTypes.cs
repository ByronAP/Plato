﻿using System.Collections.Generic;
using Plato.Stars.Models;
using Plato.Stars.Services;

namespace Plato.Articles.Star
{

    public class StarTypes : IStarTypeProvider
    {
        
        public static readonly StarType Article =
            new StarType(
                "Article",
                "Star",
                "Star this article",
                "Unstar",
                "Delete star",
                "Login to star this article",
                "You don't have permission to star articles");

        public IEnumerable<IStarType> GetFollowTypes()
        {
            return new[]
            {
                Article
            };
        }

    }

}
