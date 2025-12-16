using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Stock;

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

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        _context.Stock.Add(stockModel);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

        if (stockModel == null) return NotFound();

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        _context.SaveChanges();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

        if (stockModel == null) return NotFound();

        _context.Stock.Remove(stockModel);
        _context.SaveChanges();

        return NoContent();
    }
}
