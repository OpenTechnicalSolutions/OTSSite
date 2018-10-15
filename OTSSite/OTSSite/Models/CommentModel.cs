using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int ReplyCommentId { get; set; } = -1;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }




    }
}
