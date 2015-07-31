using System;
using System.Windows;
using System.Windows.Controls;
using Triosoft.JiraTimeTracker.JiraRestApi;

namespace Triosoft.JiraTimeTracker
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private JiraClient _jiraClient;

      public MainWindow()
      {
         InitializeComponent();
         SetAvailabilityOfIssueRelatedButtons(false);
      }

      private void HandleWindowLoaded(object sender, RoutedEventArgs e)
      {
         JiraSettingsWindow jiraSettingsWindow = new JiraSettingsWindow();
         if (jiraSettingsWindow.ShowDialog() == true)
         {
            _jiraClient = new JiraClient(jiraSettingsWindow.ProvidedSettings);
         }
         else
         {
            Application.Current.Shutdown();
         }
      }

      private async void HandleStartTrackingClick(object sender, RoutedEventArgs e)
      {
         Issue selectedIssue = GetSelectedIssue();
         await _jiraClient.LogWork(new Worklog(selectedIssue, DateTime.Now, new TimeSpan(0, 10, 0)));
      }

      private void HandleStopTrackingClick(object sender, RoutedEventArgs e)
      {
         
      }

      private void HandleGoToIssueClick(object sender, RoutedEventArgs e)
      {
         Issue selectedIssue = GetSelectedIssue();
         System.Diagnostics.Process.Start(_jiraClient.GetUrlForIssue(selectedIssue).ToString());
      }

      private void HandleUploadClick(object sender, RoutedEventArgs e)
      {
         
      }

      private async void HandleDownloadClick(object sender, RoutedEventArgs e)
      {
         _issuesDataGrid.ItemsSource = await _jiraClient.GetIssuesAsync();
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

      private Issue GetSelectedIssue()
      {
         return (Issue)_issuesDataGrid.SelectedItem;
      }
   }
}
