using System.Collections.Generic;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Author;

namespace NovelWorld.MasterData.Domain.Queries.Author
{
    public class GetAllAuthorRequest: IQuery<List<AuthorGeneralResponse>>, IRequest
    {
    }
    
    public class GetAllAuthorRequestValidator: AbstractValidator<GetAllAuthorRequest>
    {
        public GetAllAuthorRequestValidator()
        {
        }
    }
}