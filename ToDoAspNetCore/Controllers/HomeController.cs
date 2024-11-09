using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoAspNetCore.Extensions;
using ToDoAspNetCore.Models;
using ToDoAspNetCore.Persistence;

namespace ToDoAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var toDoItems = await _context.ToDoItems.ToListAsync();

            return View(new ToDoPageViewModel { Items = toDoItems.ToViewModelList() });
        }

        public IActionResult ToDoForm()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddToDoItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToDoItem([FromBody] UpdateToDoItemModel updatedToDoItem)
        {
            var dbItem = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == updatedToDoItem.Id);

            if (dbItem is null)
                return NotFound();

            if (updatedToDoItem.IsDone.HasValue)
                dbItem.IsDone = updatedToDoItem.IsDone.Value;

            if (updatedToDoItem.Text is not null)
                dbItem.Text = updatedToDoItem.Text;

            if (updatedToDoItem.DueDate.HasValue)
                dbItem.DueDate = updatedToDoItem.DueDate.Value;

            await _context.SaveChangesAsync();

            return new OkObjectResult(dbItem);
        }

        [HttpPost]
        public IActionResult DeleteToDoItem()
        {
            return View();
        }
    }
}
