using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
using Xunit;
using Moq;

namespace LibraryTest.Controllers.Validations
{
    public class NotValidatorTest
    {
        private NotValidator notValidator;
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
            notValidator = new NotValidator(falseValidator);

            var isValid = notValidator.IsValid;
            
            Assert.True(isValid);
        }
        
        [Fact]
        public void IsNotValidWhenNestedValidatorIsValid()
        {
            mock.Setup(v => v.IsValid).Returns(true);
            var trueValidator = mock.Object;
            notValidator = new NotValidator(trueValidator);

            var isValid = notValidator.IsValid;
            
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnsErrorMessagesFromNestedValidator()
        {
            mock.Setup(v => v.ErrorMessage).Returns("hey");
            var validator = mock.Object;
            notValidator = new NotValidator(validator);
            
            Assert.Equal("hey", notValidator.ErrorMessage);
        }

        [Fact]
        public void MergeUpdatesWithDataFromNestedValidation()
        {
            var data = new Dictionary<string, object> {{"a", "Alpha"}};
            var nestedValidator = new PassingValidator(data);
            notValidator = new NotValidator(nestedValidator);
            
            notValidator.MergePreviousValidationData(new Dictionary<string, object> { { "b", "Beta"}});
            
            Assert.Equal(
                new Dictionary<string,object> { {"a", "Alpha"}, {"b", "Beta"}},
                notValidator.Data);
        }
    }
}