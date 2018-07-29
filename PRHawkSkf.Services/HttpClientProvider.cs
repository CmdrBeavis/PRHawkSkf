using System;
using System.Net.Http;
using System.Net.Http.Headers;


namespace PRHawkSkf.Services
{
	public class HttpClientProvider : IHttpClientProvider
	{
		// I read some time ago that you really want to use a single instance
		// of this class, thus holding it as a static in this class.
		private static HttpClient _httpClient;

		/// <summary>
		/// Holds an instance of the <see cref="WebConfigReader"/> class.
		/// </summary>
		private readonly IWebConfigReader _webCfgRdr;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpClientProvider"/> class.
		/// </summary>
		public HttpClientProvider(
			IWebConfigReader webConfigReader)
		{
			_webCfgRdr = webConfigReader ?? throw new ArgumentNullException(nameof(webConfigReader));

			InitializeGitHubApiHttpClient();
		}

		/// <summary>
		/// Gets the HTTP client instance.
		/// </summary>
		/// <returns>
		/// An instance of the <see cref="HttpClient"/> class.
		/// </returns>
		public HttpClient GetHttpClientInstance()
		{
			return _httpClient;
		}

		/// <summary>
		/// Initializes the configured HTTP client.
		/// </summary>
		private void InitializeGitHubApiHttpClient()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(_webCfgRdr.GetAppSetting<string>("BaseGitHubApiUrl"))
			};

			// clear the default headers
			_httpClient.DefaultRequestHeaders.Accept.Clear();

			// TODO: (?) Read the accept header value from the Web.config file
			_httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

			// https://stackoverflow.com/questions/2482715/the-server-committed-a-protocol-violation-section-responsestatusline-error
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "PRHawkSkf");
		}
	}
}
