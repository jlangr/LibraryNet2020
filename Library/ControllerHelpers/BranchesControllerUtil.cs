using System.Linq;
using LibraryNet2020.Models;

namespace LibraryNet2020.ControllerHelpers
{
    public static class BranchesControllerUtil
    {
        public const string CheckedOutBranchName = "** checked out **";

        // TODO move to somewhere better, e.g. BranchService
        public static string BranchName(LibraryContext context, int branchId)
        {
            return branchId == Branch.CheckedOutId
                ? CheckedOutBranchName
                : context.Branches.Single(branch => branch.Id == branchId).Name;
        }
    }
}