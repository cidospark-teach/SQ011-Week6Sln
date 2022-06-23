using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magazine011.Data.Repository;
using TodoApp.Models;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly IRepository _repo;
        public int _rowsCount { get; private set; }
        private string TableName = "TodoTbl";

        public TodoService(IRepository repository)
        {
            _repo = repository;
            CountRows(TableName).Wait();
        }

        public Task<List<TodoItem>> GetTodoItems(int offSet, int size)
        {
            if(_rowsCount > 0)
            {
                var res = _repo.FetchData($"SELECT * FROM {TableName} ORDER BY Id, UpdatedOn OFFSET {offSet} ROWS FETCH NEXT {size} ROWS ONLY");
            
                if (res.HasRows)
                {
                    var results = new List<TodoItem>();
                    while (res.Read())
                    {
                        results.Add(
                            new TodoItem()
                            {
                                Id = res.GetString(res.GetOrdinal("Id")),
                                Task = res.GetString(res.GetOrdinal("Task")),
                                Completed = res.GetBoolean(res.GetOrdinal("Completed")),
                                CreatedOn = res.GetDateTime(res.GetOrdinal("CreatedOn")),
                                UpdatedOn = res.GetDateTime(res.GetOrdinal("UpdatedOn")),
                                ExpiresOn = res.GetDateTime(res.GetOrdinal("ExpiresOn"))
                            }
                        );
                    }
                    return Task.Run(() => results);
                }
            }

            return Task.Run(() => new List<TodoItem>());
           
        }

        public async Task<bool> RemoveMany(List<string> ids)
        {
            var stmt = $"DELETE FROM {TableName} ";
            var param = new List<string>(ids);
            var joined = "";

            if(ids.Count > 0)
                joined = string.Join(",", param);

            stmt += $"WHERE Id IN ({joined})";

            _repo.ExecuteQuery(stmt);

            await CountRows(TableName);
            return true;

        }

        public async Task<int> CountRows(string tbl)
        {
            var rs = _repo.FetchData($"SELECT COUNT(Id) FROM {TableName}");
            var count = 0;
            while(await rs.ReadAsync())
            {
                count = rs.GetInt32(0);
            }
            _rowsCount = count;
            return count;
        }
    }
}
