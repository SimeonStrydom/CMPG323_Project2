using System.Collections.Generic;
using ptoject2.Models;

namespace ptoject2.Controllers
{
    internal class GalleryIndexModel
    {
        public GalleryIndexModel()
        {
        }

        public IEnumerable<Photo> Images { get; set; }
    }
}