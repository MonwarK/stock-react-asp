using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;

namespace api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = _context.Stock.ToList().Select(s => s.ToStockDto()); 
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var stock = _context.Stock.Find(id);
        return stock is null ? NotFound() : Ok(stock);
    }
}
