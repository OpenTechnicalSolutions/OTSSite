using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Models
{
    #region CreationDtos
    public class CreateUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

    }

    #endregion

    public class GetUserProfileDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public DateTime JoinDateTime { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

