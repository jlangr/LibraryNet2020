namespace LibraryNet2020.NonPersistentModels
{
    public static class CheckoutPolicies
    {
        private static readonly CheckoutPolicy Book = new BookCheckoutPolicy();
        private static readonly CheckoutPolicy Movie = new MovieCheckoutPolicy();
        
        public static CheckoutPolicy BookCheckoutPolicy => Book;
        public static CheckoutPolicy MovieCheckoutPolicy => Movie;
    }
}
