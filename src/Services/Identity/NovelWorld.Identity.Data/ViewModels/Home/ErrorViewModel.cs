// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using NovelWorld.Data.DTO;

namespace NovelWorld.Identity.Data.ViewModels.Home
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string error)
        {
            Error = error;
        }

        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string RequestId { get; set; }
    }
}