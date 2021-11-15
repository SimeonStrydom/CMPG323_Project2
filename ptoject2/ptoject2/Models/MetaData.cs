using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptoject2.Models
{
    public class MetaData
    {
        [Key]
        public int MetaId { get; set; }
        public DateTime CapturDate { get; set; }
        public string CaptureBy { get; set; }
        public string Geolocation { get; set; }
        public virtual IEnumerable<Tag> Tags { get; set; }

        public MetaData()
        {

        }
    }
}
