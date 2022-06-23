using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TodoApp.Services.Interfaces;
using TodoApp.ViewModels;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITodoService _todo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, ITodoService todoService, IMapper mapper, IConfiguration configure)
        {
            _logger = logger;
            _todo = todoService;
            _mapper = mapper;
            _config = configure;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page, int size)
        {
            // set defaults
            var offSet = page > 0 ? (page - 1) : page;
            var rows = size > 0 ? size : Convert.ToInt32(_config.GetSection("Pagination:default-size").Value);

            var list = await _todo.GetTodoItems(offSet, rows);

            var listToReturn = new ListToDisplayViewModel();
            if (list.Count > 0)
            {
                // map
                listToReturn.ListItems = _mapper.Map<List<TodoItemViewModel>>(list);
            }

            return View(listToReturn);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
