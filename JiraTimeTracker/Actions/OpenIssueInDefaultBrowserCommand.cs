using System.Diagnostics;
using Triosoft.JiraTimeTracker.JiraRestApi;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class OpenIssueInDefaultBrowserCommand
   {
      private readonly Issue _issue;
      private readonly JiraApiClient _jiraApiClient;

      public OpenIssueInDefaultBrowserCommand(Issue issue, JiraApiClient jiraApiClient)
      {
         _issue = issue;
         _jiraApiClient = jiraApiClient;
      }

      public void Execute()
      {
         Process.Start(_jiraApiClient.GetUrlForIssue(_issue).ToString());
      }
   }
}