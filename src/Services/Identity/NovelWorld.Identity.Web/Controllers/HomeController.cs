// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
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
using NovelWorld.Authentication.Exceptions;
using NovelWorld.Data.DTO;
using NovelWorld.Identity.Data.ViewModels.Home;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Web.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : MvcController
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(
            IWebHostEnvironment environment,
            IMediator mediator,
            IMapper mapper,
            ILogger<HomeController> logger,
            IAuthContext authContext,
            IIdentityServerInteractionService interaction
            ) : base(environment, mediator, mapper, logger, authContext)
        {
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            if (_environment.IsProduction())
            {
                _logger.LogInformation("Homepage is disabled in production. Returning 404.");
                return NotFound();
            }

            // only show in development
            return View();
        }

        public IActionResult Test()
        {
            throw new UnauthenticatedException();
        }
    }
}