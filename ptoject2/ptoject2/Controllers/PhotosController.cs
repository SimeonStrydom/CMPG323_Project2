using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptoject2.Data;
using ptoject2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace ptoject2.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;
        //private readonly string _downloadFilePath;
        private IConfiguration _config;
        private string AzureConnectionString { get; }

        

        public PhotosController(ApplicationDbContext context, IHostingEnvironment hostingEnv, IConfiguration config, UserManager<IdentityUser> userManager, ILogger<PhotosController> logger, ImageService imageService)
        {
            _context = context;
            _hostingEnv = hostingEnv;
            _config = config;
            _logger = logger;
            AzureConnectionString = _config["AzureStorageConectionString"];
            _userManager = userManager;
            //_downloadFilePath = downloadFilePath;
        }

        // GET: Photos
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var filePath = Path.Combine(_hostingEnv.WebRootPath, "images");
            string[] fileNames = Directory.GetFiles(filePath);
            var imageList = new List<Photo>();
            try
            {
                foreach (string file in fileNames)
                {
                    {
                        new Photo()
                        {
                            ImageName = Path.GetFileName(file),
                            ImagePath = file,

                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("In Index: File not found. Exception: {e}", e);
                throw;
            }
            

            var gallery = new GalleryIndexModel()
            {
                Images = imageList
            };
            return View(await _context.Photo.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("In Details: Id {id} is null.", id);
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photo == null)
            {
                _logger.LogWarning("In Details: Object {obj} is null.", photo);
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PhotoId, UserId, ImagePath,ImageName,MetaId,Image"), FromForm] Photo photo)
        {
            if (photo.Image != null)
            {
                var container = _imageService.GetBlobContainer(AzureConnectionString, "images");
                
                /*var fileName = Path.GetFileName(photo.Image.FileName);
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.Image.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                photo.ImagePath = filePath;
                photo.ImageName = fileName;
                //Photo photo1 = new Photo();
                //photo1.ImagePath = filePath;

                _context.Add(photo);
                await _context.SaveChangesAsync();
                return Redirect("https://localhost:44335/MetaDatas/Create");*/
            }
            else
            {
                _logger.LogWarning("In Create: Object {obj} is null.", photo);
                return View(photo);
            }
            /*if (ModelState.IsValid)
            {
                
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photo);*/
        }
        
        public string Upload(IFormFile  file)
        {
            var uploadDirectory = "uploads/";
            var uploadPath = Path.Combine(_hostingEnv.WebRootPath, uploadDirectory);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                
            {
                file.CopyToAsync(fileStream);
                fileStream.Close();
            }
            _logger.LogInformation("File {file} uploaded successfuly.", fileName);
            return fileName;
        }

        // GET: Photos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,ImagePath,ImageName,MetaId,AlbumId")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                _logger.LogWarning("In Edit: Id {id} is null", id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!PhotoExists(photo.PhotoId))
                    {
                        _logger.LogWarning("In Edit: Object {obj} is null", photo);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Database not updated. Exception: {e}", e);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("In Delete: Id {id} is null", id);
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            
            if (photo == null)
            {
                _logger.LogWarning("In Delete: Object {obj} is null", photo);
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            try
            {
                ImageDelete(Path.Combine(photo.ImagePath, photo.ImageName));
                _logger.LogInformation("File {file} was successfuly deleted");
            }
            catch (Exception e)
            {
                _logger.LogError("In DeleteConfirm: File delete exception: {e}", e);
                throw;
            }
             
            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.PhotoId == id);
        }
        
        private static void ImageDelete(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return;
            }

            bool isDeleted = false;
            while (!isDeleted)
            {
                try
                {
                    System.IO.File.Delete(path);
                    isDeleted = true;
                }
                catch (Exception e)     //exception logged in DeleteConfirm
                {
                }
                Thread.Sleep(50);
            }
        }

        // GET: Photos/ShareImage/5
        [Authorize]
        public async Task<IActionResult> ShareImage(int? id)
        {
            return View();
        }

        //POST: Photos/ShareConfirm 
        [HttpPost, ActionName("ShareImage")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ShareConfirm(int id, [Bind("SharedWith")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                _logger.LogWarning("In ShareConfirm: Id {id} is null", id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                try
                {

                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!PhotoExists(photo.PhotoId))
                    {
                        _logger.LogWarning("Share failed. Could not update DB. In ShareConfirm: Object {obj} is null", photo);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Share failed. Could not update DB. In ShareConfirm: Exception: {e}", e);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index)); //change to view(index) if problem presists
            /*photo = await _context.Photo.FindAsync(id);
            var shared = 
            _context.Update(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        [HttpGet("download")]
        [Authorize]
        public async Task<IActionResult> Download(int? id, [Bind("PhotoId,ImagePath,ImageName,MetaId,AlbumId")] Photo photo/*, [FromQuery] string link*/)
        {
            /*if (id == null)
            {
                _logger.LogWarning("In Download: Id {id} is null", id);
                return NotFound();
            }

            if (id != photo.PhotoId)
            {
                _logger.LogWarning("In Download: Id {id} and photoId {pId} mismatch", id, photo.PhotoId);
                id = photo.PhotoId;
                //return NotFound();
            }*/

            try
            {
                //var net = new System.Net.WebClient();
                //var data = net.DownloadData(link);
                //var content = new System.IO.MemoryStream(data);
                //var contentType = "APPLICATION/octet-stream";
                //var fileName = photo.ImageName;
                //return File(content, contentType, fileName);
                //if ^ not working try https://www.aspsnippets.com/Articles/ASPNet-Core-MVC-Download-Files-from-Folder-Directory.aspx

                //Build the File Path.
                string path = Path.Combine(_hostingEnv.WebRootPath, "images\\") + photo.ImageName; //hardcode bmp.bmp in path
                

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                //Send the File to Download.
                return File(bytes, "application/octet-stream", photo.ImageName);
            }
            catch (Exception e)
            {
                _logger.LogError("In Download: Exception: {e}", e);
                throw;
                //return View("Index");
            } 
        }

    }
}
