using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using PRHawkSkf.Domain.Models;


namespace PRHawkSkf.GitHubApiRepoInterfaces
{
	public interface IGitHubRepos
	{
		Task<List<GhUserRepo>> GetGitHubReposAsync(HttpClient httpClient, string ghUsername);
	}
}
