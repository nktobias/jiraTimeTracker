﻿using System;
using System.Linq;
using System.Net;
using RestSharp;

namespace Triosoft.JiraTimeTracker.JiraRestApi
{
   public static class RestResponseExtensions
   {
      private static readonly HttpStatusCode[] SuccessStatusCodes = { HttpStatusCode.Accepted, HttpStatusCode.Created, HttpStatusCode.OK };

      public static void EnsureSuccessStatusCode(this IRestResponse response)
      {
         if (!SuccessStatusCodes.Contains(response.StatusCode))
         {
            throw new RestSharpHttpException(response);
         }
      }
   }
}