using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Icon { get; set; }
        [Required]
        public string Type { get; set; } = "Expense";
    }
}
