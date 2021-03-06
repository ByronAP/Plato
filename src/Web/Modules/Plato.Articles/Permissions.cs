﻿using System.Collections.Generic;
using PlatoCore.Security.Abstractions;

namespace Plato.Articles
{
    public class Permissions : IPermissionsProvider<Permission>
    {

        public static readonly Permission PostArticles =
            new Permission("PostArticles", "Post articles");

        public static readonly Permission PostArticleComments =
            new Permission("PostArticleComments", "Post comments");

        public static readonly Permission EditOwnArticles =
            new Permission("EditOwnArticles", "Edit own articles");

        public static readonly Permission EditAnyArticle =
            new Permission("EditAnyArticle", "Edit any article");
        
        public static readonly Permission EditOwnArticleComment =
            new Permission("EditOwnArticleComment", "Edit own comments");

        public static readonly Permission EditAnyArticleComment =
            new Permission("EditAnyArticleComment", "Edit any comment");
        
        public static readonly Permission DeleteOwnArticles = 
            new Permission("DeleteOwnArticles", "Soft delete own articles");

        public static readonly Permission RestoreOwnArticles =
            new Permission("RestoreOwnArticles", "Restore own soft deleted articles");

        public static readonly Permission PermanentDeleteOwnArticles =
            new Permission("PermanentDeleteOwnArticles", "Permanently delete own articles");

        public static readonly Permission DeleteAnyArticle =
            new Permission("DeleteAnyArticle", "Delete any article");

        public static readonly Permission RestoreAnyArticle =
            new Permission("RestoreAnyArticle", "Restore any article");
        
        public static readonly Permission PermanentDeleteAnyArticle =
            new Permission("PermanentDeleteAnyArticle", "Permanently delete any article");

        public static readonly Permission ViewDeletedArticles =
            new Permission("ViewDeletedArticles", "View soft deleted articles");
        
        public static readonly Permission DeleteOwnArticleComments =
            new Permission("DeleteOwnArticleComments", "Soft delete own comments");

        public static readonly Permission RestoreOwnArticleComments =
            new Permission("RestoreOwnArticleComments", "Restore own soft deleted comments");

        public static readonly Permission PermanentDeleteOwnArticleComments =
            new Permission("PermanentDeleteOwnArticleComments", "Permanently delete own comments");

        public static readonly Permission DeleteAnyArticleComment =
            new Permission("DeleteAnyArticleComment", "Soft delete any comment");

        public static readonly Permission RestoreAnyArticleComment =
            new Permission("RestoreAnyArticleComment", "Restore any soft deleted comment");

        public static readonly Permission PermanentDeleteAnyArticleComment =
            new Permission("PermanentDeleteAnyArticleComment", "Permanently delete any comment");

        public static readonly Permission ViewDeletedArticleComments =
            new Permission("ViewDeletedArticleComments", "View soft deleted comments");
        
        public static readonly Permission ReportArticles =
            new Permission("ReportArticles", "Report articles");

        public static readonly Permission ReportArticleComments =
            new Permission("ReportArticleComments", "Report comments");
        
        public static readonly Permission PinArticles =
            new Permission("PinArticles", "Pin articles");

        public static readonly Permission UnpinArticles =
            new Permission("UnpinArticles", "Unpin articles");

        public static readonly Permission LockArticles =
            new Permission("LockArticles", "Lock articles");

        public static readonly Permission UnlockArticles =
            new Permission("UnlockArticles", "Unlock articles");

        public static readonly Permission HideArticles =
            new Permission("HideArticles", "Hide articles");

        public static readonly Permission ShowArticles =
            new Permission("ShowArticles", "Unhide articles");

        public static readonly Permission ViewHiddenArticles =
            new Permission("ViewHiddenArticles", "View hidden articles");

        public static readonly Permission ViewPrivateArticles =
            new Permission("ViewPrivateArticles", "View private articles");

        public static readonly Permission HideArticleComments =
            new Permission("HideArticleComments", "Hide comments");

        public static readonly Permission ShowArticleComments =
            new Permission("ShowArticleComments", "Unhide comments");
        
        public static readonly Permission ViewHiddenArticleComments =
            new Permission("ViewHiddenArticleComments", "View hidden comments");

        public static readonly Permission ArticleToSpam =
            new Permission("ArticleToSpam", "Move articles to SPAM");

        public static readonly Permission ArticleFromSpam =
            new Permission("ArticleFromSpam", "Remove articles from SPAM");

        public static readonly Permission ViewSpamArticles =
            new Permission("ViewSpamArticles", "View articles flagged as SPAM");

        public static readonly Permission ArticleCommentToSpam =
            new Permission("ArticleCommentToSpam", "Move comments to SPAM");

