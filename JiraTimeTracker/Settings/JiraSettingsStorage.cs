using System;
using System.IO;

namespace Triosoft.JiraTimeTracker.Settings
{
   public class JiraSettingsStorage
   {
      private static readonly string _settingsFilePath = Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
         "JiraTimeTrackerSettings.xml");

      private readonly ProtobufSerializer binarySerializer = new ProtobufSerializer();
      private readonly DataEncryptor dataEncryptor = new DataEncryptor();

      public JiraSettings Get()
      {
         JiraSettings result = null;

         if (File.Exists(_settingsFilePath))
         {
            byte[] encryptedBytes = File.ReadAllBytes(_settingsFilePath);
            EncryptedData encryptedData = binarySerializer.Deserialize<EncryptedData, EncryptedDataProtobufContract>(encryptedBytes);

            byte[] decryptedData = dataEncryptor.Decrypt(encryptedData);
            result = binarySerializer.Deserialize<JiraSettings, JiraSettingsProtobufContract>(decryptedData);
         }

         return result;
      }

      public void Set(JiraSettings jiraSettings)
      {
         byte[] serializedJiraSettings = binarySerializer.Serialize<JiraSettings, JiraSettingsProtobufContract>(jiraSettings);
         EncryptedData encryptedJiraSettings = dataEncryptor.Encrypt(serializedJiraSettings);

         byte[] serializedEncryptedJiraSettings = binarySerializer.Serialize<EncryptedData, EncryptedDataProtobufContract>(encryptedJiraSettings);

         File.WriteAllBytes(_settingsFilePath, serializedEncryptedJiraSettings);
      }
   }
}
