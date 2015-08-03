using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Triosoft.JiraTimeTracker.Settings;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker.JiraRestApi
{
   public class JiraApiClient 
   {
      private static readonly NewtonsoftJsonSerializer _newtonsoftJsonSerializer = new NewtonsoftJsonSerializer();

      private readonly Uri _baseUrl;
      private readonly RestClient _restClient;

      public JiraApiClient(JiraSettings jiraSettings)
      {
         _baseUrl = jiraSettings.BaseUrl;
         _restClient = new RestClient(new Uri(jiraSettings.BaseUrl, "rest/api/2"))
         {
            Authenticator = new HttpBasicAuthenticator(jiraSettings.UserName, jiraSettings.Password)
         };
         _restClient.AddHandler("application/json", new DynamicJsonDeserializer());
      }

      public Uri GetUrlForIssue(Issue issue)
      {
         return new Uri(_baseUrl, "browse/" + issue.Key);
      }

      public async Task<IEnumerable<Issue>> GetIssuesAsync()
      {
         RestRequest restRequest = CreateRequest("search", Method.GET);
         restRequest.AddQueryParameter("jql", "assignee = currentUser() AND status = Open");

         IRestResponse<dynamic> restResponse = await _restClient.ExecuteTaskAsync<dynamic>(restRequest);
         restResponse.EnsureSuccessStatusCode();

         List<Issue> issues = new List<Issue>();

         foreach (dynamic issueJson in restResponse.Data.issues)
         {
            issues.Add(new Issue(issueJson.key.Value, issueJson.fields.issuetype.name.Value, issueJson.fields.summary.Value));
         }

         return issues;
      }

      public async Task LogWork(Worklog worklog)
      {
         RestRequest restRequest = CreateRequest("issue/" + worklog.Issue.Key + "/worklog", Method.POST);
         var worklogJson = new
         {
            started = worklog.Start,
            timeSpentSeconds = worklog.DurationInSeconds
         };
         restRequest.AddJsonBody(worklogJson);

         IRestResponse response = await _restClient.ExecuteTaskAsync(restRequest);
         response.EnsureSuccessStatusCode();
      }

      private static RestRequest CreateRequest(string resource, Method method)
      {
         return new RestRequest(resource, method)
         {
            JsonSerializer = _newtonsoftJsonSerializer
         };
      }
   }
}