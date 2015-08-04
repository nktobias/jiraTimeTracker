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

      
      public string Key { get; set; }
      public string Type { get; set; }
      public string Summary { get; set; }

      public Issue ToIssue()
      {
         return new Issue(Key, Type, Summary);
      }
   }
}