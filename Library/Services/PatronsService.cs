using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.Extensions;
using LibraryNet2020.Models;

namespace LibraryNet2020.Services
{
    public class PatronsService
    {
        private LibraryContext context;

        public PatronsService(LibraryContext context)
        {
            this.context = context;
        }

        public CreditVerifier CreditVerifier { get; set; } = new ProductionCreditVerifier();

        public IEnumerable<Holding> HoldingsForPatron(int id)
        {
            return new List<Holding>(context.Holdings.Where(holding => holding.HeldByPatronId == id));
        }

        public int Create(Patron patron)
        {
            if (!IsCreditGood(patron)) return -1;
            var persisted = context.Patrons.Add(patron);
            context.SaveChanges();
            return persisted.Entity.Id;
        }

        private bool IsCreditGood(Patron patron)
        {
            return CreditVerifier.Verify(patron.SSN);
        }

        public Patron FindById(int id)
        {
            return context.Patrons.FindByIdAsync(id).Result;
        }

        public void Update(Patron patron)
        {
            context.Patrons.Update(patron);
            context.SaveChanges();
        }
    }
}