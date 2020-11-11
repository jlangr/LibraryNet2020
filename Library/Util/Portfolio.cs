using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class Portfolio
    {
        public bool IsEmpty => Count == 0;

        public int Count => symbols.Count;

        private HashSet<string> symbols = new HashSet<string>();

        public void Purchase(string symbol = "")
        {
            symbols.Add(symbol);            
        }
    }
}
