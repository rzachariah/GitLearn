using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Tick.Response;
using Tick.Cache;

namespace Tick.Controllers
{
	[Route("api/[controller]")]
	public class ViewController : Controller
	{
		private readonly IPositionCache _positionCache;
		private readonly IPriceCache _priceCache;
		public ViewController(IPositionCache positionCache, IPriceCache priceCache)
		{
			_positionCache = positionCache;
			_priceCache = priceCache;
		}
		
		private class ViewResult
		{
			public string symbol{get;set;}
			public int Amount {get;set;}
		}
		[HttpGet]
		public IEnumerable<ViewResponse> GetAll()
		{
			var joinQuery = from position in _positionCache.GetAll()
				join price in _priceCache.GetAll() on position.Symbol equals price.Symbol
				select new {Symbol = position.Symbol, Amount = position.Amount, Price = price.Value, MarketValue = position.Amount * price.Value};
			 
			 
			var groupBy = joinQuery.GroupBy( r => r.Symbol);
			var result = new List<ViewResponse>();
			foreach(var gr in groupBy)
			{
				var grResult = gr.Aggregate(new {Symbol = string.Empty, Amount = 0, Price = 0, MarketValue = 0}, (curr, next)=> new {Symbol = next.Symbol, Amount = curr.Amount + next.Amount, Price = next.Price, MarketValue = curr.MarketValue + next.MarketValue});
				var values = new Dictionary<string, string>();
			  	values.Add("Symbol", grResult.Symbol);
			  	values.Add("Amount", grResult.Amount.ToString());
			  	values.Add("Price", grResult.Price.ToString());
			  	values.Add("MarketValue", grResult.MarketValue.ToString());
			  	result.Add(new ViewResponse(){Values = values});
			}
			return result;
		}
	}
}