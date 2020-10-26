using System;

namespace LibraryNet2020.NonPersistentModels
{
    [Serializable]
    public class BookCheckoutPolicy: CheckoutPolicy
    {
        public const decimal DailyFineBasis = 0.10m;
        public override int MaximumCheckoutDays()
        {
            return 21;
        }
        
        public override string Id => "book";

        public override decimal FineAmount(int daysLate)
        {
            return daysLate * DailyFineBasis;
        }
    }
}
