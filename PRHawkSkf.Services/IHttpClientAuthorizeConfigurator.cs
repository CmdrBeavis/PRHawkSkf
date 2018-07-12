
using System.Net.Http;


namespace PRHawkSkf.Services
{
	public interface IHttpClientAuthorizeConfigurator
	{
		bool AddBasicAuthorizationHeaderValue(
			HttpClient clientToUpdate,
			string userName,
			string password);
	}
}
