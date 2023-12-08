﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{
    public class SportClubsController : Controller
    {
        private readonly SportsDbContext _context;

        public SportClubsController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: SportClubs
        public async Task<IActionResult> Index(string? id)
        {
            var viewModel = new NewsViewModel()
            {
                SportClubs = await _context.SportClubs
                  .Include(i => i.Subscriptions)
                  .AsNoTracking()
                  .OrderBy(i => i.Fee)
                  .ToListAsync()
            };

            if (id != null)
            {
                viewModel.Subscriptions = viewModel.SportClubs.Where(
                    i => i.Id == id).Single().Subscriptions;
                List<int> ids = viewModel.Subscriptions.Select(i => i.FanId).ToList();
                viewModel.Fans = await _context.Fans.Where(
                    z => ids.Contains(z.Id)).ToListAsync();
            }
            return View(viewModel);
        }

        // GET: SportClubs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // GET: SportClubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportClubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportClub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportClub);
        }

        // GET: SportClubs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs.FindAsync(id);
            if (sportClub == null)
            {
                return NotFound();
            }
            return View(sportClub);
        }

        // POST: SportClubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (id != sportClub.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportClub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportClubExists(sportClub.Id))
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
            return View(sportClub);
        }

        // GET: SportClubs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // POST: SportClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SportClubs == null)
            {
                return Problem("Entity set 'SportsDbContext.SportClubs'  is null.");
            }
            var sportClub = await _context.SportClubs.FindAsync(id);
            if (sportClub != null)
            {
                _context.SportClubs.Remove(sportClub);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportClubExists(string id)
        {
          return _context.SportClubs.Any(e => e.Id == id);
        }
    }
}
