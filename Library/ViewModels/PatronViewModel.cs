using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryNet2020.Models;

namespace LibraryNet2020.ViewModels
{
    public class PatronViewModel
    {
        public PatronViewModel()
        {
        }

        public PatronViewModel(Patron patron)
        {
            Id = patron.Id;
            Balance = patron.Balance;
            Name = patron.Name;
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int Id { get; }

        [NotMapped] public IEnumerable<Holding> Holdings { get; set; }
    }
}