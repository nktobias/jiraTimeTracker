using System;
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
         _connection.Open();
         if (createTables)
         {
            ExecuteNonQuery("CREATE TABLE Issue(Key varchar PRIMARY KEY NOT NULL, Type varchar NOT NULL, Summary varchar NOT NULL)");
            ExecuteNonQuery("CREATE TABLE Worklog(Id integer PRIMARY KEY AUTOINCREMENT NOT NULL, IssueKey varchar NOT NULL, Start bigint NOT NULL, DurationInSeconds bigint NOT NULL)");
         }
      }

      public IEnumerable<Issue> GetIssues()
      {
         using (SQLiteCommand command = _connection.CreateCommand())
         {
            command.CommandText = "SELECT Key, Type, Summary FROM Issue";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                  yield return new Issue(
                     (string)reader["Key"],
                     (string)reader["Type"],
                     (string)reader["Summary"]);
               }
            }
         }
      }

      public void SetIssues(IEnumerable<Issue> issues)
      {
         using (SQLiteTransaction transaction = _connection.BeginTransaction())
         {
            ExecuteNonQuery("DELETE FROM Issue", transaction);
            foreach (Issue issue in issues)
            {
               using (SQLiteCommand command = _connection.CreateCommand())
               {
                  command.Transaction = transaction;
                  command.CommandText = "INSERT INTO Issue (Key, Type, Summary) VALUES (@Key, @Type, @Summary)";
                  command.Parameters.AddWithValue("@Key", issue.Key);
                  command.Parameters.AddWithValue("@Type", issue.Type);
                  command.Parameters.AddWithValue("@Summary", issue.Summary);
                  command.ExecuteNonQuery();
               }
            }

            transaction.Commit();
         }
      }

      public void AddWorklog(Worklog worklog)
      {
         using (SQLiteCommand command = _connection.CreateCommand())
         {
            command.CommandText = "INSERT INTO Worklog (IssueKey, Start, DurationInSeconds) VALUES (@IssueKey, @Start, @DurationInSeconds)";
            command.Parameters.AddWithValue("@IssueKey", worklog.Issue.Key);
            command.Parameters.AddWithValue("@Start", worklog.Start.Ticks);
            command.Parameters.AddWithValue("@DurationInSeconds", worklog.DurationInSeconds);
            command.ExecuteNonQuery();
         }
      }

      public void DeleteWorklog(Worklog worklog)
      {
         if (!worklog.Id.HasValue)
         {
            throw new InvalidOperationException();
         }

         using (SQLiteCommand command = _connection.CreateCommand())
         {
            command.CommandText = "DELETE FROM Worklog WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", worklog.Id.Value);
            command.ExecuteNonQuery();
         }
      }

      public IEnumerable<Worklog> GetWorklogs()
      {
         using (SQLiteCommand command = _connection.CreateCommand())
         {
            command.CommandText = "SELECT I.Key AS IssueKey, I.Type AS IssueType, I.Summary AS IssueSummary, W.Id AS WorklogId, W.Start AS WorklogStart, W.DurationInSeconds AS WorklogDurationInSeconds FROM Issue I INNER JOIN Worklog W ON I.Key = W.IssueKey";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                  Issue issue = new Issue(
                     (string)reader["IssueKey"],
                     (string)reader["IssueType"],
                     (string)reader["IssueSummary"]);

                  yield return new Worklog(
                     (long)reader["WorklogId"],
                     issue, 
                     new DateTime((long)reader["WorklogStart"]),
                     TimeSpan.FromSeconds((long)reader["WorklogDurationInSeconds"]));
               }
            }
         }
      }

      private void ExecuteNonQuery(string queryText, SQLiteTransaction transaction = null)
      {
         using (SQLiteCommand command = _connection.CreateCommand())
         {
            command.Transaction = transaction;
            command.CommandText = queryText;
            command.ExecuteNonQuery();
         }
      }
   }
}