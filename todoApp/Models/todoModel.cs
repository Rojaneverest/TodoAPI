using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApp.Models
{
    public class todoModel
    {
        [Key]
        public int Id { get; set; }
        
        
        public int UserId { get; set; } 
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }


        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}
