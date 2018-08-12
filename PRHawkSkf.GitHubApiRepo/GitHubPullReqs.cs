using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.GitHubApiRepo
{
	public class GitHubPullReqs : IGitHubPullReqs
	{
		private readonly IGitHubApiRepoHelpers _gitHubApiRepoHelpers;

		/// <summary>
		/// Initializes a new instance of the <see cref="GitHubPullReqs"/> class.
		/// </summary>
		/// <param name="ghApiCallSvcs">
		/// A concrete instance of any class that implements the
		/// <see cref="IGitHubApiRepoHelpers"/> interface. Most typically an
		/// instance of the <see cref="GitHubApiRepoHelpers"/> class.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <paramref name="ghApiCallSvcs"/> parameter is null.
		/// </exception>
		public GitHubPullReqs(
			IGitHubApiRepoHelpers ghApiCallSvcs)
		{
			_gitHubApiRepoHelpers = ghApiCallSvcs ?? throw new ArgumentNullException(nameof(ghApiCallSvcs));
		}

		/// <summary>
		/// Gets the specified GitHub repository's open Pull Request(s) count.
		/// </summary>
		/// <param name="httpClient">
		/// The HTTP client.
		/// </param>
		/// <param name="ghUsername">
		/// The username of the GitHub user being queried.
		/// </param>
		/// <param name="ghUserRepoName">
		/// Name of the repo. being queried for its number of open pull 
		/// requests.
		/// </param>
		/// <returns>
		/// A Task&lt;int&gt;
		/// </returns>
		public async Task<int> GetGitHubRepoOpenPRCountAsync(
			HttpClient httpClient,
			string ghUsername,
			string ghUserRepoName)
		{
			try
			{
				var currentPage = 1;
				var callUrl = $"repos/{ghUsername}/{ghUserRepoName}/pulls?state=opened&page={currentPage}";
				int openPRCount = 0;
				int lastPageNum = 0;
				bool lastPageSet = false;

				do
				{
					HttpResponseMessage response = await httpClient.GetAsync(callUrl).ConfigureAwait(false);

					if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
					{
						var data = await response.Content.ReadAsStringAsync();

						dynamic dynPRs = JsonConvert.DeserializeObject(data);
						openPRCount += dynPRs.Count;

						if (!lastPageSet)
						{
							lastPageNum = _gitHubApiRepoHelpers.ParseLinkHeaderForLastPageNum(
								response);

							lastPageSet = lastPageNum >= 2;
						}
					}
					else
					{
						throw new Exception("The attempted call of the GitHub API apparently failed.");
					}

					currentPage++;
					callUrl =
						$"repos/{ghUsername}/{ghUserRepoName}/pulls?state=opened&page={currentPage}";

				} while (currentPage <= lastPageNum);

				return openPRCount;
			}
			catch (Exception oEx)
			{
				var httpClientStringRep = httpClient == null ? "{null}" : "httpClient";
				Debug.WriteLine(
					$"async Task<int> GetGitHubRepoOpenPRCountAsync(httpClient: {httpClientStringRep}, \r\n\t" +
					$"ghUsername: {ghUsername}, ghUserRepoName: { ghUserRepoName}) EXCEPTION:\r\n" +
					oEx.Message + "\n\r" + oEx.StackTrace);
			}

			// default return is 0
			return 0;
		}
	}
}
