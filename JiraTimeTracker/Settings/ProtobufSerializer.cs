using System.IO;
using ProtoBuf;

namespace Triosoft.JiraTimeTracker.Settings
{
   public class ProtobufSerializer
   {
      public byte[] Serialize<TData, TContract>(TData input)
         where TContract : IProtobufContract<TData>, new()
      {
         byte[] result;

         using (MemoryStream serializationStream = new MemoryStream())
         {
            TContract contract = new TContract();
            contract.InitializeFromDataObject(input);

            Serializer.Serialize(serializationStream, contract);
            result = serializationStream.ToArray();
         }

         return result;
      }

      public TData Deserialize<TData, TContract>(byte[] input)
         where TContract : IProtobufContract<TData>
      {
         TData result;

         using (MemoryStream deserializationStream = new MemoryStream(input))
         {
            TContract contract = Serializer.Deserialize<TContract>(deserializationStream);
            result = contract.ToDataObject();
         }

         return result;
      }
   }
}