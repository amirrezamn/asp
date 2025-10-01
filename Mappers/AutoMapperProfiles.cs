using asp.Models;
using asp.Models.Dtos;
using AutoMapper;

namespace asp.Mappers;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<TodoItem, TodoForCreate>();
        CreateMap<TodoForCreate, TodoItem>();
    }
}