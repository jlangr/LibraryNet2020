using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
using Moq;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    public class PipelineValidatorTest
    {
        private readonly PipelineValidator pipeline = new PipelineValidator();

        [Fact]
        public void IsValidWhenNoErrorsExist()
        {
            Assert.True(pipeline.IsValid());
        }

        [Fact]
        public void IsInvalidWhenAValidationFails()
        {
            pipeline.Validate(new List<Validator> {PassingValidation(), FailingValidation()});

            Assert.False(pipeline.IsValid());
        }

        [Fact]
        public void IsValidWhenNoValidationFails()
        {
            pipeline.Validate(new List<Validator> {PassingValidation()});

            Assert.True(pipeline.IsValid());
        }

        [Fact]
        public void ReturnsErrorMessageFromFailedValidation()
        {
            pipeline.Validate(new List<Validator> {FailingValidation("my message")});
            
            Assert.Equal(new List<string> { "my message" }, pipeline.ErrorMessages);
        }

        [Fact]
        public void StopsValidationWhenAValidationFails()
        {
            pipeline.Validate(new List<Validator>
            {
                FailingValidation("my message"), 
                FailingValidation("should not get this message")
            });
            
            Assert.Equal(new List<string> { "my message" }, pipeline.ErrorMessages);
        }

        [Fact]
        public void PassesDataFromValidatorToNextViaMerge()
        {
            var validation1 = PassingValidation(new Dictionary<string, object> {{"A", "Alpha" }});
            var validation2 = PassingValidation(new Dictionary<string, object> {{"B", "Beta" }, {"G", "Gamma"}});
            var validation3 = PassingValidation(new Dictionary<string, object> {{"G", "New Gamma" }});
            
            pipeline.Validate(new List<Validator> { validation1, validation2, validation3 });
            
            Assert.Equal(new Dictionary<string,object>
            {
                { "A", "Alpha" },
                { "B", "Beta" },
                { "G", "Gamma" }
            }, pipeline.Data);
        }

        private Validator FailingValidation(string message = "default message")
        {
            var mock = new Mock<Validator>();
            mock.Setup(v => v.IsValid).Returns(false);
            mock.Setup(v => v.ErrorMessage).Returns(message);
            return mock.Object;
        }

        private Validator PassingValidation(Dictionary<string, object> data = null)
        {
            var mock = new Mock<Validator>();
            mock.Setup(v => v.IsValid).Returns(true);
            mock.Setup(v => v.Data).Returns(data);
            return mock.Object;
        }
    }
}