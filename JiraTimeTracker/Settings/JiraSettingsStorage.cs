using System;
using System.IO;

namespace Triosoft.JiraTimeTracker.Settings
{
   public class JiraSettingsStorage
   {
      private static readonly string _settingsFilePath = Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
         "JiraTimeTrackerSettings.xml");

      public JiraSettings Get()
      {
         JiraSettings result = null;

         if (File.Exists(_settingsFilePath))
         {
            byte[] encryptedBytes = File.ReadAllBytes(_settingsFilePath);
            BinarySerializer binarySerializer = new BinarySerializer();
            EncryptedSerializableJiraSettings encryptedSerializableJiraSettings = binarySerializer.Deserialize<EncryptedSerializableJiraSettings>(encryptedBytes);
            result = encryptedSerializableJiraSettings.Decrypt().ToJiraSettings();
         }

         return result;
      }

      public void Set(JiraSettings jiraSettings)
      {
         SerializableJiraSettings serializableJiraSettings = new SerializableJiraSettings(jiraSettings);
         EncryptedSerializableJiraSettings encryptedSerializableJiraSettings = serializableJiraSettings.Encrypt();

         BinarySerializer binarySerializer = new BinarySerializer();
         File.WriteAllBytes(_settingsFilePath, binarySerializer.Serialize(encryptedSerializableJiraSettings));
      }
   }
}
