using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryTest.Util
{
    public class Portfolio
    {
        public bool IsEmpty => Size == 0;

        public int Size => Holdings.Keys.Count;
        public decimal Value
        {
            get
            {
                if (IsEmpty) return 0;
                return StockService.CurrentPrice(Holdings.Keys.First());
                
            }
        }

        public int Shares(string symbol) => !Holdings.ContainsKey(symbol) ? 0 : Holdings[symbol];

        public void Purchase(string symbol, int shares)
        {
            ThrowOnNonPositiveShares(shares);
            Holdings[symbol] = Shares(symbol) + shares;
        }

        private void ThrowOnNonPositiveShares(int shares)
        {
            if (shares <= 0) throw new ArgumentException();
        }

        private IDictionary<string, int> Holdings { get; } = new Dictionary<string, int>();

        public StockService StockService { get; set; }

        public void Sell(string symbol, int shares)
        {
            ThrowWhenSellingMoreSharesThanOwned(symbol, shares);
            Holdings[symbol] = Shares(symbol) - shares;
            RemoveSymbolIfAllSold(symbol);
        }

        private void RemoveSymbolIfAllSold(string symbol)
        {
            if (Shares(symbol) == 0) Holdings.Remove(symbol);
        }

        private void ThrowWhenSellingMoreSharesThanOwned(string symbol, int shares)
        {
            if (Shares(symbol) < shares) throw new ArgumentException();
        }


    }
}