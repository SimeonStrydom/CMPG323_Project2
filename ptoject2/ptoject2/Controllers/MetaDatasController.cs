using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptoject2.Data;
using ptoject2.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace ptoject2.Controllers
{
    public class MetaDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        public MetaDatasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<MetaDatasController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: MetaDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.MetaData.ToListAsync());
        }

        // GET: MetaDatas/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: MetaDatas/ShowSearchResults
        public async Task<IActionResult>  ShowSearchResults([Bind("CaptureBy, Tags")]string CaptureBy, string Tags)
        {
            return View("Index", 
                await _context.MetaData.Where( j => j.CaptureBy.Contains(CaptureBy) || j.Tags.ToString().Contains(Tags)).ToListAsync());
        }

        // GET: MetaDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("In Details: Id {id} is null.", id);
                return NotFound();
            }

            var metaData = await _context.MetaData
                .FirstOrDefaultAsync(m => m.MetaId == id);
            if (metaData == null)
            {
                _logger.LogWarning("In Details: Object {obj} is null.", metaData);
                return NotFound();
            }

            return View(metaData);
        }

        // GET: MetaDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MetaDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MetaId,CapturDate,CaptureBy,Geolocation,Tags")] MetaData metaData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metaData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metaData);
        }

        // GET: MetaDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("In MetaData/Edit: Id {id} is null.", id);
                return NotFound();
            }

            var metaData = await _context.MetaData.FindAsync(id);
            if (metaData == null)
            {
                _logger.LogWarning("In MetaData/Edit: Object {obj} is null.", metaData);
                return NotFound();
            }
            return View(metaData);
        }

        // POST: MetaDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MetaId,CapturDate,CaptureBy,Geolocation,Tags")] MetaData metaData)
        {
            if (id != metaData.MetaId)
            {
                _logger.LogWarning("In MetaData/Edit: Id {mId} is not found or Id mismatch with {id}.", metaData.MetaId, id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!MetaDataExists(metaData.MetaId))
                    {
                        _logger.LogWarning("In MetaData/Edit: Object {obj} is null.", metaData);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogWarning("DB update exception: {e}", e);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(metaData);
        }

        // GET: MetaDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("In MetaData/Delete: Id {id} is null.", id);
                return NotFound();
            }

            var metaData = await _context.MetaData
                .FirstOrDefaultAsync(m => m.MetaId == id);
            if (metaData == null)
            {
                _logger.LogWarning("In MetaData/Delete: Object {obj} is null.", metaData);
                return NotFound();
            }

            return View(metaData);
        }

        // POST: MetaDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metaData = await _context.MetaData.FindAsync(id);
            _context.MetaData.Remove(metaData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetaDataExists(int id)
        {
            return _context.MetaData.Any(e => e.MetaId == id);
        }
    }
}
