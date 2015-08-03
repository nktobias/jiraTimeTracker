using SQLite.Net.Attributes;

namespace Triosoft.JiraTimeTracker.LocalDataStorage
{
   public class IssueEntity
   {
      [PrimaryKey]
      public string Key { get; set; }
      public string IssueType { get; set; }
      public string Summary { get; set; }
   }
}