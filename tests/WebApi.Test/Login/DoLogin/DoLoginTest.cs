using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string _method = "login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(_method, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var name = responseData.RootElement.GetProperty("name").GetString();

        name.ShouldNotBeNullOrWhiteSpace();
        name.ShouldBe(_name);
    }
}
