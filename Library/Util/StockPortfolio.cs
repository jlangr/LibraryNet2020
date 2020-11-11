using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class StockPortfolio
    {
        private HashSet<string> symbols { get; set; }

        public StockPortfolio()
        {
            symbols = new HashSet<string>();
        }

        public bool IsEmpty()
        {
            return symbols.Count() == 0;
        }

        public void PurchaseStock(string symbol, int shares = 1)
        {
            symbols.Add(symbol);   
        }

        public int Count()
        {
            return symbols.Count();
        }

        public int Shares(string v)
        {
            throw new NotImplementedException();
        }
    }
}
