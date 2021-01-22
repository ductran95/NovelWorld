using NovelWorld.Domain.Commands;

namespace NovelWorld.Identity.Domain.Commands.User
{
    public class ValidateUserCredentialCommand: ICommand<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}