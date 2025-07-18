using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
    {
        Validate(request);


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
            var errorMassage = result.Errors.Select(error => error.ErrorMessage);

            throw new Exception();
        }
    }
}
