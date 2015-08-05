using System.Threading.Tasks;
using Triosoft.JiraTimeTracker.Events;
using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.LocalDataStorage;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class UploadIssuesCommand
   {
      private readonly JiraApiClient _jiraApiClient;
      private readonly EventAggregator _eventAggregator;
      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public UploadIssuesCommand(JiraApiClient jiraApiClient, EventAggregator eventAggregator)
      {
         _jiraApiClient = jiraApiClient;
         _eventAggregator = eventAggregator;
      }

      public async Task ExecuteAsync()
      {
         foreach (Worklog worklogToUpload in _sqliteDataStorage.GetWorklogs())
         {
            await _jiraApiClient.LogWork(worklogToUpload);
            _eventAggregator.Raise(new WorklogUploadedEventArgs(worklogToUpload));
            _sqliteDataStorage.DeleteWorklog(worklogToUpload);
         }
      }
   }
}