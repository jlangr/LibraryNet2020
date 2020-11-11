using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class StockPortfolio
    {
        private Dictionary<string,int> stocks { get; set; }

        public StockPortfolio()
        {
            stocks = new Dictionary<string, int>();
        }

        public bool IsEmpty()
        {
            return stocks.Count() == 0;
        }

        public void PurchaseStock(string symbol, int shares = 1)
        {
            if (stocks.ContainsKey(symbol))
            {

            }
            stocks.Add(symbol,shares); 
            
        }

        public int Count()
        {
            return stocks.Count();
        }

        public int Shares(string symbol)
        {
            return stocks[symbol];
        }
    }
}
