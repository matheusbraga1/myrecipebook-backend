using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using Shouldly;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBeNullOrWhiteSpace();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await Should.ThrowAsync<ErrorOnValidationException>(act))
            .ErrorMessages.ShouldHaveSingleItem()
            .ShouldBe(ResourceMessageException.RECIPE_TITLE_EMPTY);
    }

    private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unitOfWork, mapper);
    }
}
