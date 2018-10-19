using OTSSite2.Models;
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
        public DateTime TimeStamp { get; set; }
        public int Children { get; set; }
    }
}
