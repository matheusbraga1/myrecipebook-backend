using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestStringGenerator
{
    public static string Paragraph(int minCharacters)
    {
        var faker = new Faker();

        var longText = faker.Lorem.Paragraphs(count: 7);

        while (longText.Length < minCharacters)
        {
            longText = $"{longText} {faker.Lorem.Paragraphs()}";
        }

        return longText;
    }
}
