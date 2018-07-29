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
		public async Task<int> GetGitHubRepoOpenPRCount(
			HttpClient httpClient,
			string ghUsername,
			string ghUserRepoName)
		{
			try
			{
				// TODO: handle 'pagenation'
				var callUrl =
					$"repos/{ghUsername}/{ghUserRepoName}/pulls?state=opened&page=1&per_page=50";

				HttpResponseMessage response = await httpClient.GetAsync(callUrl).ConfigureAwait(false);

				if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
				{
					var data = await response.Content.ReadAsStringAsync();

					dynamic dynPRs = JsonConvert.DeserializeObject(data);
					int openPRCount = dynPRs.Count;

					return openPRCount;
				}
			}
			catch (Exception oEx)
			{
				Debug.WriteLine(
					"async Task<int> GetGitHubRepoOpenPRCount EXCEPTION:\r\n" +
					oEx.Message + "\n\r" + oEx.StackTrace);
			}

			// default return is 0
			return 0;
		}
	}
}
