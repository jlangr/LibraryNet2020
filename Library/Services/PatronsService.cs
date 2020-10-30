using System.Collections.Generic;
using System.Linq;
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
        
        public IEnumerable<Holding> HoldingsForPatron(int id)
        {
            return new List<Holding>(context.Holdings.Where(holding => holding.HeldByPatronId == id));
        }
    }
}