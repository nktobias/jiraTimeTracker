using ProtoBuf;
using System;

namespace Triosoft.JiraTimeTracker.Settings
{
   [ProtoContract]
   public class JiraSettingsProtobufContract : IProtobufContract<JiraSettings>
   {
      [ProtoMember(1)]
      public Uri BaseUrl { get; set; }
      [ProtoMember(2)]
      public string UserName { get; set; }
      [ProtoMember(3)]
      public string Password { get; set; }

      public void InitializeFromDataObject(JiraSettings dataObject)
      {
         BaseUrl = dataObject.BaseUrl;
         UserName = dataObject.UserName;
         Password = dataObject.Password;
      }

      public JiraSettings ToDataObject()
      {
         return new JiraSettings(BaseUrl, UserName, Password);
      }
   }
}
