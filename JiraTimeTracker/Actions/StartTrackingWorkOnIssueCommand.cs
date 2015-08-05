using Triosoft.JiraTimeTracker.Events;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class StartTrackingWorkOnIssueCommand
   {
      private readonly Issue _issue;
      private readonly WorkQueue _workQueue;
      private readonly EventAggregator _eventAggregator;

      public StartTrackingWorkOnIssueCommand(Issue issue, WorkQueue workQueue, EventAggregator eventAggregator)
      {
         _issue = issue;
         _workQueue = workQueue;
         _eventAggregator = eventAggregator;
      }

      public void Execute()
      {
         new StopTrackingWorkCommand(_workQueue, _eventAggregator).Execute();
         _workQueue.StartWorkOn(_issue);
      }
   }
}