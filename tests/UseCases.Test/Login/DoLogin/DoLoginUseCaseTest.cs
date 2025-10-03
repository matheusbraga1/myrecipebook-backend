using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    private async Task Success()
    {
        var useCase = CreateUseCase();
        var result = await useCase.Execute(RequestLoginJsonBuilder.Build());
    }

    [Fact]
    private async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await Should.ThrowAsync<InvalidLoginException>(act))
            .Equals(ResourceMessageException.EMAIL_OR_PASSWORD_INVALID);
    }

    private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncrypterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter);
    }
}
