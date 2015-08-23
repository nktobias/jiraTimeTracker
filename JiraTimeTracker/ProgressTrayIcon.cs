using System;
using System.Collections.Generic;
using System.Drawing;
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

      Icon _idleIcon = new System.Drawing.Icon(@"..\..\Icons\logo.ico");

      Icon[] _porgressIcons = new Icon[]
      {
         new System.Drawing.Icon(@"..\..\Icons\1.ico"),
         new System.Drawing.Icon(@"..\..\Icons\2.ico"),
         new System.Drawing.Icon(@"..\..\Icons\3.ico"),
         new System.Drawing.Icon(@"..\..\Icons\4.ico")
      };

      private const int MilisecondsInterval = 750;

      public ProgressNotifyIcon()
      {
         _notifyIcon = new NotifyIcon();
         _notifyIcon.DoubleClick += HandleOnDoubleClick;
         _notifyIcon.Icon = _idleIcon;
         _notifyIcon.Visible = true;

         _timer = new Timer();
         _timer.Interval = MilisecondsInterval;
         _timer.Tick += HandleOnTimerTicked;
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
         if (_progress >= _porgressIcons.Length-1)
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
