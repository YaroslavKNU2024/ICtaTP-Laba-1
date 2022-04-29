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
    public class VariantsController : Controller
    {
        private readonly VirusBaseContext _context;
        private Variant vr = new Variant();
        public VariantsController(VirusBaseContext context) {
            _context = context;
        }

        // GET: Variants
        public async Task<IActionResult> Index()
        {

            return View(await _context.Variants.ToListAsync());
        }

        // GET: Variants/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null)
                return NotFound();

            var variant = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            var variantCountriesCopy = await _context.Variants.AsNoTracking().Include(x => x.Countries).FirstOrDefaultAsync(c => c.Id == id);
            var variantSymptomsCopy = await _context.Variants.AsNoTracking().Include(x => x.Symptoms).FirstOrDefaultAsync(c => c.Id == id);
            if (variant == null)
                return NotFound();

            return View(variant);
        }

        // GET: /Create
        public IActionResult Create() {
            return View();
        }

        // POST: /Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VariantName,VariantOrigin,VariantDateDiscovered, VirusId")] Variant variant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variant);
                vr = variant;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(variant);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var virus = await _context.Variants.Include(x => x.Countries).FirstOrDefaultAsync(c => c.Id == id);
            if (virus == null)// && variantSymptomsCopy == null)
                return NotFound();

            ViewBag.Countries = new MultiSelectList(_context.Countries, "Id", "CountryName");
            ViewBag.Symptoms = new MultiSelectList(_context.Symptoms, "Id", "SymptomName");
            var variantEdit = new VariantsEdit
            {
                Id = virus.Id,
                VariantName = virus.VariantName,
                VariantOrigin = virus.VariantOrigin,
                VariantDateDiscovered = virus.VariantDateDiscovered,
                CountriesIds = virus.Countries.Select(c => c.Id).ToList(),
                SymptomsIds = virus.Symptoms.Select(s => s.Id).ToList()
            };

            ViewBag.VariantName = variantEdit.VariantName;
            return View(variantEdit);
            //if (id == null)
            //    return NotFound();

            //var variantCountriesCopy = await _context.Variants.Include(x => x.Countries).FirstOrDefaultAsync(c => c.Id == id);
            //var variantSymptomsCopy = await _context.Variants.Include(x => x.Symptoms).FirstOrDefaultAsync(c => c.Id == id);
            //if (variantCountriesCopy == null && variantSymptomsCopy == null)
            //    return NotFound();

            //ViewBag.Countries = new MultiSelectList(_context.Countries, "Id", "CountryName");
            //ViewBag.Symptoms = new MultiSelectList(_context.Symptoms, "Id", "SymptomName");
            //var variantEdit = new VariantsEdit {
            //    Id = variantCountriesCopy.Id,
            //    VariantName = variantCountriesCopy.VariantName,
            //    VariantOrigin = variantCountriesCopy.VariantOrigin,
            //    VariantDateDiscovered = variantCountriesCopy.VariantDateDiscovered,
            //    CountriesIds = variantCountriesCopy.Countries.Select(c => c.Id).ToList(),
            //    SymptomsIds = variantSymptomsCopy.Symptoms.Select(s => s.Id).ToList()
            //};
            //ViewBag.VariantName = variantEdit.VariantName;
            //return View(variantEdit);
        }



        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VariantsEdit variantEdit)//[Bind("Id,CountryName")] Country country)
        {
            if (id != variantEdit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var virus = await _context.Variants.AsNoTracking().Include(c => c.Countries).FirstOrDefaultAsync(d => d.Id == variantEdit.Id);
                if (virus is null)
                    return NotFound();
                var virusIndices = _context.Variants.Select(c => c.Countries).ToList();

                virus.VariantName = variantEdit.VariantName;
                var countries = await _context.Countries.AsNoTracking().Where(v => variantEdit.CountriesIds.Contains(v.Id)).ToListAsync();
                virus.Countries = countries;
                var symptoms = await _context.Symptoms.AsNoTracking().Where(v => variantEdit.SymptomsIds.Contains(v.Id)).ToListAsync();
                virus.Symptoms = symptoms;

                //Variant variant = new Variant();
                virus.Id = variantEdit.Id;
                virus.VariantName = variantEdit.VariantName;
                virus.VariantOrigin = variantEdit.VariantOrigin;
                virus.Symptoms = virus.Symptoms;
                virus.Countries = virus.Countries;
                try
                {
                    _context.Update(virus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariantExists(virus.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
            //if (id != variantEdit.Id)
            //    return NotFound();

            //if (ModelState.IsValid)
            //{
            //    var variantCountriesCopy = await _context.Variants.AsNoTracking().Include(c => c.Countries).FirstOrDefaultAsync(d => d.Id == variantEdit.Id);
            //    var variantSymptomsCopy = await _context.Variants.AsNoTracking().Include(c => c.Symptoms).FirstOrDefaultAsync(d => d.Id == variantEdit.Id);
            //    if (variantCountriesCopy is null)
            //        return NotFound();

            //    variantCountriesCopy.VariantName = variantEdit.VariantName;
            //    var countries = await _context.Countries.AsNoTracking().Where(v => variantEdit.CountriesIds.Contains(v.Id)).ToListAsync();
            //    variantCountriesCopy.Countries = countries;
            //    var symptoms = await _context.Symptoms.AsNoTracking().Where(v => variantEdit.SymptomsIds.Contains(v.Id)).ToListAsync();
            //    variantSymptomsCopy.Symptoms = symptoms;

            //    //Variant variant = new Variant();
            //    variantCountriesCopy.Id = variantEdit.Id;
            //    variantCountriesCopy.VariantName = variantEdit.VariantName;
            //    variantCountriesCopy.VariantOrigin = variantEdit.VariantOrigin;
            //    variantCountriesCopy.Symptoms = variantSymptomsCopy.Symptoms;
            //    variantCountriesCopy.Countries = variantCountriesCopy.Countries;
            //    try {
            //        _context.Update(variantCountriesCopy);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException) {
            //        if (!VariantExists(variantCountriesCopy.Id))
            //            return NotFound();
            //        else
            //            throw;
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View();
        }






                // GET: Variants/Delete/5
                public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variant = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variant == null) {
                return NotFound();
            }

            return View(variant);
        }

        // POST: Variants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variant = await _context.Variants.FindAsync(id);
            _context.Variants.Remove(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariantExists(int id)
        {
            return _context.Variants.Any(e => e.Id == id);
        }
    }
}