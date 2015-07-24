using Tick.Cache;
using Tick.Response;
using System.Collections.Generic;
using System.Linq;

namespace Tick.Repository
{
	public interface IViewRepository
	{
		IEnumerable<ViewResponse> GetPositionView();
		
		IEnumerable<ViewResponse> GetBySecView();  
	}
	
	public class ViewRepository : IViewRepository
	{
		private readonly IPositionCache _positionCache;
		private readonly IPriceCache _priceCache;
		public ViewRepository(IPositionCache positionCache, IPriceCache priceCache)
		{
			_positionCache = positionCache;
			_priceCache = priceCache;
		}
		
		public IEnumerable<ViewResponse> GetPositionView()
		{
			var joinQuery = from position in _positionCache.GetAll()
				join price in _priceCache.GetAll() on position.Symbol equals price.Symbol
				select new {Symbol = position.Symbol, Amount = position.Amount, Price = price.Value, MarketValue = position.Amount * price.Value};
			 
			 
			var result = new List<ViewResponse>();
			foreach(var gr in joinQuery)
			{				
				var rowResponse = new ViewResponse();
				var values = new Dictionary<string, string>();
			  	rowResponse.Id =  gr.Symbol;
			  	rowResponse.Amount = gr.Amount;
			  	rowResponse.Price = gr.Price;
			  	rowResponse.MarketValue = gr.MarketValue;
			  	result.Add(rowResponse);
			}
			return result;
		}
		
		public IEnumerable<ViewResponse> GetBySecView()
		{
			var joinQuery = from position in _positionCache.GetAll()
				join price in _priceCache.GetAll() on position.Symbol equals price.Symbol
				select new {Symbol = position.Symbol, Amount = position.Amount, Price = price.Value, MarketValue = position.Amount * price.Value};
			 
			 
			var groupBy = joinQuery.GroupBy( r => r.Symbol);
			var result = new List<ViewResponse>();
			foreach(var gr in groupBy)
			{
				var grResult = gr.Aggregate(new {Symbol = string.Empty, Amount = 0, Price = 0, MarketValue = 0}, (curr, next)=> new {Symbol = next.Symbol, Amount = curr.Amount + next.Amount, Price = next.Price, MarketValue = curr.MarketValue + next.MarketValue});
				var rowResponse = new ViewResponse();
				var values = new Dictionary<string, string>();
			  	rowResponse.Id =  grResult.Symbol;
			  	rowResponse.Amount = grResult.Amount;
			  	rowResponse.Price = grResult.Price;
			  	rowResponse.MarketValue = grResult.MarketValue;
			  	result.Add(rowResponse);
			}
			return result;
		}
	}
}