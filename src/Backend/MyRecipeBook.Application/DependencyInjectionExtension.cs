using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.Services.Mappings;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddUseCases(services);
        AddPasswordEncrypter(services, configuration);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    public static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
    }

    public static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        
        services.AddScoped(option => new PasswordEncrypter(additionalKey!));
    }
}
