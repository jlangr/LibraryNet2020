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

        public decimal GetPortfolioValue() =>
            holdings.Select(holding =>
                StockPriceService.GetPrice(holding.Key) * holding.Value)
                .Sum();

        private Dictionary<string, int> holdings = new Dictionary<string, int>();

        public IStockPriceService StockPriceService { get; set; }

        public void Purchase(string symbol, int numberOfShares)
        {
            if (holdings.ContainsKey(symbol))
            {
                holdings[symbol] += numberOfShares;
            }
            else
            {
                holdings.Add(symbol, numberOfShares);
            }
        }
    }
    public interface IStockPriceService
    {
        decimal GetPrice(string symbol);
    }
}
