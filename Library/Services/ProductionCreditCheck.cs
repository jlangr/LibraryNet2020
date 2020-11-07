namespace LibraryNet2020.Services
{
    public class ProductionCreditCheck: ICreditCheck
    {
        public int CreditScore(string ssn)
        {
            return 800;
        }
    }
}