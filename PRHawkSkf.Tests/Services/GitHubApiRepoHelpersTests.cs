using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using PRHawkSkf.GitHubApiRepo;
using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class GitHubApiRepoHelpersTests
	{
		[TestMethod]
		public void WhenInstantiatedWithValidParam_ANonNullGitHubApiRepoHelpersInstance_IsCreated()
		{
			// Arrange 

			// Act 
			var gitHubApiRepoHelpersInstance = new GitHubApiRepoHelpers();

			// Assert
			Assert.IsNotNull(gitHubApiRepoHelpersInstance);
			Assert.IsInstanceOfType(gitHubApiRepoHelpersInstance, typeof(IGitHubApiRepoHelpers));
		}

		// Can't find a way to create a fake HttpResponseMessage.

		//[TestMethod]
		//public void GivenAFakeHttpResponseMsg_ParseLinkHeaderForLastPageNum_IsCreated()
		//{
		//	var fakeHttpResponseMessage = new HttpResponseMessage
		//	{
		//		StatusCode = HttpStatusCode.OK
		//		//Headers = new HttpResponseHeaders()
		//		//{
		//		//	"Link",
		//		//	{ "<https://api.github.com/repositories/30092893/pulls?state=opened&page=2>; rel=\"next\", <https://api.github.com/repositories/30092893/pulls?state=opened&page=4>; rel=\"last\"" };
		//		//};
		//	};
		//}

	}
}
