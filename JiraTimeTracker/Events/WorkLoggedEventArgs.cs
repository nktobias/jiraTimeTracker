using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Events
{
   public class WorkLoggedEventArgs
   {
      public WorkLoggedEventArgs(Worklog worklog)
      {
         Worklog = worklog;
      }

      public Worklog Worklog { get; private set; }
   }
}