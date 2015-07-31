namespace Triosoft.JiraTimeTracker
{
   public class Issue
   {
      public Issue(string key, string issueType, string summary)
      {
         Key = key;
         IssueType = issueType;
         Summary = summary;
      }

      public string Key { get; private set; }
      public string IssueType { get; private set; }
      public string Summary { get; private set; }

      public override string ToString()
      {
         return string.Format("{0} - {1} - {2}", Key, IssueType, Summary);
      }
   }
}