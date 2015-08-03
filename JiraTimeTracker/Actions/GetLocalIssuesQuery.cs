using System.Collections.Generic;
using Triosoft.JiraTimeTracker.LocalDataStorage;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class GetLocalIssuesQuery
   {
      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public IEnumerable<Issue> Execute()
      {
         return _sqliteDataStorage.GetIssues();
      }       
   }
}