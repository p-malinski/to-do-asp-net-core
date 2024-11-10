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

        public async Task<IActionResult> Index(DateOnly? dateFilter)
        {
            var toDoItemsQuery = _context.ToDoItems.AsQueryable();
            ViewBag.DateFilter = dateFilter;

            if (dateFilter.HasValue)
            {
                var date = dateFilter.Value;
                toDoItemsQuery = toDoItemsQuery.Where(i => i.DueDate.Year == date.Year && i.DueDate.Month == date.Month && i.DueDate.Day == date.Day);
            }

            var toDoItems = await toDoItemsQuery.ToListAsync();

            return View(new ToDoPageViewModel { Items = toDoItems.ToViewModelList() });
        }

        [HttpGet]
        public IActionResult CreateToDoItemForm()
        {
            return View(new ToDoItemViewModel { Id = 0, DueDate = DateTime.Now.AddDays(1), IsDone = false, Text = "" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateToDoItemForm(int id)
        {
            var dbItem = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);

            if (dbItem is null)
                return Redirect("/");

            return View(dbItem.ToViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> AddToDoItem([FromForm] UpdateToDoItemModel newToDoItem)
        {
            if (newToDoItem.Id != 0 || newToDoItem.Text is null || !newToDoItem.IsDone.HasValue || !newToDoItem.DueDate.HasValue)
                return Redirect("/");

            await _context.ToDoItems.AddAsync(newToDoItem.ToDbModel());
            await _context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToDoItem([FromForm] UpdateToDoItemModel updatedToDoItem)
        {
            var toDoItem = await UpdateTodoItem(updatedToDoItem);
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToDoItemJson([FromBody] UpdateToDoItemModel updatedToDoItem)
        {
            var toDoItem = await UpdateTodoItem(updatedToDoItem);

            if (toDoItem is null)
                return NotFound();

            return new OkObjectResult(toDoItem);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteToDoItem([FromForm] int id)
        {
            _context.ToDoItems.Remove(_context.ToDoItems.Single(i => i.Id == id));
            await _context.SaveChangesAsync();

            return Redirect("/");
        }

        private async Task<ToDoItemModel?> UpdateTodoItem(UpdateToDoItemModel updatedToDoItem)
        {
            var dbItem = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == updatedToDoItem.Id);

            if (dbItem is null)
                return null;

            if (updatedToDoItem.IsDone.HasValue)
                dbItem.IsDone = updatedToDoItem.IsDone.Value;

            if (updatedToDoItem.Text is not null)
                dbItem.Text = updatedToDoItem.Text;

            if (updatedToDoItem.DueDate.HasValue)
                dbItem.DueDate = updatedToDoItem.DueDate.Value;

            await _context.SaveChangesAsync();

            return dbItem;
        }
    }
}
