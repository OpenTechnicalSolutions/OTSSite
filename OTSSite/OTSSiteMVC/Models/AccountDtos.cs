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
        //[RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")]
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
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }

    public class LoginDto
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ModifyUserRolesDto
    {
        public Guid UserId { get; set; }
        public string[] Roles { get; set; }
    }
}

