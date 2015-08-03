using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.Settings;

namespace Triosoft.JiraTimeTracker
{
   public class JiraClientFacade
   {
      private readonly JiraSettingsStorage _jiraSettingsStorage = new JiraSettingsStorage();

      public JiraClient TryToGetClientWithPreviouslyProvidedSettings()
      {
         JiraClient result = null;

         JiraSettings jiraSettings = _jiraSettingsStorage.Get();
         if (jiraSettings != null)
         {
            result = new JiraClient(jiraSettings);
         }

         return result;
      }

      public JiraClient GetClientWithNewSettings(JiraSettings jiraSettings)
      {
         _jiraSettingsStorage.Set(jiraSettings);

         JiraClient result = new JiraClient(jiraSettings);
         return result;
      }
   }
}