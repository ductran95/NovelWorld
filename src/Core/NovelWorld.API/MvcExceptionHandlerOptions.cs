namespace NovelWorld.API
{
    public class MvcExceptionHandlerOptions
    {
        public string ErrorView { get; set; } = "/Views/Shared/Error.cshtml";
        public string ErrorUrl { get; set; } = "/Error";
        public string UnauthenticatedUrl { get; set; } = "/Error/Unauthenticated";
        public string UnauthorizedUrl { get; set; } = "/Error/Unauthorized";
        public string NotFoundUrl { get; set; } = "/Error/NotFound";
    }
}