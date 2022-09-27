using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            //var pageSize = int.Parse(Request.Form["length"]);
            //var skip = int.Parse(Request.Form["start"]);

            var categories = await _context.Categories.ToListAsync();

            if (categories != null)
            {
                return View();
            }
            else
                return Problem("No Records Found!");

        }
        public ActionResult Data()
        {
            try
            {
                var categories = _context.Categories.ToList();
                var totalRecords = categories.Count();

                if (totalRecords > 0)
                    return Json(new { recordsFiltered = totalRecords, totalRecords, data = categories });
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category());

            else
                return View(_context.Categories.Find(id));
        }

        // POST: Category/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Title,Icon,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                    _context.Add(category);

                else
                    _context.Update(category);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
