using CommonTestUtilities.Requests;
using System.Globalization;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : MyRecipeBookClassFixture
{
    private const string method = "user";

    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(method: method, request: request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var name = responseData.RootElement.GetProperty("name").GetString();
        var token = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();

        name.ShouldNotBeNullOrWhiteSpace();
        name.ShouldBe(request.Name);
        token.ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(method: method, request: request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessageException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
    }
}
