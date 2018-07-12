
using System.Net.Http;


namespace PRHawkSkf.Services
{
	public interface IHttpClientProvider
	{
		HttpClient GetHttpClientInstance();
	}
}
