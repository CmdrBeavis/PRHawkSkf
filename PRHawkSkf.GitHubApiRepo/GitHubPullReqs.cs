using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
				var currentPage = 1;
				var callUrl = $"repos/{ghUsername}/{ghUserRepoName}/pulls?state=opened&page={currentPage}";
				int openPRCount = 0;
				int lastPageNum = 0;
				bool lastPageSet = false;
				Regex regex = new Regex("\\&page=(\\d{1,3})");

				do
				{
					HttpResponseMessage response = await httpClient.GetAsync(callUrl).ConfigureAwait(false);

					if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
					{
						var data = await response.Content.ReadAsStringAsync();

						dynamic dynPRs = JsonConvert.DeserializeObject(data);
						openPRCount += dynPRs.Count;

						// ParseLinkFromHeader would probably start here.

						if (!lastPageSet)
						{
							var linkHeaderKvp = response.Headers.FirstOrDefault(q => q.Key.Equals("Link"));
							var linkValues = linkHeaderKvp.Value?.ToList();

							if (linkValues != null && linkValues.Any())
							{
								// first, split the link string on ','
								var linkStrings = new List<string>(linkValues[0].Split(','));

								// Now find the last page link and parse out the page number
								var lastPageStr = linkStrings.FirstOrDefault(q => q.Contains("rel=\"last\""));

								if (lastPageStr == null)
								{
									// NOT SURE WHAT TO DO HERE.
									throw new Exception("keep the IDE|R# quiet for now.");
								}

								var lastPageNumMatch = regex.Match(lastPageStr);
								lastPageNum = Convert.ToInt32(lastPageNumMatch.Groups[1].Value);

								lastPageSet = true;
							}
						}

						// ParseLinkFromHeader would probably END here.
					}
					else
					{
						// TODO: Might want to do SOMETHING to handle this error
					}

					//currentPage++;
					callUrl =
						$"repos/{ghUsername}/{ghUserRepoName}/pulls?state=opened&page={currentPage++}";

				} while (currentPage <= lastPageNum);

				return openPRCount;
			}
			catch (Exception oEx)
			{
				var httpClientStringRep = httpClient == null ? "{null}" : "httpClient";
				Debug.WriteLine(
					$"async Task<int> GetGitHubRepoOpenPRCount(httpClient: {httpClientStringRep}, \r\n\t" +
					$"ghUsername: {ghUsername}, ghUserRepoName: { ghUserRepoName}) EXCEPTION:\r\n" +
					oEx.Message + "\n\r" + oEx.StackTrace);
			}

			// default return is 0
			return 0;
		}
	}
}
