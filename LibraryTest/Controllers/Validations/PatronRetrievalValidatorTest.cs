using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    [Collection("SharedLibraryContext")]
    public class PatronRetrievalValidatorTest
    {
        private LibraryContext context;
        private PatronRetrievalValidator validator;

        public PatronRetrievalValidatorTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }

        [Fact]
        public void IsValidWhenHoldingExists()
        {
            validator = new PatronRetrievalValidator(context, 1);
            context.Patrons.Add(new Patron(1, ""));
            context.SaveChanges();

            validator.Validate();

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void IsNotValidWhenHoldingDoesNotExist()
        {
            validator = new PatronRetrievalValidator(context, 1);
            context.Patrons.Add(new Patron(2, ""));
            context.SaveChanges();

            validator.Validate();

            Assert.False(validator.IsValid);
        }
    }
}