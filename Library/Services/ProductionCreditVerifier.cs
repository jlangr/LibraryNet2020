namespace LibraryNet2020.Services
{
    public class ProductionCreditVerifier: CreditVerifier
    {
        public bool Verify(string cardNumber) => true;
    }
}