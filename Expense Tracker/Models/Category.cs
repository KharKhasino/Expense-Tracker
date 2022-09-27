using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage ="Title is Required!")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? Icon { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string FullCategory
        {
            get
            {
                return this.Icon + "\n" + this.Title;
            }
        }
    }
}
