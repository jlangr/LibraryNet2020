using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryNet2020.Models;

namespace LibraryNet2020.ViewModels
{
    [NotMapped]
    public class HoldingViewModel: Holding
    {
        public HoldingViewModel() { }
        public HoldingViewModel(Holding holding)
        {
            Id = holding.Id;
            BranchId = holding.BranchId;
            CheckoutPolicy = holding.CheckoutPolicy;
            CheckOutTimestamp = holding.CheckOutTimestamp;
            Classification = holding.Classification;
            CopyNumber = holding.CopyNumber;
            DueDate = holding.DueDate;
            HeldByPatronId = holding.HeldByPatronId;
        }

        [NotMapped, DisplayName("Branch")]
        public string BranchName { get; set; }
    }
}