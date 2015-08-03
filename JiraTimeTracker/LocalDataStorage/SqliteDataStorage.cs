using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Platform.Win32;

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

         _connection = new SQLiteConnection(new SQLitePlatformWin32(), ApplicationStorageFolder.GetPathToFileInDirectory(DatabaseFileName));
         if (createTables)
         {
            _connection.CreateTable<IssueEntity>();
         }
      }

      public IEnumerable<Issue> GetIssues()
      {
         return _connection.Table<IssueEntity>().Select(x => new Issue(x.Key, x.IssueType, x.Summary));
      }

      public void SetIssues(IEnumerable<Issue> issues)
      {
         _connection.BeginTransaction();
         _connection.DeleteAll<IssueEntity>();
         _connection.InsertAll(issues);
         _connection.Commit();
      }
   }
}