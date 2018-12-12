﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Plato.Discuss.Models;
using Plato.Discuss.Services;
using Plato.Internal.Navigation;
using Plato.Discuss.ViewModels;
using Plato.Entities.Stores;
using Plato.Internal.Data.Abstractions;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Layout.Alerts;
using Plato.Internal.Layout.ModelBinding;
using Plato.Internal.Layout.ViewProviders;
using Plato.Internal.Models.Users;
using Plato.Internal.Scripting.Abstractions;
using Plato.Internal.Stores.Abstractions.Users;
using Plato.Internal.Stores.Users;

namespace Plato.Discuss.Controllers
{
    public class HomeController : Controller, IUpdateModel
    {

        #region "Constructor"
        
        private readonly IViewProviderManager<Topic> _topicViewProvider;
        private readonly IViewProviderManager<Reply> _replyViewProvider;
        private readonly IEntityStore<Topic> _entityStore;
        private readonly IEntityReplyStore<Reply> _entityReplyStore;
        private readonly IPostManager<Topic> _topicManager;
        private readonly IPostManager<Reply> _replyManager;
        private readonly IAlerter _alerter;
        private readonly IBreadCrumbManager _breadCrumbManager;

        private readonly IPlatoUserStore<User> _ploatUserStore;

        public IHtmlLocalizer T { get; }

        public IStringLocalizer S { get; }
        
        public HomeController(
            IStringLocalizer<HomeController> stringLocalizer,
            IHtmlLocalizer<HomeController> localizer,
            IContextFacade contextFacade,
            IEntityStore<Topic> entityStore,
            IViewProviderManager<Topic> topicViewProvider,
            IEntityReplyStore<Reply> entityReplyStore,
            IViewProviderManager<Reply> replyViewProvider,
            IPostManager<Topic> topicManager,
            IPostManager<Reply> replyManager,
            IAlerter alerter, IBreadCrumbManager breadCrumbManager,
            IPlatoUserStore<User> ploatUserStore)
        {
            _topicViewProvider = topicViewProvider;
            _replyViewProvider = replyViewProvider;
            _entityStore = entityStore;
            _entityReplyStore = entityReplyStore;
            _topicManager = topicManager;
            _replyManager = replyManager;
            _alerter = alerter;
            _breadCrumbManager = breadCrumbManager;
            _ploatUserStore = ploatUserStore;

            T = localizer;
            S = stringLocalizer;

        }

        #endregion

        #region "Actions"
        
        public async Task<IActionResult> Index(
            TopicIndexOptions opts,
            PagerOptions pager)
        {
            
            // default options
            if (opts == null)
            {
                opts = new TopicIndexOptions();
            }

            // default pager
            if (pager == null)
            {
                pager = new PagerOptions();
            }
            
            // Build breadcrumb
            _breadCrumbManager.Configure(builder =>
            {
                builder.Add(S["Home"], home => home
                    .Action("Index", "Home", "Plato.Core")
                    .LocalNav()
                ).Add(S["Discuss"]);
              
            });
            
            await CreateSampleData();

            // Get default options
            var defaultViewOptions = new TopicIndexOptions();
            var defaultPagerOptions = new PagerOptions();

            // Add non default route data for pagination purposes
            if (opts.Search != defaultViewOptions.Search)
                this.RouteData.Values.Add("opts.search", opts.Search);
            if (opts.Sort != defaultViewOptions.Sort)
                this.RouteData.Values.Add("opts.sort", opts.Sort);
            if (opts.Order != defaultViewOptions.Order)
                this.RouteData.Values.Add("opts.order", opts.Order);
            if (opts.Filter != defaultViewOptions.Filter)
                this.RouteData.Values.Add("opts.filter", opts.Filter);
            if (pager.Page != defaultPagerOptions.Page)
                this.RouteData.Values.Add("pager.page", pager.Page);
            if (pager.PageSize != defaultPagerOptions.PageSize)
                this.RouteData.Values.Add("pager.size", pager.PageSize);
            
            // Add view options to context for use within view adaptors
            this.HttpContext.Items[typeof(TopicIndexViewModel)] = new TopicIndexViewModel()
            {
                Options = opts,
                Pager = pager
            };

            // Build view
            var result = await _topicViewProvider.ProvideIndexAsync(new Topic(), this);

            // Return view
            return View(result);
            
        }

        public Task<IActionResult> Popular(
            TopicIndexOptions opts,
            PagerOptions pager)
        {

            // default options
            if (opts == null)
            {
                opts = new TopicIndexOptions();
            }

            // default pager
            if (pager == null)
            {
                pager = new PagerOptions();
            }

            opts.Sort = SortBy.Replies;
            opts.Order = OrderBy.Desc;

            return Index(opts, pager);
        }


