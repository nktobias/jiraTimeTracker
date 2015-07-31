using Newtonsoft.Json;
using System;

namespace Triosoft.JiraTimeTracker.JiraRestApi
{
   /// <summary>
   /// This custom converter is necessary only because DateTime format accepted by Jira API is ""2015-07-31T20:02:34.976+0200".
   /// The format is very similar to .NET standard roundtrip format ("O", or ISO 8061), with two important differences:
   /// 1.) Fractional part of the second (the part after the dot) needs to be exactly three digits (i.e. milliseconds).
   /// 2.) Timezone information cannot contain colon (":").
   /// </summary>
   public class JiraDateTimeJsonConverter : JsonConverter
   {
      public override bool CanRead
      {
         get
         {
            return false;
         }
      }

      public override bool CanWrite
      {
         get
         {
            return true;
         }
      }

      public override bool CanConvert(System.Type objectType)
      {
         return objectType == typeof(DateTime);
      }

      public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
      {
         throw new System.NotImplementedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         DateTime valueToConvert = (DateTime)value;

         string convertedValue = valueToConvert.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00");

         writer.WriteValue(convertedValue);
      }
   }
}
