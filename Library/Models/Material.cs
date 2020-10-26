using System.ComponentModel.DataAnnotations.Schema;
using LibraryNet2020.NonPersistentModels;

namespace LibraryNet2020.Models
{
    public class Material: Identifiable
    {
        public string Director { get { return Author; } }
        
        public int Id { get; set; }
        
        public string CheckoutPolicyId { get; set; }
        
        [NotMapped]
        public CheckoutPolicy CheckoutPolicy
        {
            get => CheckoutPolicyFactory.Create(CheckoutPolicyId);
            set => CheckoutPolicyId = value.Id;
        }
        public string Title { get; set; }
        public string Classification { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
    }
}
