// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Attributes;
using NovelWorld.API.Controllers;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Identity.Data.ViewModels.Diagnostics;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Web.Controllers
{
    [SecurityHeaders]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DiagnosticsController : MvcController
    {
        public DiagnosticsController(
            IWebHostEnvironment environment,
            IMediator mediator,
            IMapper mapper,
            ILogger<DiagnosticsController> logger,
            IAuthContext authContext
            ) : base(environment, mediator, mapper, logger, authContext)
        {
        }
        public async Task<IActionResult> Index()
        {
            if (_environment.IsProduction())
            {
                _logger.LogInformation("Diagnostics is disabled in production. Returning 404.");
                return NotFound();
            }

            var authenticateResult = await HttpContext.AuthenticateAsync();
            var model = new DiagnosticsViewModel(authenticateResult.Properties.Items, authenticateResult.Principal.Claims);
            return View(model);
        }
    }
}