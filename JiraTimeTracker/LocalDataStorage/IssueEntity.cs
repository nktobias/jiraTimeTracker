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
         Type = issue.Type;
         Summary = issue.Summary;
      }

      [PrimaryKey]
      public string Key { get; set; }
      public string Type { get; set; }
      public string Summary { get; set; }
   }
}