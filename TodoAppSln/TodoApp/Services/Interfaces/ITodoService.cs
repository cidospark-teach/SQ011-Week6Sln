using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services.Interfaces
{
    public interface ITodoService
    {
        public int _rowsCount { get; }
        Task<List<TodoItem>> GetTodoItems(int offSet, int size);
        Task<int> CountRows(string tbl);
        Task<bool> RemoveMany(List<string> ids);
    }
}
