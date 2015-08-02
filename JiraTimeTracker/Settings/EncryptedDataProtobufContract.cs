using ProtoBuf;

namespace Triosoft.JiraTimeTracker.Settings
{
   [ProtoContract]
   public class EncryptedDataProtobufContract : IProtobufContract<EncryptedData>
   {
      [ProtoMember(1)]
      public byte[] Entropy { get; set; }
      [ProtoMember(2)]
      public byte[] Data { get; set; }

      public void InitializeFromDataObject(EncryptedData dataObject)
      {
         Entropy = dataObject.Entropy;
         Data = dataObject.Data;
      }

      public EncryptedData ToDataObject()
      {
         return new EncryptedData(Entropy, Data);
      }
   }
}
