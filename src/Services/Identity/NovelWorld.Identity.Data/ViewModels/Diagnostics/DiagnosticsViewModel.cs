// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Newtonsoft.Json;

namespace NovelWorld.Identity.Data.ViewModels.Diagnostics
{
    public class DiagnosticsViewModel
    {
        public DiagnosticsViewModel(IDictionary<string, string> authenticateItems, IEnumerable<Claim> principalClaims)
        {
            AuthenticateItems = authenticateItems;
            PrincipalClaims = principalClaims;
            
            if (authenticateItems.ContainsKey("client_list"))
            {
                var encoded = authenticateItems["client_list"];
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);

                Clients = JsonConvert.DeserializeObject<string[]>(value);
            }
        }
        
        public IEnumerable<Claim> PrincipalClaims { get; }

        public IDictionary<string, string> AuthenticateItems { get; }
        public IEnumerable<string> Clients { get; } = new List<string>();
    }
}