        public Task<IActionResult> Get(
          TopicIndexOptions opts,
          PagerOptions pager)
        {

            // default options
            if (opts == null)
            {
                opts = new TopicIndexOptions();
            }

            // default pager
            if (pager == null)
            {
                pager = new PagerOptions();
            }

            // Get default options
            var defaultViewOptions = new TopicIndexOptions();
            var defaultPagerOptions = new PagerOptions();

            // Add non default route data for pagination purposes
            if (opts.Search != defaultViewOptions.Search)
                this.RouteData.Values.Add("opts.search", opts.Search);
            if (opts.Sort != defaultViewOptions.Sort)
                this.RouteData.Values.Add("opts.sort", opts.Sort);
            if (opts.Order != defaultViewOptions.Order)
                this.RouteData.Values.Add("opts.order", opts.Order);
            if (opts.Filter != defaultViewOptions.Filter)
                this.RouteData.Values.Add("opts.filter", opts.Filter);
            if (pager.Page != defaultPagerOptions.Page)
                this.RouteData.Values.Add("pager.page", pager.Page);
            if (pager.PageSize != defaultPagerOptions.PageSize)
                this.RouteData.Values.Add("pager.size", pager.PageSize);
            
            // Build view model
            var viewModel = new TopicIndexViewModel()
            {
                Options = opts,
                Pager = pager
            };
            
            // Add view options to context for use within view adaptors
            this.HttpContext.Items[typeof(TopicIndexViewModel)] = viewModel;
            
            // Return view
            return Task.FromResult((IActionResult) View(viewModel));

        }


        // -----------------
        // add new topic
        // -----------------

