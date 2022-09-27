using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Category")]
        [Range(1,int.MaxValue, ErrorMessage ="Please select a category!")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Note { get; set; }
        [DisplayFormat(DataFormatString ="{0:C0}")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount should exceed Zero!")]
        public int Amount { get; set; }
        [DisplayFormat(DataFormatString ="{0:MMM dd yyy}")]
        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryInfo
        {
            get
            {
                return Category == null ? "" : Category.Icon + " " + Category.Title;
            }
        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
        [NotMapped]
        public string? FormattedDate
        {
            get
            {
                return Date.ToString("MMM dd yyyy");
            }
        }
    }
}
