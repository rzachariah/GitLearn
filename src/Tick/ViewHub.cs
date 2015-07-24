using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Tick.Repository;
using Tick.Response;

namespace Tick.Hubs
{
         [HubName("viewHub")]
        public class ViewHub : Hub
	{
                private readonly IViewRepository _viewRepo;
        	public ViewHub(IViewRepository viewRepository)
        	{
                        _viewRepo = viewRepository;
        		 Task.Factory.StartNew(Publish);
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
	}
}