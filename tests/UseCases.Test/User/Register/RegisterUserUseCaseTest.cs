using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using Shouldly;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Build();

        return new RegisterUserUseCase(readRepository, writeRepository, unitOfWork, mapper, passwordEncrypter);
    }
}
