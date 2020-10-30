using System.Collections.Generic;
using LibraryNet2020.Models;

namespace LibraryNet2020.ViewModels
{
    public class CheckInViewModel
    {
        public string Barcode { get; set; }
        public List<Branch> BranchesViewList { get; internal set; }
        public int BranchId { get; set; }
    }
}