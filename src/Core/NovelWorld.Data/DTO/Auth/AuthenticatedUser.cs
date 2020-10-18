using System;
using System.Collections.Generic;

namespace NovelWorld.Data.DTO.Auth
{
    public class AuthenticatedUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}