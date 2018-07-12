using System.Collections.Generic;
using System.Threading.Tasks;

using PRHawkSkf.Domain.Models;


namespace PRHawkSkf.Services
{
	public interface IGitHubApiCallServices
	{
		Task<List<GhUserRepo>> GetPublicGhUserReposByUsername(
			string ghUsername);

		Task<int> GetOpenPRsByGhUserRepo(
			string ghUsername,
			string ghRepo);
	}
}
