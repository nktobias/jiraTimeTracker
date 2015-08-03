namespace Triosoft.JiraTimeTracker.Settings
{
   public class JiraSettingsStorage
   {
      private const string JiraSettingsFileName = "JiraSettings.xml";

      private readonly ApplicationStorageFolder _applicationStorageFolder = new ApplicationStorageFolder();
      private readonly ProtobufSerializer _binarySerializer = new ProtobufSerializer();
      private readonly DataEncryptor _dataEncryptor = new DataEncryptor();

      public JiraSettings Get()
      {
         JiraSettings result = null;

         if (_applicationStorageFolder.FileExists(JiraSettingsFileName))
         {
            byte[] encryptedBytes = _applicationStorageFolder.GetBytes(JiraSettingsFileName);
            EncryptedData encryptedData = _binarySerializer.Deserialize<EncryptedData, EncryptedDataProtobufContract>(encryptedBytes);

            byte[] decryptedData = _dataEncryptor.Decrypt(encryptedData);
            result = _binarySerializer.Deserialize<JiraSettings, JiraSettingsProtobufContract>(decryptedData);
         }

         return result;
      }

      public void Set(JiraSettings jiraSettings)
      {
         byte[] serializedJiraSettings = _binarySerializer.Serialize<JiraSettings, JiraSettingsProtobufContract>(jiraSettings);
         EncryptedData encryptedJiraSettings = _dataEncryptor.Encrypt(serializedJiraSettings);

         byte[] serializedEncryptedJiraSettings = _binarySerializer.Serialize<EncryptedData, EncryptedDataProtobufContract>(encryptedJiraSettings);

         _applicationStorageFolder.StoreBytes(JiraSettingsFileName, serializedEncryptedJiraSettings);
      }
   }
}
