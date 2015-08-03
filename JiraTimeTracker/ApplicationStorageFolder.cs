using System;
using System.IO;

namespace Triosoft.JiraTimeTracker
{
   public class ApplicationStorageFolder
   {
      private static readonly DirectoryInfo _directoryInfo;

      public static string Path
      {
         get
         {
            return _directoryInfo.FullName;
         }
      }

      static ApplicationStorageFolder()
      {
          _directoryInfo = Directory.CreateDirectory(System.IO.Path.Combine(
             Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "3oSoft",
            "JiraTimeTracker"));
      }

      public static string GetPathToFileInDirectory(string fileName)
      {
         return System.IO.Path.Combine(Path, fileName);
      }

      public bool FileExists(string fileName)
      {
         return File.Exists(GetPathToFileInDirectory(fileName));
      }

      public void StoreBytes(string fileName, byte[] bytes)
      {
         File.WriteAllBytes(GetPathToFileInDirectory(fileName), bytes);
      }

      public byte[] GetBytes(string fileName)
      {
         return File.ReadAllBytes(GetPathToFileInDirectory(fileName));
      }
   }
}