﻿using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PRHawkSkf.Controllers;


namespace PRHawkSkf.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTests
	{
		[TestMethod]
		public void WhenIndexMethodCalled_NonNullViewResult_ShouldBeReturned()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void WhenAboutMethodCalled_NonNullViewResult_ShouldBeReturned()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(
				"PRHawk SKF is my take on the ServiceNow 'take home coding test' Web application.", 
				result.ViewBag.Message);
		}

		[TestMethod]
		public void WhenContactMethodCalled_NonNullViewResult_ShouldBeReturned()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
