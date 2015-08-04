using System;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.LocalDataStorage
{
   public class WorklogEntity
   {
      private string _issueKey;
      private IssueEntity _issue;

      public WorklogEntity()
      {
      }

      public WorklogEntity(Worklog worklog)
      {
         Issue = new IssueEntity(worklog.Issue);
         Start = worklog.Start;
         DurationInSeconds = worklog.DurationInSeconds;
      }

      public int Id { get; set; }

      public string IssueKey
      {
         get
         {
            return _issueKey;
         }
         set
         {
            _issueKey = value;
            Issue = null;
         }
      }
      
      public IssueEntity Issue 
      {
         get
         {
            return _issue;
         } 
         set 
         { 
            _issue = value;
            IssueKey = _issue.Key;
         } 
      }

      public DateTime Start { get; set; }
      public long DurationInSeconds { get; set; }

      public Worklog ToWorklog()
      {
         return new Worklog(Issue.ToIssue(), Start, TimeSpan.FromSeconds(DurationInSeconds));
      }
   }
}