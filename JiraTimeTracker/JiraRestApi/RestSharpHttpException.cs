using System;
using RestSharp;

namespace Triosoft.JiraTimeTracker.JiraRestApi
{
   public class RestSharpHttpException : Exception
   {
      public RestSharpHttpException(IRestResponse response) : base(
         string.Format("There was an HTTP problem. Code: {0}, Message: {1}", response.StatusCode, response.Content))
      {
         Response = response;
      }

      public IRestResponse Response { get; private set; }
   }
}