using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Test.Recipe.Register;
public class RegisterRecipeInvalidTokenTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe";

    public RegisterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestRecipeJson();

        var response = await DoPost(method: METHOD, request: request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = new RequestRecipeJson();
        
        var response = await DoPost(method: METHOD, request: request, token: string.Empty);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuild.Build().Generate(Guid.NewGuid());
        
        var request = new RequestRecipeJson();
        
        var response = await DoPost(method: METHOD, request: request, token: token);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
