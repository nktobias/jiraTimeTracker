using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Triosoft.JiraTimeTracker.Settings;

namespace Triosoft.JiraTimeTracker
{
   public partial class JiraSettingsWindow
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
