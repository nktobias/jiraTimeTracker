using System;
using System.Security.Cryptography;

namespace Triosoft.JiraTimeTracker.Settings
{
   [Serializable]
   public class EncryptedSerializableJiraSettings
   {
      public byte[] Entropy { get; set; }
      public byte[] EncryptedSettings { get; set; }

      public SerializableJiraSettings Decrypt()
      {
         byte[] decryptedJiraSettings = ProtectedData.Unprotect(EncryptedSettings, Entropy, DataProtectionScope.CurrentUser);
         BinarySerializer binarySerializer = new BinarySerializer();

         SerializableJiraSettings decryptedDeserializedJiraSettings = binarySerializer.Deserialize<SerializableJiraSettings>(decryptedJiraSettings);
         return decryptedDeserializedJiraSettings;
      }
   }
}
