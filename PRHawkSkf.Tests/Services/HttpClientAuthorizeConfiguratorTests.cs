
using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{
	public class HttpClientAuthorizeConfiguratorTests
	{
		[TestClass]
		public class HttpClientProviderTests
		{
			[TestMethod]
			public void HttpClientAuthorizeConfiguratorInstantiation_CreatesNewInstance_ShouldPass()
			{
				// Arrange
				var mockBase64Codec = new Mock<IBase64Codec>();

				// Act
				var newCfgratorInst = new HttpClientAuthorizeConfigurator(mockBase64Codec.Object);

				// Assert
				Assert.IsNotNull(newCfgratorInst);
				Assert.IsInstanceOfType(newCfgratorInst, typeof(HttpClientAuthorizeConfigurator));
			}

			[TestMethod]
			[ExpectedException(typeof(ArgumentNullException))]
			public void HttpClientAuthCfg_IfReceivesNullHttpClient_ShouldThrow()
			{
				// Arrange
				var mockBase64Codec = new Mock<IBase64Codec>();
				var newCfgratorInst = new HttpClientAuthorizeConfigurator(mockBase64Codec.Object);

				// Act
				var result = newCfgratorInst.AddBasicAuthorizationHeaderValue(
					null, "userName", "password");

				// Assert
				// should have thrown
			}

			[TestMethod]
			[ExpectedException(typeof(ArgumentException))]
			public void HttpClientAuthCfg_IfReceivesNullOrWsUsernameParam_ShouldThrow()
			{
				// Arrange
				var mockBase64Codec = new Mock<IBase64Codec>();

				// HttpClient doesn't have an interface, so using a real instance
				var httpClient = new HttpClient();

				var newCfgratorInst = new HttpClientAuthorizeConfigurator(mockBase64Codec.Object);

				// Act
				var result = newCfgratorInst.AddBasicAuthorizationHeaderValue(
					httpClient, "", "password");

				// Assert
				// should have thrown
			}

			[TestMethod]
			[ExpectedException(typeof(ArgumentException))]
			public void HttpClientAuthCfg_IfReceivesNullOrWsPasswordParam_ShouldThrow()
			{
				// Arrange
				var mockBase64Codec = new Mock<IBase64Codec>();

				// HttpClient doesn't have an interface, so using a real instance
				var httpClient = new HttpClient();

				var newCfgratorInst = new HttpClientAuthorizeConfigurator(mockBase64Codec.Object);

				// Act
				var result = newCfgratorInst.AddBasicAuthorizationHeaderValue(
					httpClient, "userName", " ");

				// Assert
				// should have thrown
			}

			[TestMethod]
			public void HttpClientAuthCfg_AddBasicAuthHeaders_ReturnsTrue_ShouldPass()
			{
				// Arrange
				var httpClient = new HttpClient();
				var base64codec = new Base64Codec();
				var newCfgratorInst = new HttpClientAuthorizeConfigurator(base64codec);

				// Act
				var result = newCfgratorInst.AddBasicAuthorizationHeaderValue(
					httpClient, "userName", "password");

				// Assert 
				Assert.IsTrue(result);
			}

			[TestMethod]
			public void HttpClientAuthCfg_AddBasicAuthHeaders_HasAddedBasicAuthorizationHdrs_ShouldPass()
			{
				// Arrange
				var httpClient = new HttpClient();
				var base64codec = new Base64Codec();
				var newCfgratorInst = new HttpClientAuthorizeConfigurator(base64codec);

				// Act
				var result = newCfgratorInst.AddBasicAuthorizationHeaderValue(
					httpClient, "userName", "password");

				var encodedAuthValue = httpClient.DefaultRequestHeaders.Authorization;
				var decodedAuthKey = base64codec.Decode(encodedAuthValue.Parameter);
				var scheme = encodedAuthValue.Scheme;

				// Assert 
				Assert.IsTrue(result);
				Assert.AreEqual("userName:password", decodedAuthKey);
				Assert.AreEqual("Basic", scheme);
			}
		}
	}
}
