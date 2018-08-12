
using System;


namespace PRHawkSkf.Services
{
	public class GitHubAPICredentialsReader : IGitHubAPICredentialsReader
	{
		/// <summary>
		/// Holds an instance of the <see cref="WebConfigReader"/> class.
		/// </summary>
		private readonly IWebConfigReader _webCfgRdr;

		/// <summary>
		/// Initializes a new instance of the <see cref="GitHubAPICredentialsReader"/> class.
		/// </summary>
		/// <param name="webConfigReader">The web configuration reader.</param>
		/// <exception cref="ArgumentNullException">webConfigReader</exception>
		public GitHubAPICredentialsReader(
			IWebConfigReader webConfigReader)
		{
			_webCfgRdr = webConfigReader ?? throw new ArgumentNullException(nameof(webConfigReader));
		}

		/// <summary>
		/// Gets the username.
		/// </summary>
		/// <returns>
		/// A string containing the 'username' to be used when accessing the
		/// GitHub API(s).
		/// </returns>
		public string GetUsername()
		{
			var result = _webCfgRdr.GetAppSetting<string>("GitHubAPIUsername");
			return result;
		}

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <returns>
		/// A string containing the 'password' to be used when accessing the
		/// GitHub API(s).
		/// </returns>
		public string GetPassword()
		{
			var result = _webCfgRdr.GetAppSetting<string>("GitHubAPIPassword");
			return result;
		}
	}
}

