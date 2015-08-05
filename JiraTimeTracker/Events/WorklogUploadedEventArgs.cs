using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Events
{
   public class WorklogUploadedEventArgs
   {
      public WorklogUploadedEventArgs(Worklog worklog)
      {
         Worklog = worklog;
      }

      public Worklog Worklog { get; private set; }
   }
}