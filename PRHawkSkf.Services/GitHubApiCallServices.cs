using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PRHawkSkf.Domain.Models;
using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.Services
{
	public class GitHubApiCallServices : IGitHubApiCallServices
	{
		private readonly IHttpClientProvider _httpClientProvider;
		private readonly IHttpClientAuthorizeConfigurator _httpClientAuthPrvdr;
		private readonly IGitHubRepos _ghRepos;
		private readonly IGitHubPullReqs _gitHubPullReqs;


		/// <summary>
		/// Initializes a new instance of the <see cref="GitHubApiCallServices"/> class.
		/// </summary>
		/// <param name="httpClientProvider">
		/// IHttpClientProvider
		/// </param>
		/// <param name="hcac">
		/// IHttpClientAuthorizeConfigurator
		/// </param>
		/// <param name="ghReposInst">
		/// IGitHubRepos
		/// </param>
		/// <param name="gitHubPRs">
		/// IGitHubPullReqs
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if the <paramref name="ghReposInst"/> parameter is null.
		/// </exception>
		public GitHubApiCallServices(
			IHttpClientProvider httpClientProvider,
			IHttpClientAuthorizeConfigurator hcac,
			IGitHubRepos ghReposInst,
			IGitHubPullReqs gitHubPRs)
		{
			_httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(ghReposInst));
			_httpClientAuthPrvdr = hcac ?? throw new ArgumentNullException(nameof(ghReposInst));
			_ghRepos = ghReposInst ?? throw new ArgumentNullException(nameof(ghReposInst));
			_gitHubPullReqs = gitHubPRs ?? throw new ArgumentNullException(nameof(gitHubPRs));
		}


		// TODO: IS this tested?

		public async Task<List<GhUserRepo>> GetPublicGhUserReposByUsername(
			string ghUsername)
		{
			if (string.IsNullOrWhiteSpace(ghUsername))
			{
				throw new ArgumentNullException(nameof(ghUsername));
			}

			// Get the HttpClient
			var httpClient = _httpClientProvider.GetHttpClientInstance();

			// Set the Authentication stuff into said HttpClient instance
			// TODO: (?) read the u/p from the web.config
			if (!_httpClientAuthPrvdr.AddBasicAuthorizationHeaderValue(
				httpClient, 
				"snkirklandinterview", 
				"99036c318413ae379983014b2eeae395b7be14b0"))
			{
				throw new Exception("Error creating HttpClient instance.");
			}

			// Call the 'repo' layer to make the actual call to GitHub, passing in the HttpClient instance
			List<GhUserRepo> rawRepoData = null;

			try
			{
				rawRepoData = await _ghRepos.GetGitHubRepos(httpClient, ghUsername);
			}
			catch (HttpRequestException httpReqException)
			{
				Debug.WriteLine(
					"*** Task<List<GhUserRepo>> GetPublicGhUserReposByUsername() EXCEPTION: ***\r\n" +
					httpReqException.Message + "\r\n" +
					httpReqException.StackTrace);
			}
			catch (Exception oEx)
			{
				Console.WriteLine(oEx);
				throw;
			}

			if (rawRepoData != null)
			{
				return rawRepoData;
			}

			return new List<GhUserRepo>();
		}


		// TODO: IS this tested?

		public async Task<int> GetOpenPRsByGhUserRepo(
			string ghUsername,
			string ghRepoName)
		{
			int result = 0;

			if (string.IsNullOrWhiteSpace(ghUsername))
			{
				throw new ArgumentNullException(nameof(ghUsername));
			}

			if (string.IsNullOrWhiteSpace(ghRepoName))
			{
				throw new ArgumentNullException(nameof(ghRepoName));
			}

			// Get the HttpClient
			var httpClient = _httpClientProvider.GetHttpClientInstance();

			// Set the Authentication stuff into said HttpClient instance
			// TODO: (?) read the u/p from the web.config
			if (!_httpClientAuthPrvdr.AddBasicAuthorizationHeaderValue(
				httpClient,
				"snkirklandinterview",
				"99036c318413ae379983014b2eeae395b7be14b0"))
			{
				throw new Exception("Error creating HttpClient instance.");
			}

			try
			{
				result = await _gitHubPullReqs.GetGitHubRepoOpenPRCount(
					httpClient, ghUsername, ghRepoName);
			}
			catch (HttpRequestException httpReqException)
			{
				Debug.WriteLine(
					"*** Task<int> GetOpenPRsByGhUserRepo() EXCEPTION: ***\r\n" +
					httpReqException.Message + "\r\n" +
					httpReqException.StackTrace);
			}
			catch (Exception oEx)
			{
				Debug.WriteLine(oEx.ToString());
				throw;
			}

			return result;
		}
		
	}
}
