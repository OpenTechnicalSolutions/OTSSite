using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OTSSiteMVC.Entities
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Website { get; set; }
        public string Description { get; set; }
        public DateTime JoinDateTime { get; set; }
        [ForeignKey("ImageDataId")]
        public Guid ImageDataId { get; set; }

        public AppIdentityUser(string username) : base(username) { }

        public AppIdentityUser() : base() { }
    }
}
