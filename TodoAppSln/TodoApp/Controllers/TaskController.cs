using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.Interfaces;
using TodoApp.ViewModels;

namespace TodoApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITodoService _todo;

        public TaskController(ITodoService todoService)
        {
            _todo = todoService;
        }


        [HttpGet]
        public async Task<IActionResult> RemoveManyTodoItems(ListToDisplayViewModel model)
        {
            var ids = new List<string>();
            foreach (var item in model.ListItems)
            {
                if (item.IsSelected)
                    ids.Add(item.Id);
            }

            await _todo.RemoveMany(ids);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditTask(string id)
        {
            return View();
        }
    }
}
