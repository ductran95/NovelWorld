using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.Identity.Data.Entities
{
    public class User: Entity
    {
        public string Account { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DoB { get; set; }
        public string Password { get; set; }
    }
}