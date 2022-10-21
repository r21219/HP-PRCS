using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataTables.Model.Enum;

namespace DataTables.Model
{
    public class Todo
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("state")]
        public State State { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

    }
}
