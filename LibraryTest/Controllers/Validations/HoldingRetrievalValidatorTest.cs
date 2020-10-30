using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    [Collection("SharedLibraryContext")]
    public class HoldingRetrievalValidatorTest
    {
        private LibraryContext context;
        private HoldingRetrievalValidator validator;

        public HoldingRetrievalValidatorTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }

        [Fact]
        public void IsValidWhenHoldingExists()
        {
            validator = new HoldingRetrievalValidator(context, "AC123:1");
            context.Holdings.Add(new Holding("AC123:1"));
            context.SaveChanges();

            validator.Validate();

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void IsNotValidWhenHoldingDoesNotExist()
        {
            validator = new HoldingRetrievalValidator(context, "ABC123:1");
            context.Holdings.Add(new Holding("QA999:1"));
            context.SaveChanges();

            validator.Validate();

            Assert.False(validator.IsValid);
        }
    }
}