using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        user.Password.ShouldBe(passwordEncrypter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            CurrentPassword = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await Should.ThrowAsync<ErrorOnValidationException>(act))
            .ErrorMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.PASSWORD_EMPTY);

        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        user.Password.ShouldBe(passwordEncrypter.Encrypt(password));
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await Should.ThrowAsync<ErrorOnValidationException>(act))
            .ErrorMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);

        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        user.Password.ShouldBe(passwordEncrypter.Encrypt(password));
    }

    [Fact]
    public async Task Error_NewPassword_Equals_CurrentPassword()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            CurrentPassword = password,
            NewPassword = password
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await Should.ThrowAsync<ErrorOnValidationException>(act))
            .ErrorMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.PASSWORD_CURRENT_EQUALS_NEW_PASSWORD);

        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        user.Password.ShouldBe(passwordEncrypter.Encrypt(request.NewPassword));
    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncrypter);
    }
}
