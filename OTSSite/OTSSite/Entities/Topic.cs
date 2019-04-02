using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Entities
{
    public class Topic
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string TopicName { get; set; }
    }
}
