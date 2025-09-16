using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email) => _repository.Setup(r => r.ExistActiveUserWithEmail(It.IsAny<string>())).ReturnsAsync(true);

    public IUserReadOnlyRepository Build() => _repository.Object;
}
