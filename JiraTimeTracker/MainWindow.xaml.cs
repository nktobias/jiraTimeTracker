using System;
using System.Windows;
using System.Windows.Controls;
using Triosoft.JiraTimeTracker.Actions;
using Triosoft.JiraTimeTracker.JiraRestApi;

namespace Triosoft.JiraTimeTracker
{
   public partial class MainWindow
   {
      private readonly JiraApiClientFacade _jiraApiClientFacade = new JiraApiClientFacade();

      public MainWindow()
      {
         InitializeComponent();
         SetAvailabilityOfIssueRelatedButtons(false);
      }

      private void HandleWindowLoaded(object sender, RoutedEventArgs e)
      {
         RefreshIssuesDataGrid();
      }      

      private void HandleStartTrackingClick(object sender, RoutedEventArgs e)
      {
      }

      private void HandleStopTrackingClick(object sender, RoutedEventArgs e)
      {  
      }

      private void HandleGoToIssueClick(object sender, RoutedEventArgs e)
      {
         InvokeJiraApiClientDependentAction(c => new OpenIssueInDefaultBrowserCommand(GetSelectedIssue(), c).Execute());
      }

      private void HandleUploadClick(object sender, RoutedEventArgs e)
      {
         
      }

      private void HandleDownloadClick(object sender, RoutedEventArgs e)
      {
         InvokeJiraApiClientDependentAction(async c =>
         {
            await new DownloadIssuesCommand(c).ExecuteAsync();
            RefreshIssuesDataGrid();
         });
      }

      private void HandleIssuesDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         SetAvailabilityOfIssueRelatedButtons(e.AddedItems.Count > 0);
      }

      private void SetAvailabilityOfIssueRelatedButtons(bool enabled)
      {
         _startTrackingButton.IsEnabled = enabled;
         _stopTrackingButton.IsEnabled = enabled;
      }

      private void RefreshIssuesDataGrid()
      {
         _issuesDataGrid.ItemsSource = new GetLocalIssuesQuery().Execute();
      }

      private Issue GetSelectedIssue()
      {
         return (Issue)_issuesDataGrid.SelectedItem;
      }

      private void InvokeJiraApiClientDependentAction(Action<JiraApiClient> action)
      {
         JiraApiClient client = _jiraApiClientFacade.TryToGetClientWithPreviouslyProvidedSettings();

         if (client == null)
         {
            JiraSettingsWindow jiraSettingsWindow = new JiraSettingsWindow { Owner = this };
            if (jiraSettingsWindow.ShowDialog() == true)
            {
               client = _jiraApiClientFacade.GetClientWithNewSettings(jiraSettingsWindow.ProvidedSettings);
            }
         }

         if (client != null)
         {
            action(client);
         }
      }
   }
}
