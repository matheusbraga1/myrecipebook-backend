using AutoMapper;
using MyRecipeBook.Application.Services.Mappings;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
    {
        Validate(request);

        var autoMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapping>();
        }).CreateMapper();

        var user = new Domain.Entities.User
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password
        };

        return new ResponseRegisteredUserJson
        {
            Name = request.Name
        };
    }

    private void Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMassage = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMassage);
        }
    }
}
