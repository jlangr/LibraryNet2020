using System;
using System.Linq;
using LibraryNet2020.Models;

namespace LibraryNet2020.ControllerHelpers
{
    public class BranchesService
    {
        private LibraryContext context;

        public BranchesService(LibraryContext context)
        {
            this.context = context;
        }

        public const string CheckedOutBranchName = "** checked out **";

        public string BranchName(int branchId)
        {
            if (branchId == Branch.CheckedOutId) return CheckedOutBranchName;
            var branch = context.Branches.SingleOrDefault(branch => branch.Id == branchId);
            return branch == null ? "" : branch.Name;
        }
    }
}