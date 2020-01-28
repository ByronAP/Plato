﻿using System.Collections.Generic;
using PlatoCore.Models.Reputations;
using PlatoCore.Reputations.Abstractions;

namespace Plato.Users
{

    public class Reputations : IReputationsProvider<Reputation>
    {

        public static readonly Reputation UniqueVisit =
            new Reputation("Visit",  1);

        public static readonly Reputation NewTopicReputation =
            new Reputation("New Topic", 1);

        public IEnumerable<Reputation> GetReputations()
        {
            return new[]
            {
                UniqueVisit,
                NewTopicReputation
            };
        }

    }
}
