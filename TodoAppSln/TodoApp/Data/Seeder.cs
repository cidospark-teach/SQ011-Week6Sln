using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Magazine011.Data.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class Seeder
    {
        private readonly IRepository _repo;
        private readonly IConfiguration _config;

        public Seeder(IRepository repository, IConfiguration configure)
        {
            _repo = repository;
            _config = configure;
        }

        public async Task SeedMe()
        {
            var tableName = "TodoTbl";
            var tblFields = "Id, Task, Completed, CreatedOn, UpdatedOn, ExpiresOn";

            var splitted = tblFields.Split(",");
            #region sql statements
            var createTodoTbl = $@"CREATE TABLE {tableName} (
                                    {splitted[0].Trim()} VARCHAR(255) NOT NULL PRIMARY KEY,
                                    {splitted[1].Trim()} VARCHAR(255) NOT NULL,
                                    {splitted[2].Trim()} BIT,
                                    {splitted[3].Trim()} DATETIME2,
                                    {splitted[4].Trim()} DATETIME2,
                                    {splitted[5].Trim()} DATETIME2,
                                 )";
            #endregion

            // ensure DB and table is created
            await EnsureDBCreated();
            await EnsureTableCreated(createTodoTbl, tableName);

            // get row count if table contains records
            var rowCount = 0;
            var reader = _repo.FetchData($"SELECT COUNT(Id) FROM {tableName}");
            if(await reader.ReadAsync())
            {
                rowCount = reader.GetInt32(0);
            }

            if (rowCount < 1)
            {
                // path to the dummy data
                var path = _config.GetSection("ConStrs:FilePath").Value;

                // read dummy data and convert to c# object
                var data = File.ReadAllText(path);
                var listOfItems = JsonConvert.DeserializeObject<List<TodoItem>>(data);

                foreach (var item in listOfItems)
                {
                    var rnd = new Random();
                    var stmt = $@"INSERT INTO {tableName} ({tblFields}) VALUES('{item.Id}', '{item.Task}',
                                '{item.Completed}', '{item.CreatedOn}', '{DateTime.Now}', '{DateTime.Now.AddDays(rnd.Next(1, 3))}')";

                    _repo.ExecuteQuery(stmt);
                }
            }


        }

        private Task EnsureDBCreated()
        {
            var stmtToEnsureDBIsCreated = @"IF (EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name = 'Week6Task'))
                                                  BEGIN
                                                    SELECT 1
                                                  END
                                                  ELSE
                                                  BEGIN
                                                    CREATE DATABASE Week6Task;
                                                    SELECT 0
                                                  END";
            _repo.BuildDBObject(stmtToEnsureDBIsCreated);
            return Task.CompletedTask;
        }

        private Task EnsureTableCreated(string stmt, string tableName)
        {
            var stmtToEnsureTableIsCreated = @$"IF (EXISTS(SELECT name FROM sys.tables WHERE name = '{tableName}'))
                                                  BEGIN
                                                    SELECT 1
                                                  END
                                                  ELSE
                                                  BEGIN
                                                    {stmt}
                                                    SELECT 0
                                                  END";
            _repo.ExecuteQuery(stmtToEnsureTableIsCreated);
            return Task.CompletedTask;
        }
    }
}
