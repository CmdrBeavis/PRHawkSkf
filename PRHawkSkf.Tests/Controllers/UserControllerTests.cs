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
		public void InstantiationOf_UserController_ShouldReturnNonNullInstance()
		{
			// Arrange
			var mockGhUserReposSvcs = new Mock<IGhUserReposServices>();

			// Act
			UserController controller = new UserController(mockGhUserReposSvcs.Object);

			// Assert
			Assert.IsNotNull(controller);
		}

		[TestMethod]
		public async Task OnCallOfIndexMethod_OnUserCtrlr_ShouldReturnViewResult()
		{
			// Arrange
			var mockGhUserReposSvcs = new Mock<IGhUserReposServices>();
			UserController controller = new UserController(mockGhUserReposSvcs.Object);

			// Act
			ViewResult result = await controller.Index("username") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public async Task IfCallOfHydrateMethod_FromWithinIndex_Throws_ItsHandledAndReThrown()
		{
			// Arrange
			string expectedExceptionMessage = "Test Exception";
			string receivedExceptionMessage = "";

			var mockGhUserReposSvcs = new Mock<IGhUserReposServices>();
			mockGhUserReposSvcs.Setup(o => o.HydrateGhUserReposDisplayVm(
				It.IsAny<string>(),
				It.IsAny<bool>())).ThrowsAsync(new Exception(expectedExceptionMessage));

			UserController controller = new UserController(mockGhUserReposSvcs.Object);
			ViewResult result;

			// Act
			try
			{
				result = await controller.Index("username") as ViewResult;
			}
			catch (Exception exception)
			{
				receivedExceptionMessage = exception.Message;
			}

			// Assert
			Assert.AreEqual(expectedExceptionMessage, receivedExceptionMessage);
		}
	}
}
