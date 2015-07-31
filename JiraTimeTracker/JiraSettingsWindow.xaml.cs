using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Triosoft.JiraTimeTracker
{
   /// <summary>
   /// Interaction logic for SettingsWindow.xaml
   /// </summary>
   public partial class JiraSettingsWindow : Window
   {
      public JiraSettings ProvidedSettings { get; private set; }

      public JiraSettingsWindow()
      {
         InitializeComponent();
      }

      private void HandleOkClick(object sender, RoutedEventArgs e)
      {
         List<string> validationErrors = new List<string>();
         if (!Uri.IsWellFormedUriString(_baseUrlTextBox.Text, UriKind.Absolute))
         {
            validationErrors.Add("Base URL is invalid.");
         }
         if (string.IsNullOrEmpty(_userNameTextBox.Text))
         {
            validationErrors.Add("User name cannot be empty.");
         }
         if (string.IsNullOrEmpty(_passwordBox.Password))
         {
            validationErrors.Add("Password cannot be empty.");
         }

         if (validationErrors.Any())
         {
            MessageBox.Show(this, string.Join(Environment.NewLine, validationErrors), "Invalid values", MessageBoxButton.OK, MessageBoxImage.Error);
         }
         else
         {
            ProvidedSettings = new JiraSettings(new Uri(_baseUrlTextBox.Text), _userNameTextBox.Text, _passwordBox.Password);
            DialogResult = true;
         }
      }

      private void HandleCancelClick(object sender, RoutedEventArgs e)
      {
         DialogResult = false;
      }
   }
}
