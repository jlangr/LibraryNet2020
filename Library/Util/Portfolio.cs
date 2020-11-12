using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LibraryNet2020.Util
{
    public class Portfolio
    {
        public bool IsEmpty => Count == 0;

        public int Count => holdings.Count;

        public decimal GetPortfolioValue()
        {
            if (holdings.Count == 0)
            {
                return 0;
            }
            return StockPriceService.GetPrice(holdings.First().Key) * holdings.First().Value;

        }

        private Dictionary<string, int> holdings = new Dictionary<string, int>();

        public IStockPriceService StockPriceService { get; set; }

        public void Purchase(string symbol = "")
        {
            if (holdings.ContainsKey(symbol))
            {
                holdings[symbol]++;
            }
            else
            {
                holdings.Add(symbol, 1);
            }
        }
    }
    public interface IStockPriceService
    {
        decimal GetPrice(string symbol);
    }
}
