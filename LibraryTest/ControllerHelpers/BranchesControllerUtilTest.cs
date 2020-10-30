using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.ControllerHelpers
{
    [Collection("SharedLibraryContext")]
    public class BranchesControllerUtilTest
    {
        private LibraryContext context;
        
        public BranchesControllerUtilTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }
        
        [Fact]
        public void BranchNameForCheckedOutBranch()
        {
            Assert.Equal(BranchesControllerUtil.CheckedOutBranchName, 
                BranchesControllerUtil.BranchName(context, Branch.CheckedOutId));
        }

        [Fact]
        public void BranchNameForBranch()
        {
            context.Add(new Branch { Id = 2, Name = "NewBranchName" });
            context.SaveChanges();

            var branchName = BranchesControllerUtil.BranchName(context, 2);

            Assert.Equal("NewBranchName", branchName);
        }
    }
}