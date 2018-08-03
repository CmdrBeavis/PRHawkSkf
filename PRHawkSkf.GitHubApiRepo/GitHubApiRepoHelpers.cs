using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

using PRHawkSkf.GitHubApiRepoInterfaces;


namespace PRHawkSkf.GitHubApiRepo
{
	public class GitHubApiRepoHelpers : IGitHubApiRepoHelpers
	{
		/// <summary>
		/// Parses the Link header for the last page number re: pagination.
		/// </summary>
		/// <param name="response">
		/// An instance of the <see cref="HttpResponseMessage"/> class.
		/// </param>
		/// <returns>
		/// An integer containing either the 'last' page number, or 0 if
		/// there's no Link header.
		/// </returns>
		/// <exception cref="Exception">
		/// Thrown if the Link header values do not contain /rel=\"last\"/
		/// </exception>
		public int ParseLinkHeaderForLastPageNum(
			HttpResponseMessage response)
		{
			var linkHeaderKvp = response.Headers.FirstOrDefault(q => q.Key.Equals("Link"));
			var linkValues = linkHeaderKvp.Value?.ToList();
			var lastPageNum = 0;

			if (linkValues == null || !linkValues.Any())
			{
				return lastPageNum;
			}

			// first, split the link string on ','
			var linkStrings = new List<string>(linkValues[0].Split(','));

			// Now find the last page link and parse out the page number
			var lastPageStr = linkStrings.FirstOrDefault(q => q.Contains("rel=\"last\""));

			if (lastPageStr == null)
			{
				throw new Exception("A 'last' page Uri not found in Link header values.");
			}

			Regex regex = new Regex("page=(\\d{1,3})");
			var lastPageNumMatch = regex.Match(lastPageStr);
			lastPageNum = Convert.ToInt32(lastPageNumMatch.Groups[1].Value);

			return lastPageNum;
		}
	}
}
