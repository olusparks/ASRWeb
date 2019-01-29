using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASRWebMVC.Models;

namespace ASRWebMVC.Controllers
{
    public class SlotTesController : Controller
    {
        private readonly ASRDbfContext _context;

        public SlotTesController(ASRDbfContext context)
        {
            _context = context;
        }

        // GET: SlotTes
        public async Task<IActionResult> Index()
        {
            var aSRDbfContext = _context.Slot.Include(s => s.BookedInStudent).Include(s => s.Room).Include(s => s.Staff);
            return View(await aSRDbfContext.ToListAsync());
        }

        // GET: SlotTes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot
                .Include(s => s.BookedInStudent)
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (slot == null)
            {
                return NotFound();
            }

            return View(slot);
        }

        // GET: SlotTes/Create
        public IActionResult Create()
        {
            ViewData["BookedInStudentId"] = new SelectList(_context.User, "UserId", "UserId");
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId");
            ViewData["StaffId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: SlotTes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,StartTime,StaffId,BookedInStudentId")] Slot slot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookedInStudentId"] = new SelectList(_context.User, "UserId", "UserId", slot.BookedInStudentId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", slot.RoomId);
            ViewData["StaffId"] = new SelectList(_context.User, "UserId", "UserId", slot.StaffId);
            return View(slot);
        }

        // GET: SlotTes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot.FindAsync(id);
            if (slot == null)
            {
                return NotFound();
            }
            ViewData["BookedInStudentId"] = new SelectList(_context.User, "UserId", "UserId", slot.BookedInStudentId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", slot.RoomId);
            ViewData["StaffId"] = new SelectList(_context.User, "UserId", "UserId", slot.StaffId);
            return View(slot);
        }

        // POST: SlotTes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoomId,StartTime,StaffId,BookedInStudentId")] Slot slot)
        {
            if (id != slot.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlotExists(slot.RoomId))
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
            ViewData["BookedInStudentId"] = new SelectList(_context.User, "UserId", "UserId", slot.BookedInStudentId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", slot.RoomId);
            ViewData["StaffId"] = new SelectList(_context.User, "UserId", "UserId", slot.StaffId);
            return View(slot);
        }

		//GET: SlotTes/Delete/5
        public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var slot = await _context.Slot
				.Include(s => s.BookedInStudent)
				.Include(s => s.Room)
				.Include(s => s.Staff)
				.FirstOrDefaultAsync(m => m.RoomId == id);
			if (slot == null)
			{
				return NotFound();
			}

			return View(slot);
		}

		// POST: SlotTes/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string stTime)
        {
            var slot = await _context.Slot.FindAsync(id);
            _context.Slot.Remove(slot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlotExists(string id)
        {
            return _context.Slot.Any(e => e.RoomId == id);
        }
    }
}
