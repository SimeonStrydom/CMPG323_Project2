using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptoject2.Data;
using ptoject2.Models;

namespace ptoject2.Controllers
{
    public class MetaDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MetaDatasController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult>  ShowSearchResults(string CaptureBy, string Tags)  //not reciving values
        {
            string c = CaptureBy;
            string t = Tags;
            string search = "";
            if(t != "" & c != "")
            {
                search = c + " OR " + t;
            }
            else
            {
                if (c != "") { search = c; }
                if (t != "") { search = t; }
            }
            
            return View("Index", await _context.MetaData.Where(i => i.CaptureBy.Contains(search)).ToListAsync());
        }

        // GET: MetaDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaData = await _context.MetaData
                .FirstOrDefaultAsync(m => m.MetaId == id);
            if (metaData == null)
            {
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
                return NotFound();
            }

            var metaData = await _context.MetaData.FindAsync(id);
            if (metaData == null)
            {
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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaDataExists(metaData.MetaId))
                    {
                        return NotFound();
                    }
                    else
                    {
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
                return NotFound();
            }

            var metaData = await _context.MetaData
                .FirstOrDefaultAsync(m => m.MetaId == id);
            if (metaData == null)
            {
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
