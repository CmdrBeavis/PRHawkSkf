
## This is "PRHawk," a 'Take Home Assignment' / interview coding problem that I completed recently. (5-Aug-2018)

The assignment was to build a web application that gives users visibility into how well GitHub repository owners are managing their Pull Requests.

When a user enters the GitHub Username into the field on the Home/Main page of this application, the following is what should happen:
1. The page will show the full list of public repositories that the given GitHub username owns.
2. Each item in the list will:
	1. Show the repository name
	2. Show the number of open pull requests in that repository
	3. When clicking the repository name, navigate to that repository in GitHub
3. The list will be sorted by number of open pull requests in descending order from most to least

And of course I was to use the publicly available GitHub API to get this information. 

**NOTE:** GitHub’s API only allows 60 requests per hour for unauthenticated users. To get 5,000 requests per hour, you will need a username and API token to use in order to test/run the solution. At least, if you're going to try testing it against the likes of Google's user ('google') or others with huge amounts of repositories and pull requests. (See instructions below.) 


## Instructions for running this solution: 

1. Download the ZIP file (don't forget to Unblock it! [if on Windows]) Or of course you could just clone it. 

2. Unzip [or clone] into desired folder.

3. Load the solution into Visual Studio 2017 (Community Ed should work fine.)

4. Build. If you need to do anything special for NuGet packages, do that.
   I'm set up to have Visual Studio automatically get all required NuGet packages.
   
   The only 'runtime/dependencies' are the NuGet packages and IIS Express, which of course you should have installed along with Visual Studio.

5. Edit the Web.config file and add appropriate GitHub API credentials to the following appSettings elements.

<add key="GitHubAPIUsername" value="username_here" />
<add key="GitHubAPIPassword" value="password_here" />

6. Run the app via ^F5 (Start Without Debugging)

7. Enter a GitHub username in the provided field and click on the Go button

8. Bask in the glory of my awesome solution!  ;)

That's it!
