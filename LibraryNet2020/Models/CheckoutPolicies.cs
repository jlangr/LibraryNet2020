namespace LibraryNet2020.Models
{
    public static class CheckoutPolicies
    {
        private static readonly CheckoutPolicy Book = new BookCheckoutPolicy();
        private static readonly CheckoutPolicy Movie = new MovieCheckoutPolicy();
        public static CheckoutPolicy BookCheckoutPolicy 
        {
            get
            {
                return Book;
            }
        }
        public static CheckoutPolicy MovieCheckoutPolicy
        {
            get
            {
                return Movie;
            }
        }
    }
}
