using System;
using System.Collections.Generic;

namespace TodoApp.ViewModels
{
    public class ListToDisplayViewModel
    {
        public List<TodoItemViewModel> ListItems { get; set; }

        public ListToDisplayViewModel()
        {
            ListItems = new List<TodoItemViewModel>();
        }
    }
}
