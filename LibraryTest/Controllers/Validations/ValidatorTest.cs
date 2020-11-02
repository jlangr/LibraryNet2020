using System.Collections.Generic;
using Xunit;

namespace LibraryTest.Controllers.Validations
{
    public class ValidatorTest
    {
        [Fact]
        public void MergesPreviousValidationData()
        {
            var data = new Dictionary<string, object> {{"a", "Alpha"}, {"b", "Beta"}};
            var validator = new PassingValidator(data);

            validator.MergePreviousValidationData(new Dictionary<string, object> {{"g", "Gamma"}, {"b", "New Beta"}});

            Assert.Equal(
                new Dictionary<string, object> {{"a", "Alpha"}, {"b", "New Beta"}, {"g", "Gamma"}},
                validator.Data);
        }
        
        [Fact]
        public void MergesPreviousValidationDataIgnoresNullInput()
        {
            var data = new Dictionary<string, object> {{"a", "Alpha"}, {"b", "Beta"}};
            var validator = new PassingValidator(data);

            validator.MergePreviousValidationData(null);

            Assert.Equal(
                new Dictionary<string, object> {{"a", "Alpha"}, {"b", "Beta"}},
                validator.Data);
        }
    }
}