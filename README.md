
# This is "PRHawk," a 'Take Home Assignment' / interview coding problem that I completed. (5-Aug-2018)

The assignment was to build a web application that gives users visibility into how well GitHub repository owners are managing their Pull Requests.

When a user enters the GitHub Username into the field on the Home/Main page of this application, the following is what should happen:

1. The page will show the full list of public repositories that the given GitHub username owns.
2. Each item in the list will:

   a. Show the repository name

   b. Show the number of open pull requests in that repository

   c. When clicking the repository name, navigate to that repository in GitHub

3. The list will be sorted by number of open pull requests in descending order from most to least

And of course I was to use the publicly available GitHub API to get this information.

**NOTE:** GitHub’s API only allows 60 requests per hour for unauthenticated users. To get 5,000 requests per hour, you will need a username and API token to use in order to test/run the solution. Thus, I suggest trying to run this against non-HUGE repositories.

## Instructions for running this solution&colon;

1. Download the ZIP file [from GitHub] (don't forget to Unblock it! [if on Windows]) Or of course you could just clone it.

2. Unzip [or clone] into desired folder.

3. Load the solution into Visual Studio 2017|2019 (Community Ed should work fine.)

4. Build. If you need to do anything special for NuGet packages, do that.
   I'm set up to have Visual Studio automatically get all required NuGet packages.

   The only 'runtime/dependencies' are the NuGet packages and IIS Express, which of course you should have installed along with Visual Studio.

5. Edit the Web.config file and add appropriate GitHub API credentials to the following appSettings elements.

    &lt;add key="GitHubAPIUsername" value="GitHubUsername_here" /&gt;

    &lt;add key="GitHubAPIPassword" value="Personal(API)AccessToken_here" /&gt;

6. Run the app via ^F5 (Start Without Debugging)

7. Enter a GitHub username in the provided field and click on the Go button

8. Bask in the glory of my awesome solution!  ;)

### That's it&excl;
