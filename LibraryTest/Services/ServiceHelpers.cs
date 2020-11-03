using System;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;

namespace LibraryTest.Services
{
    public class ServiceHelpers
    {
        public static Holding SaveCheckedOutHoldingWithClassification(LibraryContext context, string classification)
        {
            var holding = new Holding {Classification = classification};
            holding.CheckOut(DateTime.Now, 1, CheckoutPolicies.BookCheckoutPolicy);
            context.Holdings.Add(holding);
            context.SaveChanges();
            return holding;
        }
        
        public static Patron SavePatronWithId(LibraryContext context, int id)
        {
            var patron = new Patron {Id = id, Name = ""};
            context.Patrons.Add(patron);
            context.SaveChanges();
            return patron;
        }

        public static Holding SaveCheckedInHoldingWithClassification(LibraryContext context, string classification)
        {
            var holding = new Holding {Classification = classification};
            holding.CheckIn(DateTime.Now, 1);
            context.Holdings.Add(holding);
            context.SaveChanges();
            return holding;
        }
    }
}