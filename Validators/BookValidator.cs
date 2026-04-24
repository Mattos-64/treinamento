using FluentValidation;
using Library.Models;

namespace Library.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("O título do livro não pode ser vazio !! ")
                .MinimumLength(3).WithMessage("O título deve conter, no mínimo, 3 caracteres !! ");

                RuleFor(b => b.Isbn)
                .NotEmpty().WithMessage("O ISBN é obritatório !! ")
                .Length(13).WithMessage("O ISBN deter conter, no mínimo, 13 digitos !! ");
        }
    }
}