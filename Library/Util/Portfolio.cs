namespace LibraryNet2020.Util {
    public class Portfolio {
        public bool IsEmpty { get; private set; } = true;
        public int Count { get { IsEmpty ? 1 : 0 }}
        public void Purchase() {
            IsEmpty = false;
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