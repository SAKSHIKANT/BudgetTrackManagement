using InternalBudgetTracker.Data;
using InternalBudgetTracker.DTOs;
using InternalBudgetTracker.Enum;
using InternalBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InternalBudgetTracker.Services
{
    public class ExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }
        public string CreateExpense(ExpenseCreateDTO dto, ClaimsPrincipal user)
        {
            // 1️⃣ Token se user nikaalo
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = user.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                throw new Exception("Invalid token");

            // 2️⃣Sirf EMPLOYEE allow
            if (roleClaim.Value != "Employee")
                throw new Exception("Only employee can create expense");

            var employeeId = int.Parse(userIdClaim.Value);

            // 3️⃣ Expense create
            var expense = new Expense
            {
                Description = dto.Description,
                Amount = dto.Amount,
                BudgetId = dto.BudgetId,
                EmployeeId = employeeId,   //  SAME as budget logic
                Status = ExpenseStatus.Pending,
                SubmittedDate = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            // 4️⃣ ExpenseApproval create
            var approval = new ExpenseApproval
            {
                ExpenseId = expense.ExpenseId,
                ManagerId = dto.ManagerId,
                Status = ExpenseStatus.Pending,
                StartDate = DateTime.UtcNow
            };

            _context.ExpenseApprovals.Add(approval);
            _context.SaveChanges();

            return "Expense created successfully";
        }

        //Service to get expenses
        public object GetExpenses(int? expenseId)
        {
            var query = _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Budget)
                .AsQueryable();

            // GET BY ID
            if (expenseId.HasValue)
            {
                var expense = query
                    .FirstOrDefault(e => e.ExpenseId == expenseId.Value);

                if (expense == null)
                    throw new Exception("Expense not found");

                return expense; // single object
            }

            // GET ALL
            return _context.Expenses.ToList();
        }


        public string UpdateExpense(int expenseId, ExpenseUpdateDTO dto, ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expense = _context.Expenses.FirstOrDefault(e =>
                e.ExpenseId == expenseId &&
                e.EmployeeId == userId &&
                //e.Status == ExpenseStatus.Pending 
                e.EndDate == null
            );

            if (expense == null)
                throw new Exception("Expense not editable");

            expense.Description = dto.Description ?? expense.Description;
            expense.Amount = dto.Amount ?? expense.Amount;
            expense.UpdatedDate = DateTime.UtcNow;

            _context.SaveChanges();
            return "Expense updated successfully";
        }
        public string DeleteExpense(
            int expenseId,
            ClaimsPrincipal user)
        {
            int employeeId = int.Parse(
                user.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var expense = _context.Expenses.FirstOrDefault(e =>
                e.ExpenseId == expenseId &&
                e.EmployeeId == employeeId &&
                e.Status == ExpenseStatus.Pending &&
                e.EndDate == null
            );

            if (expense == null)
                throw new Exception(
                    "Expense not found OR not pending OR you are not the owner"
                );

            // SOFT DELETE
            expense.EndDate = DateTime.UtcNow;
            expense.UpdatedDate = DateTime.UtcNow;

            _context.SaveChanges();
            return "Expense deleted successfully (soft delete)";
        }
    }
}
 


    
