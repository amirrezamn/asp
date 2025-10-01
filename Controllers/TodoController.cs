using System.Security.Cryptography.X509Certificates;
using asp.Data;
using asp.Models;
using asp.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoController(DataContext context,IMapper mapper):ControllerBase
{
   private readonly DataContext _context=context;
   private readonly IMapper _mapper;

   [HttpGet]
   public async Task<IActionResult> Todos()
   {
      var todos = await _context.Todos.ToListAsync();
      return Ok(todos);
   }

   [HttpPost]
   public async Task<IActionResult> Add(TodoForCreate todoForCreate)
   {
      var todo = _mapper.Map<TodoItem>(todoForCreate);
      var result = await _context.Todos.AddAsync(todo);
      await _context.SaveChangesAsync();
      return Ok(result.Entity);
   }
}