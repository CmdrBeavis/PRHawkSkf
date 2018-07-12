
using System.ComponentModel.DataAnnotations;


namespace PRHawkSkf.Domain.Models
{
	public class RepoListItem
	{
		[Display(Name = "Repository Name")]
		public string name { get; set; }

		[Display(Name = "Num Open PRs")]
		public int num_pull_reqs { get; set; }
	}
}
