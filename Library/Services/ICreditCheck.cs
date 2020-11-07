namespace LibraryNet2020.Services
{
    public interface ICreditCheck
    {
        const int GoodCreditThreshold = 630;
        int CreditScore(string ssn);
    }
}