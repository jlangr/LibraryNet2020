using LibraryNet2020.Controllers.Validations;
using Xunit;
using Moq;

namespace LibraryTest.Controllers.Validations
{
    public class NotValidatorTest
    {
        private readonly NotValidator notValidator;
        private readonly Mock<Validator> mock;

        public NotValidatorTest()
        {
            mock = new Mock<Validator>();
        }
        
        [Fact]
        public void IsValidWhenNestedValidatorIsInvalid()
        {
            mock.Setup(v => v.IsValid).Returns(false);
            var falseValidator = mock.Object;
            var notValidator = new NotValidator(falseValidator);

            var isValid = notValidator.IsValid;
            
            Assert.True(isValid);
        }
        
        [Fact]
        public void IsNotValidWhenNestedValidatorIsValid()
        {
            mock.Setup(v => v.IsValid).Returns(true);
            var trueValidator = mock.Object;
            var notValidator = new NotValidator(trueValidator);

            var isValid = notValidator.IsValid;
            
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnsErrorMessagesFromNestedValidator()
        {
            mock.Setup(v => v.ErrorMessage).Returns("hey");
            var validator = mock.Object;
            var notValidator = new NotValidator(validator);
            
            Assert.Equal("hey", notValidator.ErrorMessage);
        }
    }
}