using System;
using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.ViewModels
{
    public class TodoItemViewModel
    {
        public string Id { get; set; }
        public string Task { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsSelected { get; set; }
    }
}
