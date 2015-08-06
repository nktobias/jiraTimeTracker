using System.Windows;
using System.Windows.Threading;

namespace Triosoft.JiraTimeTracker
{
   public partial class App
   {
      protected override void OnStartup(StartupEventArgs e)
      {
         base.OnStartup(e);

         DispatcherUnhandledException += HandleAppDispatcherUnhandledException;
      }

      private void HandleAppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
      {
         MessageBox.Show("There was an error: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
         e.Handled = true;
      }
   }
}
