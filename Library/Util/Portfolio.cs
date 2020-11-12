using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryNet2020.Util
{
    public interface StockService
    {
        decimal GetStockValue(string symbol);
    }

    public class Portfolio
    {
        public bool IsEmpty { get; private set; } = true;
        public StockService MyStockService { get; set; }
        public int SymbolCount
        {
            get
            {
                return Symbols.Count;
            }
        }
        private Dictionary<string, int> Symbols { get; set; } = new Dictionary<string, int>();
        public decimal Value
        {
            get
            {
                if (!Symbols.Any()) return 0;
                var stockValues = Symbols.Keys.Select((symbol) => GetSharesOfSymbol(symbol) * MyStockService.GetStockValue(symbol));
                return stockValues.Sum();
            }
        }

        public int GetSharesOfSymbol(string symbol)
        {
            return Symbols.ContainsKey(symbol) ? Symbols[symbol] : 0;
        }

        public void Trade(string symbol, int count = 1)
        {
            IsEmpty = false;

            ValidateTrade(symbol, count);

            if (Symbols.ContainsKey(symbol))
            {               
                Symbols[symbol] += count;
            }
            else
            {
                Symbols.Add(symbol, count);
            }
        }

        public void Sell(string symbol)
        {
            throw new Exception("Not enough shares of symbol");
        }

        private void ValidateTrade(string symbol, int count = 1)
        {
            if (!Symbols.ContainsKey(symbol) && count > 0)
                return;

            if (Symbols[symbol] + count < 0)
                throw new Exception("Insufficient shares");

        }
    }
}

/*
Is it empty?
    Portfolio is empty
    Portfolio not empty after purchase

How many unique symbols?
    Has 0 symbols before purchase
    Has 1 symbol after purchase
    Has n symbols after n purchases for a given symbol
    Count updates after purchase for a given symbol

Given a symbol and # of shares, make a purchase
    Making purchase increases symbol count
    Making purchase increases share count

How many shares of a given symbol?
    Has 0 shares if not purchased
    Has 1 share after purchase of a given symbol
    Count unchanged after purchase of different symbol

Given a symbol and a # of shares, sell the shares
    Throws exception if selling share that has not been purchased
    Throws exception if selling greater than amount purchased
    Share count decreases after partial sale
    Share removed from collection after complete sale
*/
