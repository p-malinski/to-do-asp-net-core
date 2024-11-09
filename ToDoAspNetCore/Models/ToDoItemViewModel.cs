using System.ComponentModel.DataAnnotations;

namespace ToDoAspNetCore.Models
{
    public class ToDoItemViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }
    }
}
