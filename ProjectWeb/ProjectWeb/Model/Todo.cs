using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectWeb.Model.Enum;

namespace ProjectWeb.Model
{
    public class Todo
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Description { get; set; }
        [Required]
        public Status Status { get; set; }

        public DateTime Date { get; set; }
    }
}
