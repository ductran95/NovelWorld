using System.Collections.Generic;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Book
{
    public class GetAllBookRequest: IQuery<List<BookGeneralResponse>>, IRequest
    {
    }
    
    public class GetAllBookRequestValidator: AbstractValidator<GetAllBookRequest>
    {
        public GetAllBookRequestValidator()
        {
        }
    }
}