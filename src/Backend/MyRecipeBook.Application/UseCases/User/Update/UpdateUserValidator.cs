using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessageException.NAME_EMPTY);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessageException.EMAIL_EMPTY);

        When(request => !string.IsNullOrWhiteSpace(request.Email), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessageException.EMAIL_INVALID);
        });
    }
}
