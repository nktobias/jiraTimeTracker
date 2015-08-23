using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triosoft.JiraTimeTracker
{
   public class ProgressNotifyIcon
   {
      private NotifyIcon _notifyIcon;
      private Timer _timer;
      private int _progress = 0;

      public event EventHandler DoubleClick;

      Icon _idleIcon;     
      List<Icon> _porgressIcons = new List<Icon>();

      private const int MilisecondsInterval = 750;

      public ProgressNotifyIcon()
      {
         InitializeIcons();

         _notifyIcon = new NotifyIcon();
         _notifyIcon.DoubleClick += HandleOnDoubleClick;
         _notifyIcon.Icon = _idleIcon;
         _notifyIcon.Visible = true;

         _timer = new Timer();
         _timer.Interval = MilisecondsInterval;
         _timer.Tick += HandleOnTimerTicked;
      }

      private void InitializeIcons()
      {
         Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/JiraTimeTracker;component/Icons/logo.ico")).Stream;
         _idleIcon = new Icon(iconStream);

         _porgressIcons.Add(new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/JiraTimeTracker;component/Icons/1.ico")).Stream));
         _porgressIcons.Add(new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/JiraTimeTracker;component/Icons/2.ico")).Stream));
         _porgressIcons.Add(new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/JiraTimeTracker;component/Icons/3.ico")).Stream));
         _porgressIcons.Add(new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/JiraTimeTracker;component/Icons/4.ico")).Stream));
      }

      private void HandleOnDoubleClick(object sender, EventArgs e)
      {
         if (DoubleClick != null)
         {
            DoubleClick(sender, EventArgs.Empty);
         }
      }

      private void HandleOnTimerTicked(object sender, EventArgs e)
      {
         _notifyIcon.Icon = GetNext();
      }

      private Icon GetNext()
      {
         if (_progress >= _porgressIcons.Count-1)
         {
            _progress = -1;
         }
         _progress++;
         return _porgressIcons[_progress];
      }

      public void Hide()
      {
         _notifyIcon.Visible = false;
      }

      public void Start()
      {
         _timer.Start();
      }

      public void Stop()
      { 
         _timer.Stop();
         _progress = -1;
         _notifyIcon.Icon = _idleIcon;
      }
      
   }
}

