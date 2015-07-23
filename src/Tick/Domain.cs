namespace Tick.Domain
{
	public class Position
	{
		public int Id{get;set;}
		
		public int Amount{get;set;}
		
		public string Symbol{get;set;}
	}
	
	public class Price
	{		
		public int Value{get;set;}
		
		public string Symbol{get;set;}
	}
}