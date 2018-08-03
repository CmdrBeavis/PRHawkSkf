using System;
using System.Net.Http;


namespace PRHawkSkf.GitHubApiRepoInterfaces
{
	public interface IGitHubApiRepoHelpers
	{
		/// <summary>
		/// Parses the Link header for the last page number re: pagination.
		/// </summary>
		/// <param name="response">
		/// An instance of the <see cref="Exception"/> class.
		/// </param>
		/// <returns>
		/// An integer containing either the 'last' page number, or -1 if
		/// there's no Link header.
		/// </returns>
		/// <exception cref="HttpResponseMessage">
		/// Thrown if the Link header values do not contain /rel=\"last\"/
		/// </exception>
		int ParseLinkHeaderForLastPageNum(HttpResponseMessage response);
	}
}
