using SQLite.Net.Attributes;

namespace Triosoft.JiraTimeTracker.LocalDataStorage
{
   public class IssueEntity
   {
      public IssueEntity()
      {
      }

      public IssueEntity(Issue issue)
      {
         Key = issue.Key;
         IssueType = issue.IssueType;
         Summary = issue.Summary;
      }

      [PrimaryKey]
      public string Key { get; set; }
      public string IssueType { get; set; }
      public string Summary { get; set; }
   }
}