#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirusAppFinal;
using VirusAppFinal.Models;


namespace VirusAppFinal.Controllers
{
    public class CountriesController : Controller
    {
        private readonly VirusBaseContext _context;

        public CountriesController(VirusBaseContext context) {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(v => v.Variants)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
                return NotFound();

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create() {
            return View(); 
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Countries");
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var country = await _context.Countries.Include(c => c.Variants).FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
                return NotFound();
            
            ViewBag.Variants = new MultiSelectList(_context.Variants, "Id", "VariantName");
            var countriesEdit = new CountriesEdit
            {
                Id = country.Id,
                CountryName = country.CountryName,
                VariantsIds = country.Variants.Select(c => c.Id).ToList()
            };
            ViewBag.CountryName = countriesEdit.CountryName;
            return View(countriesEdit);
        }



        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CountriesEdit countryEdit)//[Bind("Id,CountryName")] Country country)
        {
            if (id != countryEdit.Id)
                return NotFound();

            if (ModelState.IsValid) {
                var country = await _context.Countries.Include(c => c.Variants).FirstOrDefaultAsync(d => d.Id == countryEdit.Id);
                if (country is null)
                    return NotFound();

                country.CountryName = countryEdit.CountryName;
                var variants = await _context.Variants.Where(v => countryEdit.VariantsIds.Contains(v.Id)).ToListAsync();
                country.Variants = variants;
                try {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!CountryExists(country.Id))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            ViewBag.CountryName = country.CountryName;
            //ViewBag.AlbumId = song.AlbumId;
            //var album = await _context.Albums.FindAsync(country.AlbumId);
            //ViewBag.AlbumName = album!.Name;
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country  == null) return NotFound();
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}