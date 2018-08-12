using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.GitHubApiRepo
{
	/// <summary>
	/// This is a 'repository pattern' class that wraps the call(s) to the 
	/// GitHub API(s).
	/// </summary>
	public class GitHubRepos : IGitHubRepos
	{
		private readonly IGitHubApiRepoHelpers _gitHubApiRepoHelpers;

		/// <summary>
		/// Initializes a new instance of the <see cref="GitHubRepos"/> class.
		/// </summary>
		/// <param name="ghApiCallSvcs">The gh API call SVCS.</param>
		/// <exception cref="ArgumentNullException">ghApiCallSvcs</exception>
		public GitHubRepos(
			IGitHubApiRepoHelpers ghApiCallSvcs)
		{
			_gitHubApiRepoHelpers = ghApiCallSvcs ?? throw new ArgumentNullException(nameof(ghApiCallSvcs));
		}

		/// <summary>
		/// Gets the git hub repos.
		/// </summary>
		/// <param name="httpClient">The HTTP client.</param>
		/// <param name="ghUsername">The gh username.</param>
		/// <returns>
		/// A Task&lt;List&lt;GhUserRepo&gt;&gt;
		/// </returns>
		public async Task<List<GhUserRepo>> GetGitHubReposAsync(
			HttpClient httpClient, 
			string ghUsername)
		{
			try
		    {
			    var currentPage = 1;
			    var callUrl = $"users/{ghUsername}/repos?page={currentPage}";
			    var results = new List<GhUserRepo>();
				int lastPageNum = 0;
			    bool lastPageSet = false;

			    do
			    {
				    HttpResponseMessage response = await httpClient.GetAsync(callUrl).ConfigureAwait(false);

				    if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
				    {
					    var data = await response.Content.ReadAsStringAsync();

					    results.AddRange(JsonConvert.DeserializeObject<List<GhUserRepo>>(data));

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
					    $"users/{ghUsername}/repos?page={currentPage}";

			    } while (currentPage <= lastPageNum);

			    return results;
			}
			catch (Exception oEx)
		    {
			    Debug.WriteLine(
					"*** Task<List<GhUserRepo>> GetGitHubReposAsync() EXCEPTION: ***\r\n" + 
				    oEx.Message + "\n\r" + oEx.StackTrace);
		    }

			// return an empty collection if all else fails
		    return new List<GhUserRepo>();
	    }
	}
}
