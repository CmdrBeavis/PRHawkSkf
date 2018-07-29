using System;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class HttpClientProviderTests
	{
		private static Uri expectedWebCfgUri;
		private Mock<IWebConfigReader> mockWebConfigReader;

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{

		}

		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		[TestInitialize()]
		public void MyTestInitialize()
		{
			expectedWebCfgUri = new Uri("https://api.github.com/");

			mockWebConfigReader = new Mock<IWebConfigReader>();
			mockWebConfigReader.Setup(o => o.GetAppSetting<string>("BaseGitHubApiUrl")).Returns("https://api.github.com/");
		}

		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void NewHttpClientProviderInstance_GivenWorkingWebCfgReader_ShouldReturnBaseAddrCfgdHttpClient()
		{
			// Arrange
			HttpClientProvider client = new HttpClientProvider(mockWebConfigReader.Object);
			
			// Act
			var returnedValue = client.GetHttpClientInstance();

			// Assert
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(HttpClient));
			Assert.AreEqual(expectedWebCfgUri, returnedValue.BaseAddress);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HttpClientProvider_ConstructedWithNullWebCfgReader_ShouldThrow()
		{
			// Arrange
			HttpClientProvider client = new HttpClientProvider(null);

			// Act
			var returnedValue = client.GetHttpClientInstance();
		}

		// Make sure this header value has been put into the HttpClient
		// _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

		[TestMethod]
		public void NewHttpClientFromProvider_ShouldInclAcceptHeaderItem_ShouldPass()
		{

		}

		// Make sure this header value has been put into the HttpClient
		// _httpClient.DefaultRequestHeaders.Add("User-Agent", "PRHawkSkf");

		public void NewHttpClientFromProvider_ShouldInclUserAgentHeaderItem_ShouldPass()
		{

		}

	}
}
