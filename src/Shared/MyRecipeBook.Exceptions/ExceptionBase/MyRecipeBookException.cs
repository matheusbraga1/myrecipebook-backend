namespace MyRecipeBook.Exceptions.ExceptionBase;

public class MyRecipeBookException : SystemException
{
    public MyRecipeBookException(string message) : base(message)
    {
    }
}
