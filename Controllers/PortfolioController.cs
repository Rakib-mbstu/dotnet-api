using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolio;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepo = stockRepository; 
            _portfolio = portfolioRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var UserName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(UserName);
            var userPortfolio = await _portfolio.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromBody] string symbol)
        {
            var UserName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(UserName);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if(stock == null)
            {
                return BadRequest("Stock not found");
            }
            var userPortfolio = await _portfolio.GetUserPortfolio(appUser);
            if(userPortfolio.Any( e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Can not add same stock to portfolio");
            }
            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            await _portfolio.CreatePortfolio(portfolio);
            if(portfolio == null)
                return StatusCode(500," Could not create");
            return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio([FromBody] string symbol)
        {
            var UserName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(UserName);
            var userPortfolio = await _portfolio.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());
            if(filteredStock.Count()==1)
            {
                await _portfolio.DeletePortfolio(appUser, symbol);
            }
            else{
                return BadRequest("Stock is not in your portfolio");
            }
            return Ok();

        }
    }
}