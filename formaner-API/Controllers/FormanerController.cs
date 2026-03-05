using formaner_API.Data;
using formaner_API.Models;
using Microsoft.AspNetCore.Mvc;
using formaner_API.Data;
using Microsoft.EntityFrameworkCore;

namespace formaner_API.Controllers;

[ApiController]
[Route("[controller]")]
public class FormanerController : ControllerBase
{
    
    private readonly DbService _dbContext; 
   
    public FormanerController(DbService dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public Formaner[] GetFormaners()
    {
        Formaner[] formaners = _dbContext.Formaners.ToArray();
        return formaners;
    }

   

   
    [HttpPost]
    public IActionResult Create(Formaner newFormaner)
    {
        _dbContext.Formaners.Add(newFormaner);
        _dbContext.SaveChanges();
        return Ok(newFormaner);
    }
    
[HttpPost("Edit")] 
    public IActionResult Edit(Formaner formaner)
    {
        _dbContext.Formaners.Update(formaner);
        _dbContext.SaveChanges();
        return Ok(formaner);
    }

    [HttpDelete("{ID}")]
    public IActionResult Delete(int ID ) 
    {
        var formaner = _dbContext.Formaners.Find(ID);

        _dbContext.Formaners.Remove(formaner);
        _dbContext.SaveChanges();
        return Ok(formaner);
    }
    
}