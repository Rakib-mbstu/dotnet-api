using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock){
            return new StockDto
            {
                Id = stock.Id,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                Industry = stock.Industry 
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockDtos createStockDtos){
            return new Stock
            {
                Symbol = createStockDtos.Symbol,
                CompanyName = createStockDtos.CompanyName,
                Purchase = createStockDtos.Purchase,
                LastDiv = createStockDtos.LastDiv,
                MarketCap = createStockDtos.MarketCap,
                Industry = createStockDtos.Industry,
            };
        }
    }
}