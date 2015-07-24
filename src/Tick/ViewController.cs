using System;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using Tick.Response;
using Tick.Repository;

namespace Tick.Controllers
{
	[Route("api/[controller]")]
	public class ViewController : Controller
	{
		private IViewRepository _viewRepository;
		public ViewController(IViewRepository viewRepository)
		{
			_viewRepository = viewRepository;
		}
		[HttpGet]
		public IEnumerable<ViewResponse> GetAll()
		{
			return _viewRepository.GetPositionView();
		}
		
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			if(string.Equals(id, "bysecview", StringComparison.CurrentCultureIgnoreCase))
				return new ObjectResult(_viewRepository.GetBySecView());
			return HttpNotFound();
		}		
	}
}