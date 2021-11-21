using ptoject2.Data;
using ptoject2.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ptoject2.services
{
    public class ImageService : IImage
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Photo> GetAll()
        {
           return _context.Photo.include(img => img.Tags);
        }

        public Photo GetById()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Photo> GetWithTag(string tag)
        {
            throw new NotImplementedException();
        }
    }
}
 