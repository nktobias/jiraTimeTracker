using System;

namespace Triosoft.JiraTimeTracker.WorkLogging
{
   public class StartOfWork
   {
      public StartOfWork(Issue issue)
      {
         Issue = issue;
         Start = DateTime.Now;
      }

      public Issue Issue { get; private set; }
      public DateTime Start { get; private set; }

      public Worklog FinishWork()
      {
         DateTime endOfWorkTimestamp = DateTime.Now;
         Worklog result = new Worklog(Issue, Start, endOfWorkTimestamp - Start);
         return result;
      }
   }
}