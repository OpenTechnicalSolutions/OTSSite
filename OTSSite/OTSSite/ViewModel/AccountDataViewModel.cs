﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ViewModel
{
    public class AccountDataViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
