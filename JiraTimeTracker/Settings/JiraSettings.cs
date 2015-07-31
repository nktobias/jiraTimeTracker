using System;
namespace Triosoft.JiraTimeTracker.Settings
{
   public class JiraSettings
   {
      public JiraSettings(Uri baseUrl, string userName, string password)
      {
         BaseUrl = baseUrl;
         UserName = userName;
         Password = password;
      }

      public Uri BaseUrl { get; private set; }
      public string UserName { get; private set; }
      public string Password { get; private set; }
   }
}