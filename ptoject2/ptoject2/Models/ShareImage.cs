using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptoject2.Models
{
    public class ShareImage
    {
        [Key]
        public int SharedId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public string SharedWithId { get; set; }
    }
}
