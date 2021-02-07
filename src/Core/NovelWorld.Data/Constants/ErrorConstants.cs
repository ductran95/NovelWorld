namespace NovelWorld.Data.Constants
{
    public static class CommonErrorCodes
    {
        public const string Unauthenticated = "Unauthenticated";
        public const string Unauthorized = "Unauthorized";
        public const string InternalServerError = "InternalServerError";
        public const string BadRequest = "BadRequest";
        public const string NotFound = "NotFound";
        public const string ApiClientError = "ApiClientError";
    }
    
    public static class CommonErrorMessages
    {
        public const string QueryDataNotMatchBody = "Query Data is not equivalent to Request Body";
        public const string Unauthenticated = "You are not logged in";
        public const string Unauthorized = "You don't have permision to access {0}";
        public const string FieldNotFound = "Field Not Found";
        public const string DataNotFound = "Data Not Found {0}";
    }
}