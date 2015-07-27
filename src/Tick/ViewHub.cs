using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Tick.Repository;
using Tick.Response;
using Tick.Rx;

namespace Tick.Hubs
{
         [HubName("viewHub")]
        public class ViewHub : Hub
	{
                private readonly IViewRepository _viewRepo;
                private readonly StreamProvider _streamProvider;
        	public ViewHub(IViewRepository viewRepository)
        	{
                        _viewRepo = viewRepository;
                        //Task.Factory.StartNew(Publish);
                        _streamProvider = new StreamProvider(this);
                        _streamProvider.Initialize();
        	}
                
                public override Task OnConnected()
                {
                    Console.WriteLine("Client Connected!");
                    return Task.Delay(1);
                }
        	
        	private void Publish()
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Clients.All.updateStockPrice(_viewRepo.GetBySecView());                        
                    }
                }
        		
        	public IEnumerable<ViewResponse> GetAllStocks()
                {
                    return _viewRepo.GetBySecView();
                }
                
                public void Notify(ViewResponse response)
                {
                        Clients.All.updateStockPrice(new []{response});
                }
	}
}