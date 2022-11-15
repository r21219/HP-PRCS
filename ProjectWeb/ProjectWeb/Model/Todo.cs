using System.ComponentModel.DataAnnotations;
using ProjectWeb.Model.Enum;

namespace ProjectWeb.Model
{
    public class Todo
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public Status Status { get; set; }

        public DateTime Date { get; set; }
    }
}
