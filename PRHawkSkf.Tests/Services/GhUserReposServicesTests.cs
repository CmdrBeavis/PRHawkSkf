using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class GhUserReposServicesTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			// TODO: Don't forget to make some tests!
		}


		[TestMethod]
		public void InstantiationOf_GhUserReposServices_CreatesInstanceOfSame_ShouldPass()
		{
			// Arrange 
			// Act
			var nonNullClassInstance = new GhUserReposServices();

			// Assert
			Assert.IsNotNull(nonNullClassInstance);
			Assert.IsInstanceOfType(nonNullClassInstance, typeof(GhUserReposServices));
		}



	}
}
