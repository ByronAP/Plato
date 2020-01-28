﻿using System;
using PlatoCore.Models.Notifications;
using PlatoCore.Models.Users;
using PlatoCore.Notifications.Abstractions;

namespace PlatoCore.Notifications.Extensions
{
    public static class UserExtensions
    {

        public static bool NotificationEnabled(
            this IUser user,
            IUserNotificationTypeDefaults userNotificationTypeDefaults,
            INotificationType notificationType)
        {

            foreach (var userNotificationType in userNotificationTypeDefaults.GetUserNotificationTypesWithDefaults(user))
            {
                if (userNotificationType.Name.Equals(notificationType.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return userNotificationType.Enabled;
                }
            }

            return false;

        }
        
    }

}
