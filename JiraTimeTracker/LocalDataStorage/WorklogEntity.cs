using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.LocalDataStorage
{
   public class WorklogEntity
   {
      public WorklogEntity()
      {
      }

      public WorklogEntity(Worklog worklog)
      {
         Issue = new IssueEntity(worklog.Issue);
         Start = worklog.Start;
         DurationInSeconds = worklog.DurationInSeconds;
      }

      [PrimaryKey, AutoIncrement]
      public int Id { get; set; }

      [ForeignKey(typeof(IssueEntity))]
      public string IssueKey { get; set; }
      [ManyToOne]
      public IssueEntity Issue { get; set; }

      public DateTime Start { get; set; }
      public long DurationInSeconds { get; set; }
   }
}