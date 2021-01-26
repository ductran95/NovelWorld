// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using NovelWorld.Data.DTO;

namespace NovelWorld.Identity.Data.ViewModels.Home
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            Errors = new List<Error>();
        }

        public ErrorViewModel(List<Error> errors)
        {
            Errors = errors;
        }

        public List<Error> Errors { get; set; }
        public string RequestId { get; set; }
    }
}