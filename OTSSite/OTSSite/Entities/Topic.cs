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
        [StringLength(15)]
        public string TopicName { get; set; }
    }
}
