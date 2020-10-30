using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.ControllerHelpers
{
    [Collection("SharedLibraryContext")]
    public class BranchesServiceTest
    {
        private LibraryContext context;
        private BranchesService branchesService;
        
        public BranchesServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            branchesService = new BranchesService(context);
        }
        
        [Fact]
        public void BranchNameForCheckedOutBranch()
        {
            Assert.Equal(BranchesService.CheckedOutBranchName, 
                branchesService.BranchName(Branch.CheckedOutId));
        }

        [Fact]
        public void BranchNameForBranch()
        {
            context.Add(new Branch { Id = 2, Name = "NewBranchName" });
            context.SaveChanges();

            var branchName = branchesService.BranchName(2);

            Assert.Equal("NewBranchName", branchName);
        }

        [Fact]
        public void BranchNameIsNullWhenBranchNotFound()
        {
            var branchName = branchesService.BranchName(2);

            Assert.Equal("", branchName);
        }
    }
}