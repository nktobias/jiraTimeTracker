using System;

namespace Triosoft.JiraTimeTracker.WorkLogging
{
   public class Worklog
   {
      public Worklog(Issue issue, DateTime start, TimeSpan duration)
      {
         Issue = issue;
         Start = start;
         Duration = duration;
      }

      public Issue Issue { get; private set; }
      public DateTime Start { get; private set; }
      public TimeSpan Duration { get; private set; }

      public long DurationInSeconds
      {
         get
         {
            return (long)Duration.TotalSeconds;
         }
      }
   }
}