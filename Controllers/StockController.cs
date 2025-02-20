using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _repository;
        public StockController(ApplicationDBContext applicationDBContext, IStockRepository stockRepository)
        {
            _repository = stockRepository;
            _context = applicationDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var stocks = await _repository.GetAllAsync();
            var stock = stocks.Select(s=>s.ToStockDto());
            return Ok(stock);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _repository.GetByIdAsync(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDtos createStockDtos)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = createStockDtos.ToStockFromCreateDto();
            await _repository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateRequestDto){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await  _repository.UpdateAsync(id,updateRequestDto);
            if(stockModel == null){
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _repository.DeleteAsync(id);
            if(stockModel == null){
                return NotFound();
            }
            return NoContent();
        }
    }
}