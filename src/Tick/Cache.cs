using System;
using System.Collections.Generic;
using Tick.Domain;

namespace Tick.Cache
{
	public interface IPositionCache
	{
		IEnumerable<Position> GetAll();
	}
	public class PositionCache : IPositionCache
	{
		private List<Position> _positions;
		public PositionCache(ISymbolCache symbolCache)
		{
			_positions = new List<Position>();
			var random = new Random(); 
			for(int i = 0; i < 100; i++)
			{
				_positions.Add(new Position(){Id = i + 1, Amount = random.Next(1, 100), Symbol = symbolCache.GetNext()});	
			}
		}
		
		public IEnumerable<Position> GetAll()
		{
			return _positions;
		}
	}
	
	public interface ISymbolCache
	{
		string GetNext();
		
		IEnumerable<string> GetAll();
	}
	
	public class SymbolCache : ISymbolCache
	{
		private List<string> _symbols = new List<string>(){ "IBM", "CSCO", "MSFT", "AAPL", "EZE", "FB", "HP", "HD", "EC", "NDS" };
		private readonly Random _random = new Random();
		public string GetNext()
		{
			
			lock(_symbols)
				return _symbols[_random.Next(0, _symbols.Count)];
		}
		
		public IEnumerable<string> GetAll()
		{
			return _symbols;
		}
	}
	
	public interface IPriceCache
	{
		IEnumerable<Price> GetAll();
	}
	
	public class PriceCache : IPriceCache
	{
		private readonly ISymbolCache _symbolCache;
		public PriceCache(ISymbolCache symbolCache)
		{
			_symbolCache = symbolCache;
		}
		public IEnumerable<Price> GetAll()
		{
			var random = new Random();
			var result = new List<Price>();
			foreach(var symbol in _symbolCache.GetAll())
			{
				result.Add(new Price(){Value = random.Next(1, 10), Symbol = symbol});
			}
			return result;
		}
	}
}