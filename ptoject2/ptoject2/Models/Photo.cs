using Microsoft.AspNetCore.Http;
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
        [Display(Name = "Photo name")]
        public string ImageName { get; set; }
        [ForeignKey("MetaData")]
        public int MetaId { get; set; }
        [ForeignKey("Album")]
        public int AlbumId { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public  IFormFile Image { get; set; }


        public Photo()
        {

        }
    }

    /*public class PhotoViewModel
    {
        public string PhotoName { get; set; }
        public IFormFile Image { get; set; }
    }*/
}
