using InternalBudgetTracker.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalBudgetTracker.Models
{
    [Table("t_Expense")]
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Amount { get; set; }
        public string  Description { get; set; }

        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;

        //  Budget relation
        public int BudgetId { get; set; }
        public Budget Budget { get; set; }

        //  Employee who created expense
        [Column("SubmittedByUserId")]
        public int EmployeeId { get; set; }
        public User Employee { get; set; }

        //   Currently Assigned Manager (IMPORTANT)
        public int AssignedManagerId { get; set; }
        public User AssignedManager { get; set; }

       
    }
}
