using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryNet2020.Models
{
    [Serializable]
    public class Branch // : Identifiable
    {
        public const int CheckedOutId = 0;
        public static readonly Branch CheckedOutBranch = new Branch { Id = CheckedOutId, Name = "Checked Out" };

        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }
        
    }
}

