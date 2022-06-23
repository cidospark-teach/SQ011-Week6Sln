using System;
using AutoMapper;
using TodoApp.Models;
using TodoApp.ViewModels;

namespace TodoApp.Settings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TodoItem, TodoItemViewModel>();
        }
    }
}
