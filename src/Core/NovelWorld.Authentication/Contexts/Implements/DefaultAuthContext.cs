using System.Net;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.DTO;
using NovelWorld.Authentication.Exceptions;

namespace NovelWorld.Authentication.Contexts.Implements
{
    public class DefaultAuthContext: IAuthContext
    {
        private AuthenticatedUser _user;
        private readonly IPAddress _ip;
        
        public DefaultAuthContext()
        {
            
        }
        
        public AuthenticatedUser User
        {
            get
            {
                if (_user == null)
                {
                    throw new UnauthenticatedException();
                }

                return _user;
            }
        }

        public IPAddress IP => _ip;
    }
}