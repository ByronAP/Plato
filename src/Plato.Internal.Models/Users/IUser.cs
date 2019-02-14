﻿using System;
using System.Collections.Generic;

namespace Plato.Internal.Models.Users
{

    public interface IUser : ISimpleUser
    {

        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }
        
        string Password { get; set; }
        
        DateTimeOffset? PasswordExpiryDate { get; set; }

        DateTimeOffset? PasswordUpdatedDate { get; set; }
        
        string TimeZone { get; set; }

        bool ObserveDst { get; set; }

        string Culture { get; set; }

        string Theme { get; set; }

        string IpV4Address { get; set; }

        string IpV6Address { get; set; }

        int Visits { get; set; }

        DateTimeOffset? VisitsUpdatedDate { get; set; }

        int MinutesActive { get; set; }

        DateTimeOffset? MinutesActiveUpdatedDate { get; set; }
        
        int Reputation { get; set; }

        DateTimeOffset? ReputationUpdatedDate { get; set; }
        
        int Rank { get; set; }

        DateTimeOffset? RankUpdatedDate { get; set; }

        string Signature { get; set; }

        bool IsSpam { get; set; }

        bool IsBlackListed { get; set; }

        bool IsWhiteListed { get; set; }

        DateTimeOffset? BlackListedExpiryDate { get; set; }

        DateTimeOffset? WhiteListedExpiryDate { get; set; }
        
        UserType UserType { get; set; }

        int CreatedUserId { get; set; }

        DateTimeOffset? CreatedDate { get; set; }
        
        int ModifiedUserId { get; set; }

        DateTimeOffset? ModifiedDate { get; set; }
        
        IEnumerable<string> RoleNames { get; set; }

        string ResetToken { get; set; }

        string ConfirmationToken { get; set; }
    }

    public enum UserType
    {
        None = 0,
        Bot = 1
    }

}
