using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessageException.RECIPE_TITLE_EMPTY);
        RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceMessageException.COOKING_TIME_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceMessageException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessageException.AT_LEAST_ONE_INGREDIENT);
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessageException.AT_LEAST_ONE_INSTRUCTION);
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessageException.DISH_TYPE_NOT_SUPPORTED);
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMessageException.INGREDIENT_EMPTY);
        RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
        {
            instructionRule
            .RuleFor(instruction => instruction.Step)
            .GreaterThanOrEqualTo(1)
            .WithMessage(ResourceMessageException.NON_NEGATIVE_OR_EQUALS_ZERO_INSTRUCTION_STEP);

            instructionRule
                .RuleFor(instruction => instruction.Text)
                .NotEmpty()
                .WithMessage(ResourceMessageException.INSTRUCTION_TEXT_EMPTY)
                .MaximumLength(2000)
                .WithMessage(ResourceMessageException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        });
        RuleFor(recipe => recipe.Instructions)
            .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessageException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }
}
