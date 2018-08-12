namespace PRHawkSkf.Services
{
	public interface IGitHubAPICredentialsReader
	{
		/// <summary>
		/// Gets the username.
		/// </summary>
		/// <returns>
		/// A string containing the 'username' to be used when accessing the
		/// GitHub API(s).
		/// </returns>
		string GetUsername();

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <returns>
		/// A string containing the 'password' to be used when accessing the
		/// GitHub API(s).
		/// </returns>
		string GetPassword();
	}
}