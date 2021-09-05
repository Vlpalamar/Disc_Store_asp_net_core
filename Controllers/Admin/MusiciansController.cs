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
    public class MusiciansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusiciansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Musicians
        public async Task<IActionResult> Index()
        {
            return View(await _context.musicians.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.musicians.Include(m=>m.roleInGroup)
                .FirstOrDefaultAsync(m => m.id == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            ViewBag.Role = new SelectList(_context.roles, "id", "roleName");
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] Musician musician,int roleInGroup)
        {
            if (ModelState.IsValid)
            {
                musician.roleInGroup = _context.roles.Find(roleInGroup);
                _context.Add(musician);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musician);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var musician = _context.musicians.Include(p => p.roleInGroup).FirstOrDefault(m => m.id == id);
            ViewBag.Role = new SelectList(_context.roles, "id", "roleName", musician.roleInGroup.id);
            if (musician == null)
            {
                return NotFound();
            }
            return View(musician);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name")] Musician musician, int roleInGroup)
        {
            if (id != musician.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    musician.roleInGroup = _context.roles.Find(roleInGroup);
                    _context.Update(musician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicianExists(musician.id))
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
            return View(musician);
        }

        // GET: Musicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.musicians
                .FirstOrDefaultAsync(m => m.id == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musician = await _context.musicians.FindAsync(id);
            _context.musicians.Remove(musician);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicianExists(int id)
        {
            return _context.musicians.Any(e => e.id == id);
        }
    }
}
