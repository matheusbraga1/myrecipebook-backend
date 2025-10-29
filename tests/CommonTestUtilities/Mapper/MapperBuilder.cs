using AutoMapper;
using CommonTestUtilities.IdEncryption;
using MyRecipeBook.Application.Services.Mappings;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncrypter = IdEncrypterBuilder.Build();

        return new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapping(idEncrypter));
        }).CreateMapper();
    }
}
