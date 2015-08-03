using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class StartTrackingWorkOnIssueCommand
   {
      private readonly Issue _issue;
      private readonly WorkQueue _workQueue;

      public StartTrackingWorkOnIssueCommand(Issue issue, WorkQueue workQueue)
      {
         _issue = issue;
         _workQueue = workQueue;
      }

      public void Execute()
      {
         _workQueue.StartWorkOn(_issue);
      }
   }
}