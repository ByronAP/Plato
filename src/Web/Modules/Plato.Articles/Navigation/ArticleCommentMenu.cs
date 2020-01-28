﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Plato.Articles.Models;
using Plato.Entities.Extensions;
using PlatoCore.Models.Users;
using PlatoCore.Navigation.Abstractions;
using PlatoCore.Security.Abstractions;

namespace Plato.Articles.Navigation
{

    public class ArticleCommentMenu : INavigationProvider
    {
        
        public IStringLocalizer T { get; set; }

        public ArticleCommentMenu(IStringLocalizer localizer)
        {
            T = localizer;
        }

        public void BuildNavigation(string name, INavigationBuilder builder)
        {

            if (!String.Equals(name, "article-comment", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Get entity from context
            var entity = builder.ActionContext.HttpContext.Items[typeof(Article)] as Article;
            if (entity == null)
            {
                return;
            }
            
            // Get reply from context
            var reply = builder.ActionContext.HttpContext.Items[typeof(Comment)] as Comment;
            if (reply == null)
            {
                return;
            }

            //// Get authenticated user from features
            var user = builder.ActionContext.HttpContext.Features[typeof(User)] as User;

            // Get delete / restore permission
            Permission deletePermission = null;
            if (reply.IsDeleted)
            {
                deletePermission = user?.Id == reply.CreatedUserId
                    ? Permissions.RestoreOwnArticleComments
                    : Permissions.RestoreAnyArticleComment;
            }
            else
            {
                deletePermission = user?.Id == reply.CreatedUserId
                    ? Permissions.DeleteOwnArticleComments
                    : Permissions.DeleteAnyArticleComment;
            }
            
            // Options
            builder
                .Add(T["Options"], int.MaxValue, options => options
                        .IconCss("fa fa-ellipsis-h")
                        .Attributes(new Dictionary<string, object>()
                        {
                            {"data-provide", "tooltip"},
                            {"title", T["Options"]}
                        })
                        .Add(T["Edit"], int.MinValue, edit => edit
                            .Action("EditReply", "Home", "Plato.Articles", new RouteValueDictionary()
                            {
                                ["id"] = reply?.Id ?? 0
                            })
                            .Permission(user?.Id == reply.CreatedUserId ?
                                Permissions.EditOwnArticleComment :
                                Permissions.EditAnyArticleComment)
                            .LocalNav())
                        .Add(reply.IsHidden ? T["Unhide"] : T["Hide"], 2, edit => edit
                            .Action(reply.IsHidden ? "ShowReply" : "HideReply", "Home", "Plato.Articles",
                                new RouteValueDictionary()
                                {
                                    ["id"] = reply?.Id ?? 0
                                })
                            .Resource(entity.CategoryId)
                            .Permission(reply.IsHidden
                                ? Permissions.ShowArticleComments
                                : Permissions.HideArticleComments)
                            .LocalNav()
                        )
                        .Add(reply.IsSpam ? T["Not Spam"] : T["Spam"], 3, spam => spam
                            .Action(reply.IsSpam ? "ReplyFromSpam" : "ReplyToSpam", "Home", "Plato.Articles",
                                new RouteValueDictionary()
                                {
                                    ["id"] = reply?.Id ?? 0
                                })
                            .Resource(entity.CategoryId)
                            .Permission(reply.IsSpam
                                ? Permissions.ArticleCommentFromSpam
                                : Permissions.ArticleCommentToSpam)
                            .LocalNav()
                        )
                        .Add(T["Report"], int.MaxValue - 2, report => report
                            .Action("Report", "Home", "Plato.Articles", new RouteValueDictionary()
                            {
                                ["opts.id"] = entity.Id,
                                ["opts.alias"] = entity.Alias,
                                ["opts.replyId"] = reply.Id
                            })
                            .Attributes(new Dictionary<string, object>()
                            {
                                {"data-provide", "dialog"},
                                {"data-dialog-modal-css", "modal fade"},
                                {"data-dialog-css", "modal-dialog modal-lg"}
                            })
                            .Permission(Permissions.ReportArticleComments)
                            .LocalNav()
                        )
                        .Add(T["Divider"], int.MaxValue - 1, divider => divider
                            .Permission(deletePermission)
                            .DividerCss("dropdown-divider").LocalNav()
                        )
                        .Add(reply.IsDeleted ? T["Restore"] : T["Delete"], int.MaxValue, edit => edit
                                .Action(reply.IsDeleted ? "RestoreReply" : "DeleteReply", "Home", "Plato.Articles",
                                    new RouteValueDictionary()
                                    {
                                        ["id"] = reply.Id
                                    })
                                .Permission(deletePermission)
                                .LocalNav(),
                            reply.IsDeleted
                                ? new List<string>() { "dropdown-item", "dropdown-item-success" }
                                : new List<string>() { "dropdown-item", "dropdown-item-danger" }
                        )
                    , new List<string>() {"article-options", "text-muted", "dropdown-toggle-no-caret", "text-hidden"}
                );

            // If the reply if deleted display permanent delete option
            if (reply.IsDeleted)
            {

                // Permanent delete permissions
                var permanentDeletePermission = reply.CreatedUserId == user?.Id
                    ? Permissions.PermanentDeleteOwnArticleComments
                    : Permissions.PermanentDeleteAnyArticleComment;

                builder
                    .Add(T["Delete"], int.MinValue, options => options
                            .IconCss("fal fa-trash-alt")
                            .Attributes(new Dictionary<string, object>()
                            {
                                {"data-toggle", "tooltip"},
                                {"data-provide", "confirm"},
                                {"title", T["Permanent Delete"]},
                            })
                            .Action("PermanentDeleteReply", "Home", "Plato.Articles",
                                new RouteValueDictionary()
                                {
                                    ["id"] = reply.Id
                                })
                            .Permission(permanentDeletePermission)
                            .LocalNav()
                        , new List<string>() { "article-permanent-delete", "text-muted", "text-hidden" }
                    );
            }

            // If entity & reply are not hidden and entity is not locked allow replies
            if (!entity.IsHidden() && !reply.IsHidden() && !entity.IsLocked)
            {

                builder
                    .Add(T["Comment"], int.MaxValue, options => options
                            .IconCss("fa fa-reply")
                            .Attributes(new Dictionary<string, object>()
                                {
                                    {"data-provide", "postQuote"},
                                    {"data-quote-selector", "#quote" + reply.Id.ToString()},
                                    {"data-toggle", "tooltip"},
                                    {"title", T["Comment"]}
                                })
                            .Action("Login", "Account", "Plato.Users",
                                new RouteValueDictionary()
                                {
                                    ["returnUrl"] = builder.ActionContext.HttpContext.Request.Path
                                })
                            .Permission(Permissions.PostArticleComments)
                            .LocalNav()
                        , new List<string>() { "article-reply", "text-muted", "text-hidden" }
                    );

            }



        }

    }

}
