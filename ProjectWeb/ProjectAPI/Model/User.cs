using System.ComponentModel.DataAnnotations;
using ProjectAPI.Model.Enum;

namespace ProjectAPI.Model
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public Gender Gender  { get; set; }
        public List<Todo>? Todo { get; set; }
    }
}
