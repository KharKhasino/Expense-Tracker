using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string? Note { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
