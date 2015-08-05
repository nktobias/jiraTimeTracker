using Triosoft.JiraTimeTracker.Events;
using Triosoft.JiraTimeTracker.LocalDataStorage;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class StopTrackingWorkCommand
   {
      private readonly WorkQueue _workQueue;
      private readonly EventAggregator _eventAggregator;

      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public StopTrackingWorkCommand(WorkQueue workQueue, EventAggregator eventAggregator)
      {
         _workQueue = workQueue;
         _eventAggregator = eventAggregator;
      }

      public void Execute()
      {
         _workQueue.StopCurrentWork();
         foreach (Worklog worklog in _workQueue.DequeueCompletedWork())
         {
            _sqliteDataStorage.AddWorklog(worklog);
            _eventAggregator.Raise(new WorkLoggedEventArgs(worklog));
         }
      }
   }
}