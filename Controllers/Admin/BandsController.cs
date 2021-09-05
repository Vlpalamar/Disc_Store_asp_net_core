using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disc_Store.Data;
using Disc_Store.Entities;

namespace Disc_Store.Controllers.Admin
{
    public class BandsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bands
        public async Task<IActionResult> Index()
        {
            return View(await _context.bands.ToListAsync());
        }

        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.bands.Include(m=>m.musicians).Include(b=>b.discs)
                .FirstOrDefaultAsync(m => m.id == id);
            ViewBag.Musicians = band.musicians;
            ViewBag.discs = band.discs;
            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

        // GET: Bands/Create
        public IActionResult Create()
        {
           
           ViewBag.musiciansIds = new SelectList(_context.musicians, "id", "name");
           ViewBag.discsIds = new SelectList(_context.discs, "id", "name");
            return View();
        }

        // POST: Bands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] Band band, int[] musicians,int[] discs) 
        {
            if (ModelState.IsValid)
            {
                band.musicians = _context.musicians.Where(m => musicians.Contains(m.id)).ToList();
                    _context.Add(band);
                    band.discs = _context.discs.Where(d => discs.Contains(d.id)).ToList();
                    _context.Add(band);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View(band);
        }

        // GET: Bands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.bands.Include(b=>b.musicians).Include(b => b.discs).FirstOrDefaultAsync(m => m.id == id);
            if (band == null)
            {
                return NotFound();
            }
            ViewBag.musiciansIds = new SelectList(_context.musicians, "id", "name");
            ViewBag.discsIds = new SelectList(_context.discs, "id", "name");
            return View(band);
        }

        // POST: Bands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name")] Band band, int[] musicians, int[] discs)
        {
            if (id != band.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (musicians!=null)
                    {
                       
                        band.musicians = _context.musicians.Where(m => musicians.Contains(m.id)).ToList();
                    }

                    if (discs!=null)
                    {
                        band.discs = _context.discs.Where(m => discs.Contains(m.id)).ToList();

                    }

                   var bandToChange= _context.bands.Where(b => b.id == band.id).FirstOrDefaultAsync();
                  

                    _context.Update(band.musicians);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandExists(band.id))
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
           // ViewData["BandId"] = new SelectList(_context.bands, "id", "name");
            return View(band);
        }

        // GET: Bands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.bands
                .FirstOrDefaultAsync(m => m.id == id);
            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

        // POST: Bands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var band = await _context.bands.FindAsync(id);
            _context.bands.Remove(band);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandExists(int id)
        {
            return _context.bands.Any(e => e.id == id);
        }
    }
}
