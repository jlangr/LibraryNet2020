using System;
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
            Console.WriteLine($"branch Id: {branchId}");
            Console.WriteLine($"branch count: {context.Branches.Count()}");
            foreach (var branch in context.Branches)
            {
                Console.WriteLine($"branch {branch.Name} id {branch.Id}");
            }
            return branchId == Branch.CheckedOutId
                ? CheckedOutBranchName
                : context.Branches.SingleOrDefault(branch => branch.Id == branchId).Name;
        }
        
        // TODO test for default case
    }
}