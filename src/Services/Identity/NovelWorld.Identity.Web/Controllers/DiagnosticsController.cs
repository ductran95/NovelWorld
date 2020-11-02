// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NovelWorld.API.Attributes;
using NovelWorld.API.Extensions;
using NovelWorld.Identity.Web.Models.Diagnostics;

namespace NovelWorld.Identity.Web.Controllers
{
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

            var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            return View(model);
        }
    }
}