namespace MyRecipeBook.Exceptions.ExceptionBase;

public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException() : base(ResourceMessageException.EMAIL_OR_PASSWORD_INVALID)
    {
    }
}
