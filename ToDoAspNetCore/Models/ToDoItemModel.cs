namespace ToDoAspNetCore.Models
{
    public class ToDoItemModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }
    }
}
