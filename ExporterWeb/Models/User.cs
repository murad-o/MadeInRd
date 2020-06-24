using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class User : IdentityUser
    {
        public User()
        {
        }
        public User(string userName) : base(userName)
        {
        }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string SecondName { get; set; } = "";

        public string? Patronymic { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
