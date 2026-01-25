using Microsoft.AspNetCore.Mvc;

namespace InternalBudgetTracker.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
