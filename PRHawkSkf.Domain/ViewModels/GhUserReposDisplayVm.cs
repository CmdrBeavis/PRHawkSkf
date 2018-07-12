using System.Collections.Generic;

using PRHawkSkf.Domain.Models;


namespace PRHawkSkf.Domain.ViewModels
{
	public class GhUserReposDisplayVm
	{
		public string GitHubUsername { get; set; }

		public List<RepoListItem> Repositories { get; set; }
	}
}
