using System.Collections.Generic;
using System.Threading.Tasks;
using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.LocalDataStorage;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class DownloadIssuessCommand
   {
      private readonly JiraApiClient _jiraApiClient;
      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public DownloadIssuessCommand(JiraApiClient jiraApiClient)
      {
         _jiraApiClient = jiraApiClient;
      }

      public async Task ExecuteAsync()
      {
         IEnumerable<Issue> downloadedIssues = await _jiraApiClient.GetIssuesAsync();
         _sqliteDataStorage.SetIssues(downloadedIssues);
      }
   }
}