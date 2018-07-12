
using System.Net.Http;
using System.Threading.Tasks;


namespace PRHawkSkf.GitHubApiRepoInterfaces
{
	public interface IGitHubPullReqs
	{
		Task<int> GetGitHubRepoOpenPRCount(
			HttpClient httpClient,
			string ghUsername,
			string ghUserRepoName);
	}
}
