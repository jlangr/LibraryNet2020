using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class Portfolio
    {
        public bool IsEmpty => Count == 0;

        public int Count { get; set; } = 0;

        public void Purchase()
        {
            Count = 1;
        }
    }
}
