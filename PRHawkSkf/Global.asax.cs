using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

using PRHawkSkf.GitHubApiRepo;
using PRHawkSkf.GitHubApiRepoInterfaces;
using PRHawkSkf.Services;


namespace PRHawkSkf
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// Create the container as usual.
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

			container.Register<IWebConfigReader, WebConfigReader>(Lifestyle.Scoped);
			container.Register<IBase64Codec, Base64Codec>(Lifestyle.Scoped);
			container.Register<IHttpClientAuthorizeConfigurator, HttpClientAuthorizeConfigurator>(Lifestyle.Scoped);

			var webConfigReader = new WebConfigReader();
			var clientProvider = new HttpClientProvider(webConfigReader);
			container.Register<IHttpClientProvider>(() => clientProvider, Lifestyle.Scoped);

			container.Register<IGitHubAPICredentialsReader, GitHubAPICredentialsReader>(Lifestyle.Scoped);
			container.Register<IGitHubApiRepoHelpers, GitHubApiRepoHelpers>(Lifestyle.Scoped);
			container.Register<IGitHubRepos, GitHubRepos>(Lifestyle.Scoped);
			container.Register<IGitHubPullReqs, GitHubPullReqs>(Lifestyle.Scoped);

			container.Register<IGitHubApiCallServices, GitHubApiCallServices>(Lifestyle.Scoped);
			container.Register<IGhUserReposServices, GhUserReposServices>(Lifestyle.Scoped);

			// This is an extension method from the [Simple Injector] integration package.
			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

			container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

			// the rest of the 'normal' registration stuff.
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
