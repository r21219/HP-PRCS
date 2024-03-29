﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ProjectAPI.Model.Enum;

namespace ProjectAPI.Model
{
    public class NewUserDTO
    {
        [Required]
        [JsonProperty("email")]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [JsonProperty("username")]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [JsonProperty("forename")]
        public string Forename { get; set; }
        [Required]
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("gender")]
        public Gender Gender { get; set; }
    }
}
