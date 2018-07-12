using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;
using PRHawkSkf.Domain.Models;
using PRHawkSkf.Domain.ViewModels;
using PRHawkSkf.GitHubApiRepoInterfaces;
using PRHawkSkf.Services;


namespace PRHawkSkf.Controllers
{
	public class UserController : Controller
    {
		private readonly IGitHubApiCallServices _ghApiCallServices;
	    private readonly IGhUserReposServices _ghUserReposServices;

	    /// <summary>
		/// Initializes a new instance of the <see cref="UserController"/> class.
		/// </summary>
		/// <param name="ghApiCallSvcs">
		/// IGitHubApiCallServices
		/// </param>
		/// <param name="ghUserReposSvcs">
		/// IGhUserReposServices
		/// </param>
		/// <exception cref="System.ArgumentNullException">ghApiCallSvcs</exception>
		public UserController(
			IGitHubApiCallServices ghApiCallSvcs,
			IGhUserReposServices ghUserReposSvcs)
		{
			_ghApiCallServices = ghApiCallSvcs ?? throw new ArgumentNullException(nameof(ghApiCallSvcs));

			_ghUserReposServices = ghUserReposSvcs ?? throw new ArgumentNullException(nameof(ghUserReposSvcs));
		}


		// TODO: Do I want/need this here?
		// GET: User
		public ActionResult Index()
        {
            return View();
        }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
		public async Task<ActionResult> Index(string githubUsername)
		{
			// Call service layer and get the repos for the specified user
		    List<GhUserRepo> ghReposData;
			GhUserReposDisplayVm viewModel;

		    try
		    {
			    ghReposData = await _ghApiCallServices.GetPublicGhUserReposByUsername(githubUsername);

			    // TODO: I have to get all the open PRs for EACH Repo BEFORE I can even display the list!!
			    // "pulls_url":


				// load up the GhUserReposDisplayVm ViewModel
				viewModel = _ghUserReposServices.HydrateGhUserReposDisplayVm(
				    githubUsername,
				    ghReposData);
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