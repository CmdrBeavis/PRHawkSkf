
using System.Net.Http;
using System.Threading.Tasks;


namespace PRHawkSkf.GitHubApiRepoInterfaces
{
	public interface IGitHubPullReqs
	{
		Task<int> GetGitHubRepoOpenPRCountAsync(
			HttpClient httpClient,
			string ghUsername,
			string ghUserRepoName);
	}
}
