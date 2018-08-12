using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

		/// <summary>
		/// Gets the public repository(s) of/by the specified username.
		/// </summary>
		/// <param name="ghUsername">
		/// The GitHub username.
		/// </param>
		/// <returns>
		/// A Task&lt;List&lt;GhUserRepo&gt;&gt;
		/// </returns>
		/// <exception cref="ArgumentNullException">ghUsername</exception>
		/// <exception cref="Exception">Error creating HttpClient instance.</exception>
		public async Task<List<GhUserRepo>> GetPublicGhUserReposByUsername(
			string ghUsername)
		{
			if (string.IsNullOrWhiteSpace(ghUsername))
			{
				throw new ArgumentNullException(nameof(ghUsername));
			}

			// Get the HttpClient
			var httpClient = _httpClientProvider.GetHttpClientInstance();

			// Set the Authentication stuff into the HttpClient instance
			// TODO: (?) read the u/p from the web.config
			if (!_httpClientAuthPrvdr.AddBasicAuthorizationHeaderValue(
				httpClient, 
				"snkirklandinterview", 
				"99036c318413ae379983014b2eeae395b7be14b0"))
			{
				throw new Exception("Error creating HttpClient instance.");
			}

			List<GhUserRepo> rawRepoData = null;

			try
			{
				// Call the 'repo' layer to make the actual call to GitHub
				rawRepoData = await _ghRepos.GetGitHubReposAsync(httpClient, ghUsername);
			}
			catch (HttpRequestException httpReqException)
			{
				Debug.WriteLine(
					"*** Task<List<GhUserRepo>> GetPublicGhUserReposByUsername() EXCEPTION: ***\r\n" +
					httpReqException.Message + "\r\n" +
					httpReqException.StackTrace);
				throw;
			}
			catch (Exception oEx)
			{
				Console.WriteLine(oEx);
				throw;
			}

			return rawRepoData ?? new List<GhUserRepo>();
		}

		/// <summary>
		/// Gets the open PRs for the specified user repository.
		/// </summary>
		/// <param name="ghUsername">
		/// The username.
		/// </param>
		/// <param name="ghRepoName">
		/// A string containing the name of the repository.
		/// </param>
		/// <returns>
		/// A Task&lt;int&gt;
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// ghUsername
		/// or
		/// ghRepoName
		/// </exception>
		/// <exception cref="Exception">Error adding authentication credentials to HttpClient instance.</exception>
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

			// Set the Authentication stuff into HttpClient instance
			// TODO: (?) read the u/p from the web.config
			if (!_httpClientAuthPrvdr.AddBasicAuthorizationHeaderValue(
				httpClient,
				"snkirklandinterview",
				"99036c318413ae379983014b2eeae395b7be14b0"))
			{
				throw new Exception("Error adding authentication credentials to HttpClient instance.");
			}

			try
			{
				result = await _gitHubPullReqs.GetGitHubRepoOpenPRCountAsync(
					httpClient, ghUsername, ghRepoName);
			}
			catch (HttpRequestException httpReqException)
			{
				Debug.WriteLine(
					"*** Task<int> GetOpenPRsByGhUserRepo() EXCEPTION: ***\r\n" +
					httpReqException.Message + "\r\n" +
					httpReqException.StackTrace);
				throw;
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
