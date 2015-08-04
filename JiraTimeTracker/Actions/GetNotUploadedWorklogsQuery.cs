using System.Collections.Generic;
using Triosoft.JiraTimeTracker.LocalDataStorage;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.Actions
{
   public class GetNotUploadedWorklogsQuery
   {
      private readonly SqliteDataStorage _sqliteDataStorage = new SqliteDataStorage();

      public IEnumerable<Worklog> Execute()
      {
         return _sqliteDataStorage.GetWorklogs();
      }       
   }
}