namespace Triosoft.JiraTimeTracker
{
//test hotfix
   public class Issue
   {
      public Issue(string key, string type, string summary)
      {
         Key = key;
         Type = type;
         Summary = summary;
      }

      public string Key { get; private set; }
      public string Type { get; private set; }
      public string Summary { get; private set; }

      public override string ToString()
      {
         return string.Format("{0} - {1} - {2}", Key, Type, Summary);
      }
   }
}