using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.GitHubApiRepoInterfaces;
using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{


	[TestClass]
	public class GitHubApiCallServicesTests
	{
		[TestMethod]
		public void InstantiationOf_GitHubApiCallServices_WithProperParameters_ShouldPass()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			// Assert
			Assert.IsNotNull(apiCallServices);
			Assert.IsInstanceOfType(apiCallServices, typeof(IGitHubApiCallServices));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InstantiationOf_GitHubApiCallServices_WithAnyNullParameter_ShouldThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			HttpClientAuthorizeConfigurator mockHCAuthConfigr = null;
			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			// Assert
			// Should throw, so nothing here.
		}

		// async Task<List<GhUserRepo>> GetPublicGhUserReposByUsername(string ghUsername)

		[TestMethod]
		public void CallOfGetReposByUsername_ReturnsListOfGhUserRepo_ShouldPass()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();

			// will be calling ghRepos.GetGitHubRepos(httpClient, ghUsername);
			// The method will return an empty collection if there are any 
			// problems, or no public repos are found, so set it up to return
			// a List containing a single item.
			mockGHReposSvc.Setup(o => o.GetGitHubRepos(
				It.IsAny<HttpClient>(),
				It.IsAny<string>())).ReturnsAsync(() => new List<GhUserRepo> {new GhUserRepo()});

			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				apiCallServices.GetPublicGhUserReposByUsername("github");
			var returnedList = taskOfListOfGhUserRepos.Result;

			// Assert
			Assert.IsNotNull(returnedList);
			Assert.IsInstanceOfType(returnedList, typeof(List<GhUserRepo>));
			Assert.AreEqual(1, returnedList.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task CallOfGetReposByUsername_WithEmptyUsername_ShouldThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				await apiCallServices.GetPublicGhUserReposByUsername(" ");

			// Should have thrown.
		}

		[TestMethod]
		[ExpectedException(typeof(HttpRequestException))]
		public async Task IfGetGitHubReposCall_ThrowsHttpRequestException_GetPublicGhUserReposShouldHandleAndThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();

			// will be calling ghRepos.GetGitHubRepos(httpClient, ghUsername);
			// The method will return an empty collection if there are any 
			// problems, or no public repos are found, so set it up to return
			// a List containing a single item.
			mockGHReposSvc.Setup(o => o.GetGitHubRepos(
				It.IsAny<HttpClient>(),
				It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				await apiCallServices.GetPublicGhUserReposByUsername("github");

			// should have thrown
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public async Task IfGetGitHubReposCall_ThrowsPlainException_GetPublicGhUserReposShouldHandleAndThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();

			// will be calling ghRepos.GetGitHubRepos(httpClient, ghUsername);
			// The method will return an empty collection if there are any 
			// problems, or no public repos are found, so set it up to return
			// a List containing a single item.
			mockGHReposSvc.Setup(o => o.GetGitHubRepos(
				It.IsAny<HttpClient>(),
				It.IsAny<string>())).ThrowsAsync(new Exception());

			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				await apiCallServices.GetPublicGhUserReposByUsername("github");

			// should have thrown
		}

		//
		// The following are the tests for the
		// /public async Task<int> GetOpenPRsByGhUserRepo(string ghUsername,string ghRepoName)/
		// method.
		// 

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task CallingGetOpenPRsByGhUserRepo_WithNullOrWsUsernameParam_ShouldThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				await apiCallServices.GetOpenPRsByGhUserRepo(" ", "NonNullValue");

			// Should have thrown.
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task CallingGetOpenPRsByGhUserRepo_WithNullOrWsRepoNameParam_ShouldThrow()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var taskOfListOfGhUserRepos =
				await apiCallServices.GetOpenPRsByGhUserRepo("NonNullValue", " ");

			// Should have thrown.
		}


		[TestMethod]
		public void CallingGetOpenPRsByGhUserRepo_WithNonNullOrWsParams_ReturnsNon0int_ShouldPass()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// The apiCallServices.GetOpenPRsByGhUserRepo() method calls 
			// async Task<int> GetGitHubRepoOpenPRCount(HttpClient, string, string)
			// which is the method that actually makes the GitHub API call, 
			// so it definitely needs to be mocked out.
			mockGHPRsSvc.Setup(o => o.GetGitHubRepoOpenPRCount(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>()
			)).ReturnsAsync(2);

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var methodCallResultTask = apiCallServices.GetOpenPRsByGhUserRepo("username", "TestGhRepoName");
			var methodCallResult = methodCallResultTask.Result;

			// Assert
			Assert.IsInstanceOfType(methodCallResult, typeof(int));
			Assert.AreEqual(2, methodCallResult);
		}

		[TestMethod]
		[ExpectedException(typeof(HttpRequestException))]
		public async Task IfGetGitHubRepoOpenPRCount_ThrowsHttpRequestExcept_GetOpenPRsShouldAlso()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// The apiCallServices.GetOpenPRsByGhUserRepo() method calls 
			// async Task<int> GetGitHubRepoOpenPRCount(HttpClient, string, string)
			// which is the method that actually makes the GitHub API call, 
			// so it definitely needs to be mocked out.
			mockGHPRsSvc.Setup(o => o.GetGitHubRepoOpenPRCount(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>()
			)).ThrowsAsync(new HttpRequestException());

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var methodCallResultTask = 
				await apiCallServices.GetOpenPRsByGhUserRepo("username", "TestGhRepoName");

			// should have thrown
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public async Task IfGetGitHubRepoOpenPRCount_ThrowsException_GetOpenPRsShouldAlso()
		{
			// Arrange 
			var mockHttpClientProvider = new Mock<IHttpClientProvider>();
			mockHttpClientProvider.Setup(o => o.GetHttpClientInstance()).Returns(new HttpClient());

			var mockHCAuthConfigr = new Mock<IHttpClientAuthorizeConfigurator>();
			mockHCAuthConfigr.Setup(o => o.AddBasicAuthorizationHeaderValue(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>())).Returns(true);

			var mockGHReposSvc = new Mock<IGitHubRepos>();
			var mockGHPRsSvc = new Mock<IGitHubPullReqs>();

			// The apiCallServices.GetOpenPRsByGhUserRepo() method calls 
			// async Task<int> GetGitHubRepoOpenPRCount(HttpClient, string, string)
			// which is the method that actually makes the GitHub API call, 
			// so it definitely needs to be mocked out.
			mockGHPRsSvc.Setup(o => o.GetGitHubRepoOpenPRCount(
				It.IsAny<HttpClient>(),
				It.IsAny<string>(),
				It.IsAny<string>()
			)).ThrowsAsync(new Exception());

			// Act
			GitHubApiCallServices apiCallServices = new GitHubApiCallServices(
				mockHttpClientProvider.Object,
				mockHCAuthConfigr.Object,
				mockGHReposSvc.Object,
				mockGHPRsSvc.Object);

			var methodCallResultTask =
				await apiCallServices.GetOpenPRsByGhUserRepo("username", "TestGhRepoName");

			// should have thrown
		}






	}
}
