using formaner_API.Data;
using formaner_API.Models;
using Microsoft.AspNetCore.Mvc;
using formaner_API.Data;

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
    public void PostForman(Formaner formaner)
    {
        _dbContext.Add(formaner);
        _dbContext.SaveChanges();
    }
}