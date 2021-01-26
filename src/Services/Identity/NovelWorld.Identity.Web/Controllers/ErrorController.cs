// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Attributes;
using NovelWorld.API.Controllers;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.DTO;
using NovelWorld.Identity.Data.ViewModels.Home;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Web.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ErrorController : MvcController
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ErrorController(
            IWebHostEnvironment environment,
            IMediator mediator,
            IMapper mapper,
            ILogger<ErrorController> logger,
            IAuthContext authContext,
            IIdentityServerInteractionService interaction
            ) : base(environment, mediator, mapper, logger, authContext)
        {
            _interaction = interaction;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Index(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from Identity Server
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Errors.Add(new Error(message.Error, message.ErrorDescription));
                vm.RequestId = message.RequestId;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }

        public IActionResult Unauthenticated()
        {
            return View("Unauthenticated");
        }
        
        public new IActionResult Unauthorized()
        {
            return View("Unauthorized");
        }
        
        public new IActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}