using Triosoft.JiraTimeTracker.LocalDataStorage;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class StopTrackingWorkCommand
   {
      private readonly WorkQueue _workQueue;
      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public StopTrackingWorkCommand(WorkQueue workQueue)
      {
         _workQueue = workQueue;
      }

      public void Execute()
      {
         _workQueue.StopCurrentWork();
         foreach (Worklog worklog in _workQueue.DequeueCompletedWork())
         {
            _sqliteDataStorage.AddWorklog(worklog);
         }
      }
   }
}