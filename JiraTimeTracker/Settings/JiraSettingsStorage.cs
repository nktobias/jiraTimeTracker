using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
namespace Triosoft.JiraTimeTracker.Settings
{
   public class JiraSettingsStorage
   {
      private static readonly _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JiraTimeTrackerSettings.xml")
      private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();


      [Serializable]
      private class SerializableJiraSettings
      {
         public SerializableJiraSettings(JiraSettings jiraSettings)
         {
            BaseUrl = jiraSettings.BaseUrl;
            UserName = jiraSettings.UserName;
            Password = jiraSettings.Password;
         }

         public Uri BaseUrl { get; set; }
         public string UserName { get; set; }
         public string Password { get; set; }
         public byte[] Enthropy { get; set;}

         public JiraSettings ToJiraSettings()
         {
            return new JiraSettings(BaseUrl, UserName, Password);
         }
      }

      public JiraSettings Get()
      {
         throw new NotImplementedException();
      }

      public void Set(JiraSettings jiraSettings)
      {
         byte[] entropy = new byte[20];
         using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
         {
            rng.GetBytes(entropy);
         }

         byte[] serializedSettings;
         using (MemoryStream serializationStream = new MemoryStream())
         {
            _binaryFormatter.Serialize(serializationStream, new SerializableJiraSettings(jiraSettings, entropy));
            serializedSettings = serializationStream.ToArray();
         }

         byte[] protectedSerializedSettings = ProtectedData.Protect(serializedSettings, entropy, DataProtectionScope.CurrentUser);
         File.WriteAllBytes(_settingsFilePath, protectedSerializedSettings);
      }
   }
}
