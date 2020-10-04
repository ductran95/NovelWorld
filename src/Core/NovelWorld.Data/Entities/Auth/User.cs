using System;

namespace NovelWorld.Data.Entities.Auth
{
    public class User: Entity
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DoB { get; set; }
    }
}