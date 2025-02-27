using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Migrations;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext dBContext)
        {
            _context = dBContext;
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync( x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() ==  symbol.ToLower());
            if(portfolio == null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where( u => u.AppUserId == user.Id)
            .Select( stock=> new Stock
            {
                Id = stock.Stock.Id,
                Symbol = stock.Stock.Symbol,
                Industry = stock.Stock.Industry,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                MarketCap = stock.Stock.MarketCap,

            }).ToListAsync();
        }
    }
}