using System;

namespace Triosoft.JiraTimeTracker.WorkLogging
{
   public class Worklog
   {
      public Worklog(Issue issue, DateTime start, TimeSpan duration)
         : this(null, issue, start, duration)
      {
      }

      public Worklog(long id, Issue issue, DateTime start, TimeSpan duration)
         : this((long?)id, issue, start, duration)
      {
      }

      private Worklog(long? id, Issue issue, DateTime start, TimeSpan duration)
      {
         Id = id;
         Issue = issue;
         Start = start;
         Duration = duration;
      }

      public long? Id { get; private set; }

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