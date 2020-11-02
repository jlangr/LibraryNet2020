using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
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
            pipeline.Validate(new List<Validator> {new PassingValidator(), new FailingValidator()});

            Assert.False(pipeline.IsValid());
        }

        [Fact]
        public void IsValidWhenNoValidationFails()
        {
            pipeline.Validate(new List<Validator> {new PassingValidator()});

            Assert.True(pipeline.IsValid());
        }

        [Fact]
        public void ReturnsErrorMessageFromFailedValidation()
        {
            pipeline.Validate(new List<Validator> {new FailingValidator("my message")});
            
            Assert.Equal(new List<string> { "my message" }, pipeline.ErrorMessages);
        }

        [Fact]
        public void StopsValidationWhenAValidationFails()
        {
            pipeline.Validate(new List<Validator>
            {
                new FailingValidator("my message"), 
                new FailingValidator("should not get this message")
            });
            
            Assert.Equal(new List<string> { "my message" }, pipeline.ErrorMessages);
        }

        [Fact]
        public void PassesDataFromValidatorToNextViaMerge()
        {
            var validation1 = new PassingValidator(new Dictionary<string, object> {{"A", "Alpha" }});
            // var validation2 = new NotValidator(new FailingValidator("", new Dictionary<string, object> {{"B", "Beta" }, {"G", "Step 2 Gamma"}}));
            var validation2 = 
                new PassingValidator(new Dictionary<string, object> {{"B", "Beta" }, {"G", "Step 2 Gamma"}});
            var validation3 = 
                new PassingValidator(new Dictionary<string, object> {{"G", "Step 3 Gamma" }});
            
            pipeline.Validate(new List<Validator> { validation1, validation2, validation3 });
            
            Assert.Equal(new Dictionary<string,object>
            {
                { "A", "Alpha" },
                { "B", "Beta" },
                { "G", "Step 2 Gamma" }
            }, pipeline.Data);
        }

        class PassingValidator: Validator
        {
            public PassingValidator(Dictionary<string, object> data = null)
            {
                Data = data;
            }
            public override void Validate() {}

            public override bool IsValid => true;
            public override string ErrorMessage { get; }
        }

        class FailingValidator : Validator
        {
            public FailingValidator(string message = "default message", Dictionary<string,object> data = null)
            {
                ErrorMessage = message;
                Data = data;
            }
            public override void Validate() {}

            public override bool IsValid => false;
            public override string ErrorMessage { get; }
        }
    }
}