using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Book
{
    public class DeleteBookRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteBookRequestValidator: AbstractValidator<DeleteBookRequest>
    {
        public DeleteBookRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}