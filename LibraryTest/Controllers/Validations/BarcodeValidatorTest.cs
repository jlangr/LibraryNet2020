using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Services;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    public class BarcodeValidatorTest
    {
        private BarcodeValidator validator;

        [Fact]
        public void IsValidWhenBarcodeIsValid()
        {
            validator = new BarcodeValidator(null, "AB123:1");

            validator.Validate();

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void IsNotValidWhenBarcodeIsNotValid()
        {
            validator = new BarcodeValidator(null, "invalid");

            validator.Validate();

            Assert.False(validator.IsValid);
        }
    }
}