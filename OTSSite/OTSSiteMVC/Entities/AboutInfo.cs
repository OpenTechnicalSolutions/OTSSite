using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public class AboutInfo
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string FounderName { get; set; }
        public DateTime FoundingDate { get; set; }
        public string Description { get; set; }
    }
}
