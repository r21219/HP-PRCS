using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ProjectAPI.Model.Enum;

namespace ProjectAPI.Model.DTO
{
    public class NewTodoDTO
    {
        public int UserId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [Required]
        [JsonProperty("status")]
        public Status Status { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
