﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class StockPortfolio
    {
        public bool isEmpty { get; set; } = true;

        public bool IsEmpty()
        {
            return true;
        }

        public void PurchaseStock()
        {
            isEmpty = false;
        }
    }
}
