using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using PRHawkSkf.GitHubApiRepo;
using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class GitHubPullReqsTests
	{
		/// <summary>
		/// Tests instantiation of the <see cref="GitHubPullReqs"/> object.
		/// </summary>
		[TestMethod]
		public void WhenInstantiatedWithValidParam_ANonNullGitHubPullReqsInstance_IsCreated()
		{
			// Arrange
			var mockGhApiRepoHelper = new Mock<IGitHubApiRepoHelpers>();

			// Act
			var ghApiCallServicesInstance = new GitHubPullReqs(mockGhApiRepoHelper.Object);

			// Assert
			Assert.IsNotNull(ghApiCallServicesInstance);
			Assert.IsInstanceOfType(ghApiCallServicesInstance, typeof(IGitHubPullReqs));
		}
	}
}
