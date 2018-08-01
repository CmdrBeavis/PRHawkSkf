using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using PRHawkSkf.Controllers;
using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Controllers
{
	[TestClass]
	public class UserControllerTests
	{
		[TestMethod]
		public async Task OnCallOfIndexMethod_OsUserCtrlr_ShouldReturnViewResult()
		{
			// Arrange
			var mockGhUserReposSvcs = new Mock<IGhUserReposServices>();
			UserController controller = new UserController(mockGhUserReposSvcs.Object);

			// Act
			ViewResult result = await controller.Index("username") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

	}
}
