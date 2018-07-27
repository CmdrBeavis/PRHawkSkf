using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

using PRHawkSkf.Domain.ViewModels;
using PRHawkSkf.Services;


namespace PRHawkSkf.Controllers
{
	public class UserController : Controller
    {
	    private readonly IGhUserReposServices _ghUserReposServices;

	    /// <summary>
		/// Initializes a new instance of the <see cref="UserController"/> class.
		/// </summary>
		/// <param name="ghUserReposSvcs">
		/// IGhUserReposServices
		/// </param>
		/// <exception cref="System.ArgumentNullException">ghApiCallSvcs</exception>
		public UserController(
			IGhUserReposServices ghUserReposSvcs)
		{
			_ghUserReposServices = ghUserReposSvcs ?? throw new ArgumentNullException(nameof(ghUserReposSvcs));
		}

		/// <summary>
		/// Displays the page of the specified github user's Repository(s)
		/// and count of open PRs for each.
		/// </summary>
		/// <param name="githubUsername">
		/// A string containing the specified GitHub username.
		/// </param>
		/// <returns>
		/// A Task&lt;ActionResult&gt;
		/// </returns>
		[HttpPost]
	    [ValidateAntiForgeryToken]
		public async Task<ActionResult> Index(string githubUsername)
		{
			GhUserReposDisplayVm viewModel;

		    try
		    {
				// load up the GhUserReposDisplayVm [ViewModel]
				viewModel = await _ghUserReposServices.HydrateGhUserReposDisplayVm(
				    githubUsername);
			}
			catch (Exception oEx)
		    {
			    Debug.WriteLine(oEx.ToString());
			    throw;
		    }

			return View(viewModel);
	    }
	}
}
