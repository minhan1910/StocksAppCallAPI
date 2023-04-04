using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnService _finhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public HomeController(FinnService finhubService, 
                              IOptions<TradingOptions> tradingOptions)
        {
            _finhubService = finhubService;
            _tradingOptions = tradingOptions;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol is null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            string? stockSymbol = _tradingOptions.Value.DefaultStockSymbol;
            
            Dictionary<string, object>? responseDictionary = await _finhubService.GetStockPriceQuote(stockSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),    
                OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),    
            };

            return View(stock);
        }
    }
}
