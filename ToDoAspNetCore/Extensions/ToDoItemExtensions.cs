using ToDoAspNetCore.Models;

namespace ToDoAspNetCore.Extensions
{
    public static class ToDoItemExtensions
    {
        public static ToDoItemViewModel ToViewModel(this ToDoItemModel toDoItem)
        {
            return new ToDoItemViewModel
            {
                Id = toDoItem.Id,
                Text = toDoItem.Text,
                IsDone = toDoItem.IsDone,
                DueDate = toDoItem.DueDate
            };
        }

        public static List<ToDoItemViewModel> ToViewModelList(this IEnumerable<ToDoItemModel> orders)
        {
            return orders.Select(order => order.ToViewModel()).ToList();
        }
    }
}
