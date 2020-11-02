using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.ControllerHelpers
{
    [Collection("SharedLibraryContext")]
    public class BranchesServiceTest
    {
        private readonly LibraryContext context;
        private readonly BranchesService branchesService;
        
        public BranchesServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            branchesService = new BranchesService(context);
        }
        
        [Fact]
        public void BranchNameForCheckedOutBranch()
        {
            Assert.Equal(Branch.CheckedOutBranch.Name,
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

        [Fact]
        public void RetrievesAllBranchesWithCheckedOutVirtualBranch()
        {
            context.Add(new Branch { Id = 2, Name = "A Branch" });
            context.SaveChanges();
            
            var all = branchesService.AllBranchesIncludingVirtual();
            
            Assert.Equal(new List<string>{ Branch.CheckedOutBranch.Name, "A Branch" },
                all.Select(b => b.Name));
        }
    }
}