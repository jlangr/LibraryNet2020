using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNet2020.Util
{
    public class Portfolio
    {
        public bool IsEmpty { get; set; } = true;

        public int Count => 0;

        public void Purchase()
        {
            IsEmpty = false;
        }
    }
}
