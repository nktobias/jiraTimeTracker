using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Platform.Win32;
using SQLiteNetExtensions.Extensions;
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

         _connection = new SQLiteConnection(new SQLitePlatformWin32(), ApplicationStorageFolder.GetPathToFileInDirectory(DatabaseFileName));
         if (createTables)
         {
            _connection.CreateTable<IssueEntity>();
            _connection.CreateTable<WorklogEntity>();
         }
      }

      public IEnumerable<Issue> GetIssues()
      {
         return _connection.Table<IssueEntity>().Select(x => new Issue(x.Key, x.Type, x.Summary)).ToList();
      }

      public void SetIssues(IEnumerable<Issue> issues)
      {
         _connection.BeginTransaction();
         _connection.DeleteAll<IssueEntity>();
         _connection.InsertAll(issues.Select(x => new IssueEntity(x)));
         _connection.Commit();
      }

      public void AddWorklog(Worklog worklog)
      {
         _connection.InsertWithChildren(new WorklogEntity(worklog));
      }
   }
}