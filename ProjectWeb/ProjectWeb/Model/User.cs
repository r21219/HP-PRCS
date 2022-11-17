using System.ComponentModel.DataAnnotations;
using ProjectWeb.Model.Enum;

namespace ProjectWeb.Model
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        
        public Gender Gender  { get; set; }
        public List<Todo>? Todo { get; set; }
    }
}
