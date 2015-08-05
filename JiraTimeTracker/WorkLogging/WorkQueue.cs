using System;
using System.Collections.Generic;

namespace Triosoft.JiraTimeTracker.WorkLogging
{
   public class WorkQueue
   {
      private StartOfWork _currentWork;
      private readonly Queue<Worklog> _worklogsQueue = new Queue<Worklog>();

      public void StartWorkOn(Issue issue)
      {
         StopCurrentWork();
         _currentWork = new StartOfWork(issue);
      }

      public void StopCurrentWork()
      {
         if (_currentWork != null)
         {
            Worklog finishedWork = _currentWork.FinishWork();

            if (finishedWork.IsAtLeast(TimeSpan.FromMinutes(2)))
            {
               _worklogsQueue.Enqueue(finishedWork);
            }
            _currentWork = null;
         }
      }

      public IEnumerable<Worklog> DequeueCompletedWork()
      {
         while (_worklogsQueue.Count > 0)
         {
            yield return _worklogsQueue.Dequeue();
         }
      }
   }
}