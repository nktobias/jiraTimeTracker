using System.Security.Cryptography;

namespace Triosoft.JiraTimeTracker.Settings
{
   public class DataEncryptor
   {
      public EncryptedData Encrypt(byte[] dataToEncrypt)
      {
         byte[] entropy = new byte[20];
         using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
         {
            rng.GetBytes(entropy);
         }

         byte[] encryptedData = ProtectedData.Protect(dataToEncrypt, entropy, DataProtectionScope.CurrentUser);
         return new EncryptedData(entropy, encryptedData);
      }

      public byte[] Decrypt(EncryptedData encryptedData)
      {
         return ProtectedData.Unprotect(encryptedData.Data, encryptedData.Entropy, DataProtectionScope.CurrentUser);
      }
   }
}
