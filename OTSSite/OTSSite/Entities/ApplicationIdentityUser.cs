﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Entities
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public ApplicationIdentityUser() : base()
        { }
        public ApplicationIdentityUser(string username) : base (username)
        { }
    }
}
