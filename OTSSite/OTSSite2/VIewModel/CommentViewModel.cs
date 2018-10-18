using OTSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite2.VIewModel
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string Author { get; set; }
        public CommentViewModel[] Children { get; set; }
    }
}
