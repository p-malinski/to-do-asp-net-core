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

        public static ToDoItemModel ToDbModel(this UpdateToDoItemModel updateToDoItem)
        {
            return new ToDoItemModel
            {
                Id = updateToDoItem.Id,
                Text = updateToDoItem.Text is not null ? updateToDoItem.Text : "",
                IsDone = updateToDoItem.IsDone.HasValue ? updateToDoItem.IsDone.Value : false,
                DueDate = updateToDoItem.DueDate.HasValue ? updateToDoItem.DueDate.Value : DateTime.Now,
            };
        }
    }
}
