using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Book
{
    public class UploadBookCoverRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
        public IFormFile Cover { get; set; }
    }
    
    public class UploadBookCoverRequestValidator: AbstractValidator<UploadBookCoverRequest>
    {
        public UploadBookCoverRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Cover).NotNull();
        }
    }
}