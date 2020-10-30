using System.Collections.Generic;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    public class HoldingAvailableValidatorTest
    {
        private readonly LibraryContext context = new LibraryContext();
        private readonly HoldingAvailableValidator validator;

        public HoldingAvailableValidatorTest()
        {
            validator = new HoldingAvailableValidator(context);
        }

        [Fact]
        public void IsValidWhenHoldingNotCheckedOut()
        {
            validator.Data = new Dictionary<string, object>
                {{"Holding", new Holding {BranchId = 1}}};

            validator.Validate();

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void IsNotValidWhenHoldingCheckedOut()
        {
            validator.Data = new Dictionary<string, object>
                {{"Holding", new Holding {BranchId = Branch.CheckedOutBranch.Id}}};

            validator.Validate();

            Assert.False(validator.IsValid);
        }
    }
}