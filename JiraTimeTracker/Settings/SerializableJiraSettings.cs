using System;
using System.Security.Cryptography;

namespace Triosoft.JiraTimeTracker.Settings
{
   [Serializable]
   public class SerializableJiraSettings
   {
      public SerializableJiraSettings()
      {
      }

      public SerializableJiraSettings(JiraSettings jiraSettings)
      {
         BaseUrl = jiraSettings.BaseUrl;
         UserName = jiraSettings.UserName;
         Password = jiraSettings.Password;
      }

      public Uri BaseUrl { get; set; }
      public string UserName { get; set; }
      public string Password { get; set; }

      public JiraSettings ToJiraSettings()
      {
         return new JiraSettings(BaseUrl, UserName, Password);
      }

      public EncryptedSerializableJiraSettings Encrypt()
      {
         BinarySerializer binarySerializer = new BinarySerializer();

         byte[] entropy = new byte[20];
         using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
         {
            rng.GetBytes(entropy);
         }

         byte[] serializedSettings = binarySerializer.Serialize(this);
         byte[] protectedSerializedSettings = ProtectedData.Protect(serializedSettings, entropy, DataProtectionScope.CurrentUser);

         return new EncryptedSerializableJiraSettings { EncryptedSettings = protectedSerializedSettings, Entropy = entropy };
      }
   }
}
