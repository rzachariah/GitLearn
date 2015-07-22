
using Xunit;
using HelloWorld;

namespace MyFirstDnxUnitTests
{
    public class Program
	{		
		[Fact]
		public void PassingTest()
		{
			Assert.Equal(true, new TestUtility().ReturnTrue());
		}
	}
}