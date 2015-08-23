using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Triosoft.JiraTimeTracker.Actions;
using Triosoft.JiraTimeTracker.Events;
using Triosoft.JiraTimeTracker.JiraRestApi;
using Triosoft.JiraTimeTracker.Settings;
using Triosoft.JiraTimeTracker.WorkLogging;

namespace Triosoft.JiraTimeTracker
{
   public partial class MainWindow
   {
      private readonly WorkQueue _workQueue = new WorkQueue();
      private readonly EventAggregator _eventAggregator = new EventAggregator();

      private readonly JiraApiClientFacade _jiraApiClientFacade = new JiraApiClientFacade();
      private readonly JiraSettingsStorage _jiraSettingsStorage = new JiraSettingsStorage();

      private int _numberOfNotUploadedWorklogs;
      private DataGridRow _markedRow;
      private ProgressNotifyIcon _progressNotifyIcon;

      public MainWindow()
      {
         InitializeComponent();
         _progressNotifyIcon = new ProgressNotifyIcon();
         _progressNotifyIcon.DoubleClick += HandleOnNotifyIconDoubleClicked;
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
         MarkIssueAsTrackingOn();
         _progressNotifyIcon.Start();
         _stopTrackingButton.IsEnabled = true;
      }

      private void HandleStopTrackingClick(object sender, RoutedEventArgs e)
      {
         new StopTrackingWorkCommand(_workQueue, _eventAggregator).Execute();
         MarkIssueAsTrackingOff();
         _progressNotifyIcon.Stop();
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

      private void HandleSettingsClick(object sender, RoutedEventArgs e)
      {
         JiraSettingsWindow jiraSettingsWindow = new JiraSettingsWindow { Owner = this, InitialSettings = _jiraSettingsStorage.Get() };
         if (jiraSettingsWindow.ShowDialog() == true)
         {
            _jiraSettingsStorage.Set(jiraSettingsWindow.ProvidedSettings);
         }
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

      private void MarkIssueAsTrackingOn()
      {
          MarkIssueAsTrackingOff();
          _markedRow = _issuesDataGrid.ItemContainerGenerator.ContainerFromItem(_issuesDataGrid.SelectedItem) as DataGridRow;
          _markedRow.Background = System.Windows.Media.Brushes.MediumSpringGreen;
      }

      private void MarkIssueAsTrackingOff()
      {
          if (_markedRow != null)
          {
              _markedRow.Background = System.Windows.Media.Brushes.White;
          }
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

      private void HandleOnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         _progressNotifyIcon.Hide();
      }

      private void HandleOnNotifyIconDoubleClicked(object sender, EventArgs e)
      {
         this.WindowState = WindowState.Normal;
         this.Visibility = System.Windows.Visibility.Visible;
         SystemCommands.RestoreWindow(this);
      }      

      private void HandleOnStateChanged(object sender, EventArgs e)
      {
         if (this.WindowState == WindowState.Minimized)
         {
            this.Visibility = System.Windows.Visibility.Collapsed;
         }
      }
   }
}
