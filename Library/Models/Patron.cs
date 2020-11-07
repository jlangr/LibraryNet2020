using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryNet2020.Models
{
    [Serializable]
    public class Patron: Identifiable
    {
        public Patron() { }

        public Patron(int id, string name)
            : this()
        {
            Name = name;
            Id = id;
        }

        [Required, StringLength(25)]
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public decimal Balance { get; set; }
        
        public string SSN { get; set; }

        public void Fine(decimal amount)
        {
            Balance += amount;
        }

        public void Remit(decimal amount)
        {
            Balance -= amount;
        }
    }
}