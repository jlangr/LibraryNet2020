using System.Linq;
using LibraryNet2020.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryNet2020.ControllerHelpers
{
    public class BranchesControllerUtil
    {
        public const string CheckedOutBranchName = "** checked out **";

        public static string BranchName(DbSet<Branch> branchRepo, int branchId)
        {
            return branchId == Branch.CheckedOutId
                ? CheckedOutBranchName
                : branchRepo.Single(branchId => branchId == branchId).Name;
        }
    }
}