/*
* sones GraphDB - Community Edition - http://www.sones.com
* Copyright (C) 2007-2011 sones GmbH
*
* This file is part of sones GraphDB Community Edition.
*
* sones GraphDB is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB. If not, see <http://www.gnu.org/licenses/>.
* 
*/

using System;
using System.Threading;

namespace sones.Library.Threading
{
    public abstract class AWorkItemsGroupBase : IWorkItemsGroup
    {
        #region Private Fields

        /// <summary>
        /// Contains the name of this instance of SmartThreadPool.
        /// Can be changed by the user.
        /// </summary>
        private string _name = "WorkItemsGroupBase";

        /// <summary>
        /// Inidcates if the SmartThreadPool/WorkItemsGroup is idle.
        /// Its value is true if the _isIdleWaitHandle is set.
        /// </summary>
        private bool _isIdle = true;

        #endregion

        #region IWorkItemsGroup Members

        #region Public Methods

        /// <summary>
        /// Get/Set the name of the SmartThreadPool/WorkItemsGroup instance
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Abstract Methods

        public abstract int Concurrency { get; set; }
        public abstract int WaitingCallbacks { get; }
        public abstract object[] GetStates();
        public abstract WIGStartInfo WIGStartInfo { get; }
        public abstract void Start();
        public abstract void Cancel(bool abortExecution);
        public abstract bool WaitForIdle(int millisecondsTimeout);
        public abstract event WorkItemsGroupIdleHandler OnIdle;

        internal abstract void Enqueue(WorkItem workItem);
        internal virtual void PreQueueWorkItem() { }

        #endregion

        #region Common Base Methods

        /// <summary>
        /// Cancel all the work items.
        /// Same as Cancel(false)
        /// </summary>
        public virtual void Cancel()
        {
            Cancel(false);
        }

        /// <summary>
        /// Wait for the SmartThreadPool/WorkItemsGroup to be idle
        /// </summary>
        public void WaitForIdle()
        {
            WaitForIdle(Timeout.Infinite);
        }

        /// <summary>
        /// Wait for the SmartThreadPool/WorkItemsGroup to be idle
        /// </summary>
        public bool WaitForIdle(TimeSpan timeout)
        {
            return WaitForIdle((int)timeout.TotalMilliseconds);
        }

        /// <summary>
        /// IsIdle is true when there are no work items running or queued.
        /// </summary>
        public bool IsIdle
        {
            get { return _isIdle; }
            protected set { _isIdle = value; }
        }

        #endregion

        #region QueueWorkItem

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback)
        {
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="workItemPriority">The priority of the work item</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="workItemInfo">Work item info</param>
        /// <param name="callback">A callback to execute</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemInfo workItemInfo, WorkItemCallback callback)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, workItemInfo, callback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, object state)
        {
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, object state, WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="workItemInfo">Work item information</param>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemInfo workItemInfo, WorkItemCallback callback, object state)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, workItemInfo, callback, state);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            CallToPostExecute callToPostExecute)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, callToPostExecute);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            CallToPostExecute callToPostExecute,
            WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, callToPostExecute, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        #endregion

        #region QueueWorkItem(Action<...>)

        public IWorkItemResult QueueWorkItem(Action action)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                delegate
                {
                    action.Invoke();
                    return null;
                });
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        public IWorkItemResult QueueWorkItem<T>(Action<T> action, T arg)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                delegate
                {
                    action.Invoke(arg);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg } : null);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        public IWorkItemResult QueueWorkItem<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                delegate
                {
                    action.Invoke(arg1, arg2);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2 } : null);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        public IWorkItemResult QueueWorkItem<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                delegate
                {
                    action.Invoke(arg1, arg2, arg3);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3 } : null);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        public IWorkItemResult QueueWorkItem<T1, T2, T3, T4>(
            Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                           this,
                           WIGStartInfo,
                           delegate
                           {
                               action.Invoke(arg1, arg2, arg3, arg4);
                               return null;
                           },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3, arg4 } : null);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        #endregion

        #region QueueWorkItem(Func<...>)

        public IWorkItemResult<TResult> QueueWorkItem<TResult>(Func<TResult> func)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            delegate
                            {
                                return func.Invoke();
                            });
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        public IWorkItemResult<TResult> QueueWorkItem<T, TResult>(Func<T, TResult> func, T arg)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                delegate
                {
                    return func.Invoke(arg);
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            delegate
                            {
                                return func.Invoke(arg1, arg2);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            delegate
                            {
                                return func.Invoke(arg1, arg2, arg3);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            delegate
                            {
                                return func.Invoke(arg1, arg2, arg3, arg4);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3, arg4 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        #endregion

        #endregion
    }
}