        public async Task<IActionResult> Create(int channel)
        {
            var topic = new Topic();
            if (channel > 0)
            {
                topic.CategoryId = channel;
            }

            // Build breadcrumb
            _breadCrumbManager.Configure(builder =>
            {
                builder.Add(S["Home"], home => home
                    .Action("Index", "Home", "Plato.Core")
                    .LocalNav()
                ).Add(S["Discuss"], discuss => discuss
                    .Action("Index", "Home", "Plato.Discuss")
                    .LocalNav()
                ).Add(S["Post Topic"], post => post
                    .LocalNav()
                );
            });

            var result = await _topicViewProvider.ProvideEditAsync(topic, this);

            // Return view
            return View(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Create))]
        public async Task<IActionResult> CreatePost(EditTopicViewModel model)
        {
            
            // Validate model state within all view providers
            if (await _topicViewProvider.IsModelStateValid(new Topic()
            {
                Title = model.Title,
                Message = model.Message
            }, this))
            {

                // Get fully composed type from all involved view providers
                var topic = await _topicViewProvider.GetComposedType(this);

                // We need to first add the fully composed type
                // so we have a nuique entity Id for all ProvideUpdateAsync
                // methods within any involved view provider
                var newTopic = await _topicManager.CreateAsync(topic);

                // Ensure the insert was successful
                if (newTopic.Succeeded)
                {

                    // Execute view providers ProvideUpdateAsync method
                    await _topicViewProvider.ProvideUpdateAsync(newTopic.Response, this);

                    // Everything was OK
                    _alerter.Success(T["Topic Created Successfully!"]);

                    // Redirect to topic
                    return RedirectToAction(nameof(Topic), new {Id = newTopic.Response.Id});
                    
                }
                else
                {
                    // Errors that may have occurred whilst creating the entity
                    foreach (var error in newTopic.Errors)
                    {
                        ViewData.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            
            // if we reach this point some view model validation
            // failed within a view provider, display model state errors
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _alerter.Danger(T[error.ErrorMessage]);
                }
            }

            return await Create(0);
            
        }

        // display topic

        public async Task<IActionResult> Topic(
            int id,
            TopicIndexOptions topicIndexOptions,
            PagerOptions pagerOptions)
        {

            var topic = await _entityStore.GetByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            
            // default options
            if (topicIndexOptions == null)
            {
                topicIndexOptions = new TopicIndexOptions();
            }

            // default pager
            if (pagerOptions == null)
            {
                pagerOptions = new PagerOptions();
            }

            // Build breadcrumb
            _breadCrumbManager.Configure(builder =>
            {
                builder.Add(S["Home"], home => home
                    .Action("Index", "Home", "Plato.Core")
                    .LocalNav()
                ).Add(S["Discuss"], discuss => discuss
                    .Action("Index", "Home", "Plato.Discuss")
                    .LocalNav()
                ).Add(S[topic.Title], post => post
                    .LocalNav()
                );
            });

        
            // Maintain previous route data when generating page links
            var routeData = new RouteData();
            routeData.Values.Add("search", topicIndexOptions.Search);
            routeData.Values.Add("sort", topicIndexOptions.Sort);
            routeData.Values.Add("page", pagerOptions.Page);
            
            // Build view
            var result = await _topicViewProvider.ProvideDisplayAsync(topic, this);

            // Return view
            return View(result);
            
        }
        
        // reply to topic

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Topic))]
        public async Task<IActionResult> TopicPost(EditReplyViewModel model)
        {
            // We always need an entity to reply to
            var topic = await _entityStore.GetByIdAsync(model.EntityId);
            if (topic == null)
            {
                return NotFound();
            }
            
            // Validate model state within all view providers
            if (await _replyViewProvider.IsModelStateValid(new Reply()
            {
                Id = model.EntityId,
                Message = model.Message
            }, this))
            {

                // We need to first add the reply so we have a nuique Id
                // for all ProvideUpdateAsync methods within any involved view provider
                var reply = await _replyManager.CreateAsync(new Reply()
                {
                    EntityId = model.EntityId,
                    Message = model.Message
                });

                // Ensure the insert was successful
                if (reply.Succeeded)
                {

                    // Execute view providers ProvideUpdateAsync method
                    await _replyViewProvider.ProvideUpdateAsync(reply.Response, this);

                    // Everything was OK
                    _alerter.Success(T["Reply Added Successfully!"]);

                    // Redirect to topic
                    return RedirectToAction(nameof(Topic), new
                    {
                        Id = topic.Id,
                        Alias = topic.Alias
                    });

                }
                else
                {
                    // Errors that may have occurred whilst creating the entity
                    foreach (var error in reply.Errors)
                    {
                        ViewData.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                
            }

            // if we reach this point some view model validation
            // failed within a view provider, display model state errors
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _alerter.Danger(T[error.ErrorMessage]);
                }
            }

            return await Topic(topic.Id, null, null);
            
        }

        // edit topic

        public async Task<IActionResult> Edit(int id)
        {

            var topic = await _entityStore.GetByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            var result = await _topicViewProvider.ProvideEditAsync(topic, this);

            // Return view
            return View(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(EditTopicViewModel model)
        {
            // Get entity we are editing 
            var topic = await _entityStore.GetByIdAsync(model.Id);
            if (topic == null)
            {
                return NotFound();
            }

            // Validate model state within all view providers
            if (await _topicViewProvider.IsModelStateValid(new Topic()
            {
                Title = model.Title,
                Message = model.Message
            }, this))
            {

                // Update title & message
                topic.Title = model.Title;
                topic.Message = model.Message;

                // Execute view providers ProvideUpdateAsync method
                await _topicViewProvider.ProvideUpdateAsync(topic, this);

                // Everything was OK
                _alerter.Success(T["Topic Updated Successfully!"]);

                // Redirect to topic
                return RedirectToAction(nameof(Topic), new
                {
                    Id = topic.Id,
                    Alias = topic.Alias
                });

            }

            // if we reach this point some view model validation
            // failed within a view provider, display model state errors
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _alerter.Danger(T[error.ErrorMessage]);
                }
            }

            return await Create(0);

        }

        // edit reply

        public async Task<IActionResult> EditReply(int id)
        {

            var reply = await _entityReplyStore.GetByIdAsync(id);
            if (reply == null)
            {
                return NotFound();
            }

            var result = await _replyViewProvider.ProvideEditAsync(reply, this);

            // Return view
            return View(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(EditReply))]
        public async Task<IActionResult> EditReplyPost(EditReplyViewModel model)
        {

            // Ensure the reply exists
            var reply = await _entityReplyStore.GetByIdAsync(model.Id);
            if (reply == null)
            {
                return NotFound();
            }

            // Ensure the entity exists
            var topic = await _entityStore.GetByIdAsync(reply.EntityId);
            if (topic == null)
            {
                return NotFound();
            }

            // Update the message
            reply.Message = model.Message;
       
            // Validate model state within all view providers
            if (await _replyViewProvider.IsModelStateValid(reply, this))
            {

                // Execute view providers ProvideUpdateAsync method
                await _replyViewProvider.ProvideUpdateAsync(reply, this);

                // Everything was OK
                _alerter.Success(T["Reply Updated Successfully!"]);

                // Redirect to topic
                return RedirectToAction(nameof(Topic), new
                {
                    Id = topic.Id,
                    Alias = topic.Alias
                });
                
            }

            // if we reach this point some view model validation
            // failed within a view provider, display model state errors
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _alerter.Danger(T[error.ErrorMessage]);
                }
            }

            return await Create(0);

        }
        
        #endregion

        #region "Private Methods"

        string GetSampleMarkDown(int number)
        {
            return number.ToString() + @"

Hi @admin, 

<script>alert('test')</script>

![Example Image](https://www.catster.com/wp-content/uploads/2017/08/A-fluffy-cat-looking-funny-surprised-or-concerned.jpg ""Image Title Here"")

This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. This is a sample paragraph. 

# header 1

```
<script>alert(""test"")</script>
```

![Video1](https://www.youtube.com/watch?v=7lhlKF6MECs)

test

```
<script>alert(""test"")</script>
```

![image.png](/media/2)

- This is a test `< script > alert(""test"") </ script >`
- This is a test `<script> alert(""test"") </script>`

### Check List 

- [x] check list 1 
- [x] check list 2
- [ ] check list 3

Test message Test message Test message Test :)

## Header 2 @newuser1

message Test message Test message Test message :(

Test message Test message Test message Test 

    var entity = await _entityStore.GetByIdAsync(entityId);
    var replies = await GetEntityReplies(entityId, filterOptions, pagerOptions);
        return new HomeTopicViewModel(
            entity,
            replies,
            filterOptions,
            pagerOptions);

### Header 3

＜script
＞alert('XSS')＜/
script＞

'＜script lang＞alert('XSS')＜/ script＞'

<code test=""23"">qweqweqweqweqweqwewqeqwe</code>

test
```<&#115;cript>alert(""test"")</script>```

```
<script>alert(""test"")</script>
```

-This is a test `< &#115;cript > alert(""test"") </ script >`
-This is a test `<script> alert(""test"") </script>`

```

<script>alert(""test"")</script>

```

``

<script>alert(""test"")</script>

<code>123</code>

<code> <&#115;&#99;&#114;&#105;&#112;&#116;>alert(""test"")<&#115;&#99;&#114;&#105;&#112;&#116;>
</code>

&#115;

<code> <&#115;&#99;&#114;&#105;&#112;&#116;>alert(""test"")<&#115;&#99;&#114;&#105;&#112;&#116;></code>

<code> &#115;&#99;&#114;&#105;&#112;&#116;>alert(""test"")<&#115;&#99;&#114;&#105;&#112;&#116;</code>

<&#115;&#99;&#114;&#105;&#112;&#116;>alert(""test"")<&#115;&#99;&#114;&#105;&#112;&#116;>

'''
＜script lang＞alert('XSS')＜/ script＞
'''

＜script lang＞alert('XSS')＜/ script＞

＜script＞alert('XSS')＜/script＞

<div onmouseover=alert('test') onmouseenter=alert""('test')"">
This is a div
</div>

<iframe src=""https://www.instantasp.co.uk/></iframe>
< iframe src=""https://www.instantasp.co.uk/""></iframe>

<section onload=""alert('test')"" onmouseenter=alert""('test')"">
This is a section
</section>


message Test message Test message Test message 

Test message Test message Test message Test message 

<script>alert('test')</script>
<script>alert('test')</script>

#### Header 4

Test message Test message Test message Test message Test 

- list 1
- list 2
- list 3

message Test message  " + number.ToString();

        }

        async Task CreateSampleData()
        { 

            var users =   await _ploatUserStore.QueryAsync()
                .Take(1, 1000)
                .Select<UserQueryParams>(q =>
                {
              
                })
                .OrderBy("LastLoginDate", OrderBy.Desc)
                .ToList();

            var rnd = new Random();
            var topic = new Topic()
            {
                Title = "Test Topic " + rnd.Next(0, users.Total).ToString(),
                Message = GetSampleMarkDown(rnd.Next(0, users.Total)),
                CreatedUserId = users.Data[rnd.Next(0, users.Total)].Id
                
            };
            
            // create topic
            var data = await _topicManager.CreateAsync(topic);
            if (data.Succeeded)
            {
                
                for (var i = 0; i < 10; i++)
                {
                    rnd = new Random();
                    var reply = new Reply()
                    {
                        EntityId = data.Response.Id,
                        Message = GetSampleMarkDown(rnd.Next(1, 6)),
                        CreatedUserId = users.Data[rnd.Next(0, users.Total)].Id
                    };
                    var newReply = await _replyManager.CreateAsync(reply);
                }
               
            
                
            }

        }

        #endregion

    }
    
}
