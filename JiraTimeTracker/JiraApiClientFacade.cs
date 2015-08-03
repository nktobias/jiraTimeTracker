using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.Settings;

namespace Triosoft.JiraTimeTracker
{
   public class JiraApiClientFacade
   {
      private readonly JiraSettingsStorage _jiraSettingsStorage = new JiraSettingsStorage();

      public JiraApiClient TryToGetClientWithPreviouslyProvidedSettings()
      {
         JiraApiClient result = null;

         JiraSettings jiraSettings = _jiraSettingsStorage.Get();
         if (jiraSettings != null)
         {
            result = new JiraApiClient(jiraSettings);
         }

         return result;
      }

      public JiraApiClient GetClientWithNewSettings(JiraSettings jiraSettings)
      {
         _jiraSettingsStorage.Set(jiraSettings);

         JiraApiClient result = new JiraApiClient(jiraSettings);
         return result;
      }
   }
}