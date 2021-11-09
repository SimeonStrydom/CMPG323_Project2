using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptoject2.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("MetaData")]
        public int MetaId { get; set; }
        [ForeignKey("Album")]
        public int AlbumId { get; set; }

        public Photo()
        {

        }
    }
}
