using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryNet2020.Models
{
    public interface Identifiable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        int Id {get; set; }
    }
}
