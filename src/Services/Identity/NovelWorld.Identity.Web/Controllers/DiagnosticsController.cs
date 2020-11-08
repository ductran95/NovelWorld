// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NovelWorld.API.Attributes;
using NovelWorld.API.Extensions;
using NovelWorld.Identity.Data.ViewModels.Diagnostics;

namespace NovelWorld.Identity.Web.Controllers
{
    [FallbackView("/Home/Index")]
    [SecurityHeaders]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DiagnosticsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.IsFromLocal())
            {
                return NotFound();
            }

            var authenticateResult = await HttpContext.AuthenticateAsync();
            var model = new DiagnosticsViewModel(authenticateResult.Properties.Items, authenticateResult.Principal.Claims);
            return View(model);
        }
    }
}