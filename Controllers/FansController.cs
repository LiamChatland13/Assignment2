using System;
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
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: Fans
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new SportClubViewModel()
            {
                Fans = await _context.Fans
                  .Include(i => i.Subscriptions)
                  .AsNoTracking()
                  .OrderBy(i => i.LastName)
                  .ToListAsync()
            };

            if (id != null)
            {
                viewModel.Subscriptions = viewModel.Fans.Where(
                    i => i.Id == id).Single().Subscriptions;
                List<string> ids = viewModel.Subscriptions.Select(i => i.SportClubId).ToList();
                viewModel.SportClubs = await _context.SportClubs.Where(
                    z => ids.Contains(z.Id)).ToListAsync();
            }

            return View(viewModel);
        }

        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }



        //GET: Fans/EditSubscriptions/5
        public async Task<IActionResult> EditSubscriptions(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            var subscriptions = await _context.Subscriptions
            .Include(s => s.SportClub)
            .Where(s => s.FanId == id)
            .ToListAsync();

            var sportClubSubscriptions = subscriptions.Select(s => new SportClubSubscriptionViewModel
            {
                SportClubId = s.SportClub.Id,
                Title = s.SportClub.Title,
                IsMember = true 
            }).ToList();

            var allSportClubs = await _context.SportClubs.ToListAsync();

            var notSubscribedSportClubs = allSportClubs.Where(sc => !sportClubSubscriptions.Any(ss => ss.SportClubId == sc.Id));

            sportClubSubscriptions.AddRange(notSubscribedSportClubs.Select(sc => new SportClubSubscriptionViewModel
            {
                SportClubId = sc.Id,
                Title = sc.Title,
                IsMember = false 
            }));

            var viewModel = new FanSubscriptionViewModel()
            {
                Fan = fan,
                Subscriptions = sportClubSubscriptions
            };

            return View(viewModel);
        }






        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
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
            return View(fan);
        }

        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
          return _context.Fans.Any(e => e.Id == id);
        }
    }
}
