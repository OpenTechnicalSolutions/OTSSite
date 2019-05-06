using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OTSSite.Pages.Authors
{
    [Authorize(Roles = "author")]
    public class AuthorDashboardModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}