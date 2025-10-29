using Sqids;

namespace CommonTestUtilities.IdEncryption;

public class IdEncrypterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "N4O75AEBPt9rYZjDFHCxVGQL62UMKsiw1038"
        });
    }
}
