using NovelWorld.Domain.Commands;

namespace NovelWorld.Identity.Domain.Commands.User
{
    public class ValidateUserCredentialCommand: Command<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}