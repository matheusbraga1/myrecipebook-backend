using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly PasswordEncrypter _passwordEncrypter;

    public DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEncrypter passwordEncrypter)
    {
        _repository = repository;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncrypter.EncryptPassword(request.Password);

        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }
}
