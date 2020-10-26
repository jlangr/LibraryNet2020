namespace LibraryNet2020.NonPersistentModels
{
    public class CheckoutPolicyFactory
    {
        public static CheckoutPolicy Create(string id)
        {
            return id == "book" 
                ? new BookCheckoutPolicy() 
                : new MovieCheckoutPolicy() as CheckoutPolicy;
       }
    }
}