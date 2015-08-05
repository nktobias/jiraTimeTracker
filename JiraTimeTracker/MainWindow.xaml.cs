using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Triosoft.JiraTimeTracker.Actions;
using Triosoft.JiraTimeTracker.Events;
using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker
{
   public partial class MainWindow
   {
      private readonly WorkQueue _workQueue = new WorkQueue();
      private readonly EventAggregator _eventAggregator = new EventAggregator();

      private readonly JiraApiClientFacade _jiraApiClientFacade = new JiraApiClientFacade();

      private int _numberOfNotUploadedWorklogs;

      public MainWindow()
      {
         InitializeComponent();
         _eventAggregator.Subscribe<WorkLoggedEventArgs>(x =>
         {
            _numberOfNotUploadedWorklogs++;
            RefreshNotUploadedWorklogsStatus(); 
         });
         _eventAggregator.Subscribe<WorklogUploadedEventArgs>(x =>
         {
            _numberOfNotUploadedWorklogs--;
            RefreshNotUploadedWorklogsStatus();
         });
      }

      private void HandleWindowLoaded(object sender, RoutedEventArgs e)
      {
         SetAvailabilityOfIssueRelatedButtons(false);
         _stopTrackingButton.IsEnabled = false;
         RefreshIssuesDataGrid();
         _numberOfNotUploadedWorklogs = new GetNotUploadedWorklogsQuery().Execute().Count();
         RefreshNotUploadedWorklogsStatus();
      }

      private void HandleStartTrackingClick(object sender, RoutedEventArgs e)
      {
         new StartTrackingWorkOnIssueCommand(GetSelectedIssue(), _workQueue, _eventAggregator).Execute();
         _stopTrackingButton.IsEnabled = true;
      }

      private void HandleStopTrackingClick(object sender, RoutedEventArgs e)
      {
         new StopTrackingWorkCommand(_workQueue, _eventAggregator).Execute();
         _stopTrackingButton.IsEnabled = false;
      }

      private void HandleGoToIssueClick(object sender, RoutedEventArgs e)
      {
         InvokeJiraApiClientDependentAction(c => new OpenIssueInDefaultBrowserCommand(GetSelectedIssue(), c).Execute());
      }

      private void HandleUploadClick(object sender, RoutedEventArgs e)
      {
         InvokeJiraApiClientDependentAction(async c =>
         {
            await new UploadIssuesCommand(c, _eventAggregator).ExecuteAsync();
         });
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
         _gotToIssueButton.IsEnabled = enabled;
      }

      private void RefreshIssuesDataGrid()
      {
         _issuesDataGrid.ItemsSource = new GetLocalIssuesQuery().Execute();
      }

      private void RefreshNotUploadedWorklogsStatus()
      {
         _notUploadedWorklogsStatusText.Text = string.Format("Pending worklogs: {0}", _numberOfNotUploadedWorklogs);
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
