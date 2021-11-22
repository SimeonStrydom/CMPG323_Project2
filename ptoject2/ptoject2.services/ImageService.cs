using Microsoft.WindowsAzure.Storage;
using ptoject2.Data;
using ptoject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public Photo GetById(int id)
        {
            return _context.Photo.Find(id);
        }

        public IEnumerable<Photo> GetWithTag(string tag)
        {
            //return GetAll().Where(img => img.Tags.Any(t => t.Description == tag));
            throw new NotImplementedException();
        }

        public object GetBlobContainer(String AzureConnectionString,string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(AzureConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName);
        }
    }
}
 