using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Triosoft.JiraTimeTracker.Settings
{
   public class BinarySerializer
   {
      private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

      public byte[] Serialize<T>(T input)
      {
         byte[] output;

         using (MemoryStream serializationStream = new MemoryStream())
         {
            _binaryFormatter.Serialize(serializationStream, input);
            output = serializationStream.ToArray();
         }

         return output;
      }

      public T Deserialize<T>(byte[] input)
      {
         T output;

         using (MemoryStream serializationStream = new MemoryStream(input))
         {
            output = (T)_binaryFormatter.Deserialize(serializationStream);
         }

         return output;
      }
   }
}