        public static readonly Permission ArticleCommentFromSpam =
            new Permission("ArticleCommentFromSpam", "Remove comments from SPAM");

        public static readonly Permission ViewSpamArticleComments =
            new Permission("ViewSpamArticleComments", "View comments flagged as SPAM");
        
        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                PostArticles,
                PostArticleComments,
                EditOwnArticles,
                EditAnyArticle,
                EditOwnArticleComment,
                EditAnyArticleComment,
                DeleteOwnArticles,
                RestoreOwnArticles,
                PermanentDeleteOwnArticles,
                DeleteAnyArticle,
                RestoreAnyArticle,
                PermanentDeleteAnyArticle,
                ViewDeletedArticles,
                DeleteOwnArticleComments,
                RestoreOwnArticleComments,
                PermanentDeleteOwnArticleComments,
                DeleteAnyArticleComment,
                RestoreAnyArticleComment,
                PermanentDeleteAnyArticleComment,
                ViewDeletedArticleComments,
                ReportArticles,
                ReportArticleComments,
                PinArticles,
                UnpinArticles,
                LockArticles,
                UnlockArticles,
                HideArticles,
                ShowArticles,
                ViewHiddenArticles,
                ViewPrivateArticles,
                HideArticleComments,
                ShowArticleComments,
                ViewHiddenArticleComments,
                ArticleToSpam,
                ArticleFromSpam,
                ViewSpamArticles,
                ArticleCommentToSpam,
                ArticleCommentFromSpam,
                ViewSpamArticleComments
            };
        }

        public IEnumerable<DefaultPermissions<Permission>> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissions<Permission>
                {
                    RoleName = DefaultRoles.Administrator,
                    Permissions = new[]
                    {
                        PostArticles,
                        PostArticleComments,
                        EditOwnArticles,
                        EditAnyArticle,
                        EditOwnArticleComment,
                        EditAnyArticleComment,
                        DeleteOwnArticles,
                        RestoreOwnArticles,
                        PermanentDeleteOwnArticles,
                        DeleteAnyArticle,
                        RestoreAnyArticle,
                        PermanentDeleteAnyArticle,
                        ViewDeletedArticles,
                        DeleteOwnArticleComments,
                        RestoreOwnArticleComments,
                        PermanentDeleteOwnArticleComments,
                        DeleteAnyArticleComment,
                        RestoreAnyArticleComment,
                        PermanentDeleteAnyArticleComment,
                        ViewDeletedArticleComments,
                        ReportArticles,
                        ReportArticleComments,
                        PinArticles,
                        UnpinArticles,
                        LockArticles,
                        UnlockArticles,
                        HideArticles,
                        ShowArticles,
                        ViewHiddenArticles,
                        ViewPrivateArticles,
                        HideArticleComments,
                        ShowArticleComments,
                        ViewHiddenArticleComments,
                        ArticleToSpam,
                        ArticleFromSpam,
                        ViewSpamArticles,
                        ArticleCommentToSpam,
                        ArticleCommentFromSpam,
                        ViewSpamArticleComments
                    }
                },
                new DefaultPermissions<Permission>
                {
                    RoleName = DefaultRoles.Member,
                    Permissions = new[]
                    {
                        PostArticleComments,
                        EditOwnArticleComment,
                        DeleteOwnArticleComments,
                        ReportArticles,
                        ReportArticleComments
                    }
                },
                new DefaultPermissions<Permission>
                {
                    RoleName = DefaultRoles.Staff,
                    Permissions = new[]
                    {
                        PostArticles,
                        PostArticleComments,
                        EditOwnArticles,
                        EditOwnArticleComment,
                        DeleteOwnArticles,
                        RestoreOwnArticles,
                        PermanentDeleteOwnArticles,
                        ViewDeletedArticles,
                        DeleteOwnArticleComments,
                        RestoreOwnArticleComments,
                        PermanentDeleteOwnArticleComments,
                        ViewDeletedArticleComments,
                        ReportArticles,
                        ReportArticleComments,
                        PinArticles,
                        UnpinArticles,
                        LockArticles,
                        UnlockArticles,
                        HideArticles,
                        ShowArticles,
                        ViewHiddenArticles,
                        ViewPrivateArticles,
                        HideArticleComments,
                        ShowArticleComments,
                        ViewHiddenArticleComments,
                        ArticleToSpam,
                        ArticleFromSpam,
                        ViewSpamArticles,
                        ArticleCommentToSpam,
                        ArticleCommentFromSpam,
                        ViewSpamArticleComments
                    }
                },
                new DefaultPermissions<Permission>
                {
                    RoleName = DefaultRoles.Anonymous,
                    Permissions = new[]
                    {
                        ReportArticles,
                        ReportArticleComments
                    }
                }
            };
        }

    }

}
