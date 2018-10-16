using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ViewModel
{
    public class ArticleViewModel
    {
        public string ArticleTitle { get; set; }
        public string AutherId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Article { get; set; }
    }
}
