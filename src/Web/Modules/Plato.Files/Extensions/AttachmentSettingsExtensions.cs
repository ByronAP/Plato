﻿using Plato.Files.Models;
using PlatoCore.Models.Users;
using System.Collections.Generic;

namespace Plato.Files.Extensions
{
    public static class AttachmentSettingsExtensions
    {

        public static long GetAvailableSpace(this FileSettings settings, IUser user)
        {
           
            // We need to be authenticated to upload attachments
            if (user?.UserRoles == null)
            {
                return 0;
            }

            // No settings use defaults
            if (settings?.Settings == null)
            {
                return DefaultFileOptions.AvailableSpace;
            }

            // Calculate the highest available space for given roles
            long output = 0;
            var roles = user.UserRoles;
            foreach (var role in roles)
            {
                foreach (var setting in settings.Settings)
                {
                    if (setting.RoleId == role.Id)
                    {
                        if (setting.AvailableSpace > output)
                        {
                            output = setting.AvailableSpace;
                        }     
                    }
                }
            }

            return output;

        }

        public static long GetMaxFileSize(this FileSettings settings, IUser user)
        {
         
            // We need to be authenticated to upload attachments
            if (user?.UserRoles == null)
            {
                return 0;
            }

            // No settings use defaults
            if (settings?.Settings == null)
            {
                return DefaultFileOptions.AvailableSpace;
            }

            // Calculate the highest available space for given roles
            long output = 0;
            var roles = user.UserRoles;
            foreach (var role in roles)
            {
                foreach (var setting in settings.Settings)
                {
                    if (setting.RoleId == role.Id)
                    {
                        if (setting.MaxFileSize > output)
                        {
                            output = setting.MaxFileSize;
                        }
                    }
                }
            }

            return output;

        }

        public static string[] GetAllowedExtensions(this FileSettings settings, IUser user)
        {

            // We need to be authenticated to upload attachments
            if (user?.UserRoles == null)
            {
                return null;
            }

            // No settings use defaults
            if (settings == null)
            {
                return DefaultFileOptions.AllowedExtensions;
            }

            // No settings use defaults
            if (settings.Settings == null)
            {
                return DefaultFileOptions.AllowedExtensions;
            }

            // Accumulate all unique extensions for given roles
            List<string> output = null;
            var roles = user.UserRoles;
            foreach (var role in roles)
            {
                foreach (var setting in settings.Settings)
                {
                    if (setting.RoleId == role.Id)
                    {
                        foreach (var extension in setting.AllowedExtensions)
                        {
                            if (output == null)
                            {
                                output = new List<string>();
                            }
                            if (!output.Contains(extension))
                            {
                                output.Add(extension);
                            }
                        }
                    }
                }
            }

            return output?.ToArray();

        }

        public static bool Contains(this FileSettings settings, FileSetting comparer)
        {

            if (settings == null)
            {
                return false;
            }

            foreach (var setting in settings.Settings)
            {
                if (setting.RoleId == comparer.RoleId)
                {
                    return true;
                }
            }

            return false;

        }

    }

}
