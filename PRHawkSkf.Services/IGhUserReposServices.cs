using System.Collections.Generic;
using System.Threading.Tasks;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;


namespace PRHawkSkf.Services
{
	public interface IGhUserReposServices
	{
		Task<GhUserReposDisplayVm> HydrateGhUserReposDisplayVmAsync(
			string ghUsername,
			//List<GhUserRepo> repoList,
			bool returnPrivateRepos = false);
	}
}
