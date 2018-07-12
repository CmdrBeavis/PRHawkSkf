using System.Collections.Generic;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;


namespace PRHawkSkf.Services
{
	public interface IGhUserReposServices
	{
		GhUserReposDisplayVm HydrateGhUserReposDisplayVm(
			string ghUsername,
			List<GhUserRepo> repoList,
			bool returnPrivateRepos = false);
	}
}
