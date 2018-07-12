using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.GitHubApiRepoInterfaces;
using PRHawkSkf.Services;


namespace PRHawkSkf.GitHubApiRepo
{
	/// <summary>
	/// This is a 'repository pattern' class that wraps the call(s) to the 
	/// GitHub API(s).
	/// </summary>
	public class GitHubRepos : IGitHubRepos
	{
		/// <summary>
		/// Gets the git hub repos.
		/// </summary>
		/// <param name="httpClient">The HTTP client.</param>
		/// <param name="ghUsername">The gh username.</param>
		/// <returns>
		/// A Task&lt;GhUserRepoCollection&gt;
		/// </returns>
		public async Task<List<GhUserRepo>> GetGitHubRepos(
			HttpClient httpClient, 
			string ghUsername)
		{
			try
		    {
				// TODO: handle 'pagenation'
			    var callUrl =
				    $"users/{ghUsername}/repos?page=1&per_page=50";

				HttpResponseMessage response = await httpClient.GetAsync(callUrl).ConfigureAwait(false);

				if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
				{
					var data = await response.Content.ReadAsStringAsync();

					var results = JsonConvert.DeserializeObject<List<GhUserRepo>>(data);

					return results;
				}
			}
		    catch (Exception oEx)
		    {
			    Debug.WriteLine(
					"*** Task<List<GhUserRepo>> GetGitHubRepos() EXCEPTION: ***\r\n" + 
				    oEx.Message + "\n\r" + oEx.StackTrace);
		    }

			// return an empty collection if all else fails
		    return new List<GhUserRepo>();
	    }
	}
}
