using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {

            var appDbContext = _context.Transactions.Include(t => t.Category);
            return View(await appDbContext.ToListAsync());
        }

        public ActionResult Data()
        {
            try
            {
                GetCategories();

                var transactions = _context.Transactions.ToList();
                var totalRecords = transactions.Count();

                if (totalRecords > 0)
                    return Json(new { recordsFiltered = totalRecords, totalRecords, data = transactions });
                else
                    return Problem("No Transactions Found!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            GetCategories();
            if (id == 0)
                return View(new Transaction());
            
            else
                return View(_context.Transactions.Find(id));
            
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,CategoryId,Note,Amount,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if(transaction.Id == 0)
                    _context.Add(transaction);
                else
                    _context.Update(transaction);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            GetCategories();
            return View(transaction);
        }


        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'AppDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void GetCategories()
        {
            var categories = _context.Categories.ToList();
            Category defCategory = new() { Id = 0, Title = "Choose a Category:" };
            categories.Insert(0, defCategory);
            ViewBag.Categories = categories;
        }

    }
}
