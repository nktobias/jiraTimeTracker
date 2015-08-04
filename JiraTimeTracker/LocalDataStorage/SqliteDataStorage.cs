using System.Collections.Generic;
using System.Data.SQLite;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.LocalDataStorage
{
   public class SqliteDataStorage
   {
      private const string DatabaseFileName = "3jtt.db";

      private readonly ApplicationStorageFolder _applicationStorageFolder = new ApplicationStorageFolder();
      private readonly SQLiteConnection _connection;

      public SqliteDataStorage()
      {
         bool createTables = !_applicationStorageFolder.FileExists(DatabaseFileName);

         _connection = new SQLiteConnection(string.Format("Data Source={0};Version=3", ApplicationStorageFolder.GetPathToFileInDirectory(DatabaseFileName)));
         if (createTables)
         {
            using (SQLiteCommand command = _connection.CreateCommand())
            {
               command.CommandText = "CREATE TABLE Issue(Key TEXT PRIMARY KEY NOT NULL, Type TEXT NOT NULL, Summary TEXT NOT NULL)";
               command.ExecuteNonQuery();
            }
         }
      }

      public IEnumerable<Issue> GetIssues()
      {
         return null;
      }

      public void SetIssues(IEnumerable<Issue> issues)
      {
         
      }

      public void AddWorklog(Worklog worklog)
      {
         
      }

      public IEnumerable<Worklog> GetWorklogs()
      {
         return null;
      }
   }
}