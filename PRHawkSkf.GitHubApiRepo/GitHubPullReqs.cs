﻿using System;
using System.Collections.Generic;
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
		//public GitHubPullReqs()
		//{ }

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

				// Check response.statuscode   (ie. statuscode = 404 if card number not found | 200 == OK)
				if (response.IsSuccessStatusCode && (response.StatusCode == HttpStatusCode.OK))
				{
					var data = await response.Content.ReadAsStringAsync();

					dynamic dynPRs = JsonConvert.DeserializeObject(data);

					var test1 = ((IDictionary<string, object>) dynPRs).Count;

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