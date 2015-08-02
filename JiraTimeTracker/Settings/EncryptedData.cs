namespace Triosoft.JiraTimeTracker.Settings
{
   public class EncryptedData
   {
      public EncryptedData(byte[] entropy, byte[] data)
      {
         Entropy = entropy;
         Data = data;
      }

      public byte[] Entropy { get; private set; }
      public byte[] Data { get; private set; }
   }
}
