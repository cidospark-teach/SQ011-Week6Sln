using System;
namespace TodoApp.Models
{
    public class TodoItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Task { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
