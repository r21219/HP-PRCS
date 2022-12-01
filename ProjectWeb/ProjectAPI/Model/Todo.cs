using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectAPI.Model.Enum;

namespace ProjectAPI.Model
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
