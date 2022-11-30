using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ProjectWeb.Model.Enum;

namespace ProjectAPI.Model
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
