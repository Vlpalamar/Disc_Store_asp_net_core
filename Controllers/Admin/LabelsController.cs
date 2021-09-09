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
    public class LabelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Labels
        public async Task<IActionResult> Index()
        {
            return View(await _context.labels.ToListAsync());
        }

        // GET: Labels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.labels
                .Include(l=>l.Discs)
                .FirstOrDefaultAsync(m => m.id == id);
            ViewBag.discs = label.Discs;
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // GET: Labels/Create
        public IActionResult Create()
        {
            ViewBag.discsIds = new SelectList(_context.discs, "id", "name");
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] Label label, int[] Discs)
        {
            if (ModelState.IsValid)
            {

               
                label.Discs= _context.discs.Where(d => Discs.Contains(d.id)).ToList();
                _context.Add(label);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(label);
        }

        // GET: Labels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.labels
                .Include(l=>l.Discs)
                .FirstOrDefaultAsync(m => m.id == id);
            if (label == null)
            {
                return NotFound();
            }
            ViewBag.discsIds = new SelectList(_context.discs, "id", "name");
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name")] Label label, int[] Discs)
        {
            if (id != label.id)
            {
                return NotFound();
            }


            _context.Entry(label).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            label = _context.labels
                .Where(b => b.id == label.id)
                .Include(b => b.Discs)
                .First();


            if (ModelState.IsValid)
            {
                try
                {

                    if (Discs != null)
                    {
                        label.Discs = _context.discs.Where(m => Discs.Contains(m.id)).ToList();

                    }
                    _context.Update(label);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelExists(label.id))
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
            return View(label);
        }

        // GET: Labels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.labels
                .FirstOrDefaultAsync(m => m.id == id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var label = await _context.labels.FindAsync(id);
            _context.labels.Remove(label);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabelExists(int id)
        {
            return _context.labels.Any(e => e.id == id);
        }
    }
}
