using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;

namespace MyRecipeBook.API.Controllers;

[AuthenticatedUser]
public class RecipeController : MyRecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof())]
    public async Task<IActionResult> Register()
    {

    }
}
