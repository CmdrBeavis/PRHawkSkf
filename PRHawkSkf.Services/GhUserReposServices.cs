using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;


namespace PRHawkSkf.Services
{
	public class GhUserReposServices : IGhUserReposServices
	{
		private readonly IGitHubApiCallServices _ghApiCallServices;

		/// <summary>
		/// Initializes a new instance of the <see cref="GhUserReposServices"/> class.
		/// </summary>
		/// <param name="ghApiCallSvcs">The gh API call SVCS.</param>
		/// <exception cref="ArgumentNullException">ghApiCallSvcs</exception>
		public GhUserReposServices(
			IGitHubApiCallServices ghApiCallSvcs)
		{
			_ghApiCallServices = ghApiCallSvcs ?? throw new ArgumentNullException(nameof(ghApiCallSvcs));
		}

		/// <summary>
		/// Hydrates the GitHub user repositories/PR count display ViewModel.
		/// </summary>
		/// <param name="ghUsername">
		/// The GitHub username.
		/// </param>
		/// <param name="returnPrivateRepos">
		/// if set to <c>true</c> [include private repos in returned set],
		/// else don't.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="GhUserReposDisplayVm"/> class, 
		/// hydrated with the appropriate data.
		/// </returns>
		public async Task<GhUserReposDisplayVm> HydrateGhUserReposDisplayVmAsync(
			string ghUsername,
			bool returnPrivateRepos = false)
		{
			if (string.IsNullOrWhiteSpace(ghUsername))
			{
				throw new ArgumentNullException(nameof(ghUsername));
			}

			var result = new GhUserReposDisplayVm
			{
				Repositories = new List<RepoListItem>(),
				GitHubUsername = ghUsername
			};

			// get the public repositories for the specified user
			List<GhUserRepo> ghReposData = 
				await _ghApiCallServices.GetPublicGhUserReposByUsername(ghUsername);

			// filter private repos per returnPrivateRepos value.
			var filteredRepos = returnPrivateRepos ? ghReposData : ghReposData.Where(x => !x.@private).ToList();

			if (filteredRepos.Count <= 0)
			{
				return result;		// EXIT point
			}

			foreach (var ghUserRepo in filteredRepos)
			{
				var newRecord = new RepoListItem();

				newRecord.name = $"<a href=\"{ghUserRepo.html_url}\" target=\"_blank\">{ghUserRepo.name}</a>";
				newRecord.num_pull_reqs =
					await _ghApiCallServices.GetOpenPRsByGhUserRepo(ghUsername, ghUserRepo.name);

				result.Repositories.Add(newRecord);
			}

			// do the sort on number of PRs
			result.Repositories = 
				result.Repositories.OrderByDescending(x => x.num_pull_reqs).ToList();

			return result;
		}
	}
}
