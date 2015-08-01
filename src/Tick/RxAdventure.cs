using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Tick.Response;
using Tick.Hubs;

namespace Tick.Rx
{
	public class StreamProvider
    {
        private readonly ViewHub _hub;
        public StreamProvider(ViewHub hub)
        {
            _hub = hub;
        }
        
        public void Initialize()
        {
            var prices = Observable.Interval(TimeSpan.FromMilliseconds(300))
                           .Select(_ => Price.CreateNext())
                           .Publish()
                           .RefCount();

            var trades = Observable.Interval(TimeSpan.FromMilliseconds(3000))
                                   .Select(_ => Trade.CreateNext())
                                   .Publish()
                                   .RefCount();

            var positions = trades.GroupBy(t => t.Symbol)
                                        .Select(g => g.Scan(new Position(g.Key, 0), (current, next) => new Position(g.Key, current.Amount + next.Amount)))
                                        .Merge()
                                        .Publish()
                                        .RefCount();

            var valuations = positions.GroupBy(p => p.Symbol)
                                      .Select(g => new { Key = g.Key, PositionStream = g, PriceStream = prices.Where(p => p.Symbol == g.Key) })
                                      .Select(o => o.PositionStream.CombineLatest(o.PriceStream, (pos, price) => new { Symbol = pos.Symbol, Amount = pos.Amount, Price = price.Value, MarketValue = pos.Amount * price.Value }))
                                      .Merge()
                                      .Publish()
                                      .RefCount();

            valuations.Subscribe(r =>
            {
                _hub.Notify(new ViewResponse(){Id = r.Symbol, Amount = r.Amount, Price = r.Price, MarketValue = r.MarketValue});
            });
        }
    }

    public static class SecurityMaster
    {
        private static string[] universe = new string[] { "IBM", "CSCO", "MSFT", "AAPL", "EZE", "FB", "HP", "HD", "EC", "NDS" };

        public static IList<string> SymbolList
        {
            get { return universe.ToList(); }
        }
    }

    public class Price
    {
        private static Random rand = new Random();

        public string Symbol { get; private set; }
        public decimal Value { get; private set; }

        public Price(string symbol, decimal value)
        {
            Symbol = symbol;
            Value = value;
        }

        public static Price CreateNext()
        {
            return new Price(SecurityMaster.SymbolList[rand.Next(0, SecurityMaster.SymbolList.Count())], rand.Next(100, 10000) / 100m);
        }
    }

    public class Trade
    {
        private static Random rand = new Random();

        public string Symbol { get; set; }
        public decimal Amount { get; set; }

        public Trade(string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        public static Trade CreateNext()
        {
            return new Trade(SecurityMaster.SymbolList[rand.Next(0, SecurityMaster.SymbolList.Count())], rand.Next(1, 5) * 3);
        }
    }

    public class Position
    {
        public string Symbol { get; private set; }
        public decimal Amount { get; private set; }

        public Position(string symbol, decimal amount)
        {
            Symbol = symbol;
            Amount = amount;
        }
    }
}