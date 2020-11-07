namespace LibraryNet2020.Services
{
    public interface CreditVerifier
    {
        bool Verify(string cardNumber);
    }
}