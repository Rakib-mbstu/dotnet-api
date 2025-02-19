using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }
        public Task<List<Stock>> GetAllAsync()
        {
            return _context.Stock.Include(c=>c.Comments).ToListAsync();
        }
        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c=>c.Comments).FirstOrDefaultAsync( i => i.Id == id);
        }
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateRequestDto)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if( stock == null)
            {
                return null;
            }
            stock.Symbol = updateRequestDto.Symbol;
            stock.LastDiv = updateRequestDto.LastDiv;
            stock.CompanyName = updateRequestDto.CompanyName;
            stock.Purchase = updateRequestDto.Purchase;
            stock.Industry = updateRequestDto.Industry;
            stock.MarketCap = updateRequestDto.MarketCap; 
            await _context.SaveChangesAsync();
            return stock;
        }
        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }
        public async Task<Stock?> DeleteAsync(int id)
        {
             var stockModel = await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
            if(stockModel == null){
                return null;
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<bool> StockExists(int id)
        {
           return await _context.Stock.AnyAsync( x => x.Id == id );
        }
    }
}