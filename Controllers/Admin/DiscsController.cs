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
    public class DiscsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discs
        public async Task<IActionResult> Index()
        {
            return View(await _context.discs.ToListAsync());
        }

        // GET: Discs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var  disc  = await  _context.discs.Include(d=>d.band).Include(d=>d.label)
                .FirstOrDefaultAsync(m => m.id == id);
            if (disc.label!=null)
            {
                ViewBag.labalId = disc.label.name;
            }
            if (disc.band != null)
            {
                ViewBag.bandId = disc.band.name;
            }
            
            if (disc == null)
            {
                return NotFound();
            }

            return View(disc);
        }

        // GET: Discs/Create
        public IActionResult Create()
        {
            ViewBag.BandId = new SelectList(_context.bands, "id", "name");
            ViewBag.LableId = new SelectList(_context.labels, "id", "name");
            return View();
        }

        // POST: Discs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,dateOfPublish,price")] Disc disc,int band,int label)
        {
            if (ModelState.IsValid)
            {
                disc.band = _context.bands.Find(band);
                disc.label = _context.labels.Find(label);

                _context.Add(disc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disc);
        }

        // GET: Discs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var  disc = _context.discs
                .Include(d => d.band)
                .Include(d=>d.label)
                .FirstOrDefault(m => m.id == id);

            ViewBag.BandId= new SelectList(_context.bands, "id", "name");
            ViewBag.LableId= new SelectList(_context.labels, "id", "name");
            if (disc == null)
            {
                return NotFound();
            }
            return View(disc);
        }

        // POST: Discs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,dateOfPublish,price")] Disc disc, int band, int label)
        {
            if (id != disc.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    disc.band = _context.bands.Find(band);
                    disc.label = _context.labels.Find(label);
                    _context.Update(disc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscExists(disc.id))
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
            return View(disc);
        }

        // GET: Discs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disc = await _context.discs
                .FirstOrDefaultAsync(m => m.id == id);
            if (disc == null)
            {
                return NotFound();
            }

            return View(disc);
        }

        // POST: Discs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disc = await _context.discs.FindAsync(id);
            _context.discs.Remove(disc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscExists(int id)
        {
            return _context.discs.Any(e => e.id == id);
        }
    }
}
