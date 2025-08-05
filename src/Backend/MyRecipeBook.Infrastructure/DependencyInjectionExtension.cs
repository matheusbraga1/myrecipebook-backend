using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddRepositories(services);
    }

    private static void AddDbContext(IServiceCollection services)
    {
        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseSqlServer("YourConnectionString");
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }
}
