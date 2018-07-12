using System.Collections.Generic;
using System.Linq;
using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;


namespace PRHawkSkf.Services
{
	public class GhUserReposServices : IGhUserReposServices
	{
		//public GhUserReposServices()
		//{ }

		/// <summary>
		/// Hydrates the GitHub user repositories display ViewModel.
		/// </summary>
		/// <param name="ghUsername">The gh username.</param>
		/// <param name="repoList">The repo list.</param>
		/// <param name="returnPrivateRepos">if set to <c>true</c> [return private repos].</param>
		/// <returns>
		/// An instance of the <see cref="GhUserReposDisplayVm"/> class, 
		/// hydrated with the appropriate data.
		/// </returns>
		public GhUserReposDisplayVm HydrateGhUserReposDisplayVm(
			string ghUsername,
			List<GhUserRepo> repoList,
			bool returnPrivateRepos = false)
		{
			var result = new GhUserReposDisplayVm
			{
				Repositories = new List<RepoListItem>(),
				GitHubUsername = ghUsername
			};

			// handling private status
			var filteredRepos = returnPrivateRepos ? repoList : repoList.Where(x => !x.@private).ToList();

			if (filteredRepos.Count <= 0)
			{
				return result;		// EXIT point
			}

			int tempCtr = 1;
			foreach (var ghUserRepo in filteredRepos)
			{
				var newRecord = new RepoListItem();

				newRecord.name = $"<a href=\"{ghUserRepo.html_url}\" target=\"_blank\">{ghUserRepo.name}</a>";
				newRecord.num_pull_reqs = tempCtr++;

				result.Repositories.Add(newRecord);
			}

			// do the sort on number of PRs
			result.Repositories = 
				result.Repositories.OrderByDescending(x => x.num_pull_reqs).ToList();

			return result;
		}


	}
}
