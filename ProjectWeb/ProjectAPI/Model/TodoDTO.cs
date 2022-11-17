using System.ComponentModel.DataAnnotations;
using ProjectWeb.Model.Enum;

namespace ProjectAPI.Model
{
    public class TodoDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        [Required]
        public Status Status { get; set; }

        public DateTime Date { get; set; }
    }
}
