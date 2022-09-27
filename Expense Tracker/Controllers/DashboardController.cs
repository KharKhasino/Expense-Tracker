using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            DateTime startDate = DateTime.Today.AddDays(-28);
            DateTime endDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions.Include(x => x.Category)
                .Where(y => y.Date >= startDate && y.Date <= endDate).ToListAsync();

            // Total Income
            int totalIncome = selectedTransactions.Where(i => i.Category?.Type == "Income").Sum(s => s.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0");

            // Total Expense
            int totalExpense = selectedTransactions.Where(i => i.Category?.Type == "Expense").Sum(s => s.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("C0");

            // Balance 
            int balance = totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0");

            // Chart - Expense by Category
            ViewBag.DoughnutChartData = selectedTransactions.Where(i => i.Category?.Type == "Expense")
                .GroupBy(j => j.CategoryId).Select(k => new
                {
                    categoryDetails = k.First().Category?.Icon + " " + k.First().Category?.Title,
                    amount = k.Sum(x => x.Amount),
                    formattedAmount = k.Sum(x => x.Amount).ToString("C0"),
                }).ToList();

            // Spline Chart - Income vs Expense
            // 1)Income
            List<SplineChartData> IncomeSummary = selectedTransactions.Where(i => i.Category?.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(x => x.Amount)
                }).ToList();

            // 2)Expense
            List<SplineChartData> ExpenseSummary = selectedTransactions.Where(i => i.Category?.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(x => x.Amount)
                }).ToList();

            // Combine Expense & Income
            string[] Days = Enumerable.Range(0, 30)
                .Select(i=>startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into dayExpenseJoined
                                      from expense in dayExpenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category)
                .OrderByDescending(j => j.Date).Take(5)
                .ToListAsync();

            return View();
        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;
        }
    }
}
