using System;
using System.Collections.Generic;

namespace NovelWorld.Authentication.DTO
{
    public class AuthenticatedUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        /// <summary>
        /// User's email, equal to OIDC SubjectId
        /// </summary>
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}