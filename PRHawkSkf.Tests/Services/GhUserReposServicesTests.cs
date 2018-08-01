using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;
using PRHawkSkf.GitHubApiRepoInterfaces;
using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class GhUserReposServicesTests
	{
		private Mock<IHttpClientProvider> mockHttpClientProvider;
		private Mock<IHttpClientAuthorizeConfigurator> mockHCAuthConfigr;
		private Mock<IGitHubRepos> mockGHReposSvc;
		private Mock<IGitHubPullReqs> mockGHPRsSvc;
		private GitHubApiCallServices apiCallServices;

		#region Additional test attributes

		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize]
		//public static void MyClassInitialize(TestContext testContext)
		//{ }

		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		[TestInitialize()]
		public void MyTestInitialize()
		{
			mockHttpClientProvider = new Mock<IHttpClientProvider>();

			mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			mockGHReposSvc = new Mock<IGitHubRepos>();

			// The HydrateGhUserReposDisplayVm method is going to call the
			// following, so I need a mocked version in order to test the
			// 'private filter' functionality
			mockGHReposSvc.Setup(o => o.GetGitHubRepos(
				It.IsAny<HttpClient>(),
				It.IsAny<string>())).ReturnsAsync(() => new List<GhUserRepo>
			{
				new GhUserRepo {name = "test1", @private = false},
				new GhUserRepo {name = "test2", @private = true},
				new GhUserRepo {name = "test3", @private = false}
			});

			mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);
		}

		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//

		#endregion

		[TestMethod]
		public void GhUserReposServices_InstantiationWithProperParam_ShouldPass()
		{
			// arrange
			var ghApiCallServices = new Mock<IGitHubApiCallServices>();

			// act
			var ghUserReposServices = new GhUserReposServices(ghApiCallServices.Object);

			// assert
			Assert.IsNotNull(ghUserReposServices);
			Assert.IsInstanceOfType(ghUserReposServices, typeof(GhUserReposServices));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GhUserReposServices_InstantiationWithNullParam_ShouldThrow()
		{
			// act
			var ghUserReposServices = new GhUserReposServices(null);

			// assert
			// should have thrown
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task HydrateGhUserReposDisplayVm_CallWithEmptyOrWhiteSpGhUsernameParam_ShouldThrow()
		{
			// Arrange 
			var mockedApiCallServices = new Mock<IGitHubApiCallServices>();

			// Act
			var classInstance = new GhUserReposServices(mockedApiCallServices.Object);
			var callResult = await classInstance.HydrateGhUserReposDisplayVm(" ");

			// Assert
			// should have thrown
		}

		[TestMethod]
		public async Task HydrateGhUserReposDisplayVm_NoInclPrivateRepos_ShouldPass()
		{
			// Arrange 
			var ghUserReposSvcs = new GhUserReposServices(apiCallServices);

			// Act
			GhUserReposDisplayVm callResult =
				await ghUserReposSvcs.HydrateGhUserReposDisplayVm("username");

			// if 'returnPrivateRepos' is false, I should get 2 back
			Assert.AreEqual(2, callResult.Repositories.Count);
		}

		[TestMethod]
		public async Task HydrateGhUserReposDisplayVm_InclPrivateRepos_ShouldPass()
		{
			// Arrange 
			var ghUserReposSvcs = new GhUserReposServices(apiCallServices);

			// Act
			GhUserReposDisplayVm callResult =
				await ghUserReposSvcs.HydrateGhUserReposDisplayVm("username", true);

			// if 'returnPrivateRepos' is true, I should get 3 back
			Assert.AreEqual(3, callResult.Repositories.Count);
		}

		[TestMethod]
		public async Task HydrateGhUserReposDisplayVm_IfNoReposAfterFilter_ShouldGetSimplyInitdRetClass()
		{
			// Arrange
			mockGHReposSvc.Setup(o => o.GetGitHubRepos(
				It.IsAny<HttpClient>(),
				It.IsAny<string>())).ReturnsAsync(() => new List<GhUserRepo>
			{
				new GhUserRepo {name = "test1", @private = true},
				new GhUserRepo {name = "test2", @private = true}
			});

			apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var ghUserReposSvcs = new GhUserReposServices(apiCallServices);

			// Act
			GhUserReposDisplayVm callResult =
				await ghUserReposSvcs.HydrateGhUserReposDisplayVm("username");

			Assert.IsInstanceOfType(callResult, typeof(GhUserReposDisplayVm));
			Assert.AreEqual("username", callResult.GitHubUsername);
			Assert.IsNotNull(callResult.Repositories);
			Assert.IsInstanceOfType(callResult.Repositories, typeof(List<RepoListItem>));
			Assert.AreEqual(0, callResult.Repositories.Count);
		}

		[TestMethod]
		public async Task HydrateGhUserReposDisplayVm_MockRepoCallReturnsOne_ShouldGetHydratedViewModel()
		{
			// Arrange
			var ghUserReposSvcs = new GhUserReposServices(apiCallServices);

			// Act
			GhUserReposDisplayVm callResult =
				await ghUserReposSvcs.HydrateGhUserReposDisplayVm("cmdrbeavis");

			// Assert
			Assert.IsInstanceOfType(callResult, typeof(GhUserReposDisplayVm));
			Assert.AreEqual("cmdrbeavis", callResult.GitHubUsername);
			Assert.IsNotNull(callResult.Repositories);
			Assert.IsInstanceOfType(callResult.Repositories, typeof(List<RepoListItem>));
			Assert.AreEqual(2, callResult.Repositories.Count);
			Assert.IsTrue(callResult.Repositories[0].name.Contains("test1"));
			Assert.IsTrue(callResult.Repositories[1].name.Contains("test3"));
		}
	}
}
