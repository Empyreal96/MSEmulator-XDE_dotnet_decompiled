using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x020000DA RID: 218
	public class PSEventJob : Job
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x0004576C File Offset: 0x0004396C
		public PSEventJob(PSEventManager eventManager, PSEventSubscriber subscriber, ScriptBlock action, string name) : base((action == null) ? null : action.ToString(), name)
		{
			if (eventManager == null)
			{
				throw new ArgumentNullException("eventManager");
			}
			if (subscriber == null)
			{
				throw new ArgumentNullException("subscriber");
			}
			base.UsesResultsCollection = true;
			this.action = action;
			this.eventManager = eventManager;
			this.subscriber = subscriber;
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x000457C5 File Offset: 0x000439C5
		public PSModuleInfo Module
		{
			get
			{
				return this.action.Module;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000457D2 File Offset: 0x000439D2
		public override void StopJob()
		{
			this.eventManager.UnsubscribeEvent(this.subscriber);
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x000457E5 File Offset: 0x000439E5
		public override string StatusMessage
		{
			get
			{
				return this.statusMessage;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000457ED File Offset: 0x000439ED
		public override bool HasMoreData
		{
			get
			{
				return this.moreData;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000457F5 File Offset: 0x000439F5
		public override string Location
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000457F8 File Offset: 0x000439F8
		internal ScriptBlock ScriptBlock
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00045800 File Offset: 0x00043A00
		internal void Invoke(PSEventSubscriber eventSubscriber, PSEventArgs eventArgs)
		{
			if (base.IsFinishedState(base.JobStateInfo.State))
			{
				return;
			}
			base.SetJobState(JobState.Running);
			SessionState publicSessionState = this.action.SessionStateInternal.PublicSessionState;
			publicSessionState.PSVariable.Set("eventSubscriber", eventSubscriber);
			publicSessionState.PSVariable.Set("event", eventArgs);
			publicSessionState.PSVariable.Set("sender", eventArgs.Sender);
			publicSessionState.PSVariable.Set("eventArgs", eventArgs.SourceEventArgs);
			List<object> list = new List<object>();
			try
			{
				Pipe outputPipe = new Pipe(list);
				this.action.InvokeWithPipe(false, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, outputPipe, null, false, null, null, eventArgs.SourceArgs);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				if (!(ex is PipelineStoppedException))
				{
					this.LogErrorsAndOutput(list, publicSessionState);
					base.SetJobState(JobState.Failed);
				}
				throw;
			}
			this.LogErrorsAndOutput(list, publicSessionState);
			this.moreData = true;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000458FC File Offset: 0x00043AFC
		internal void NotifyJobStopped()
		{
			base.SetJobState(JobState.Stopped);
			this.moreData = false;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0004590C File Offset: 0x00043B0C
		private void LogErrorsAndOutput(List<object> results, SessionState actionState)
		{
			for (int i = 0; i < results.Count; i++)
			{
				this.WriteObject(results[i]);
			}
			base.Error.Clear();
			int num = 0;
			ArrayList arrayList = (ArrayList)actionState.PSVariable.Get("error").Value;
			arrayList.Reverse();
			for (int j = 0; j < arrayList.Count; j++)
			{
				ErrorRecord errorRecord = (ErrorRecord)arrayList[j];
				if (num == this.highestErrorIndex)
				{
					this.WriteError(errorRecord);
					this.highestErrorIndex++;
				}
				num++;
			}
		}

		// Token: 0x0400057D RID: 1405
		private PSEventManager eventManager;

		// Token: 0x0400057E RID: 1406
		private PSEventSubscriber subscriber;

		// Token: 0x0400057F RID: 1407
		private int highestErrorIndex;

		// Token: 0x04000580 RID: 1408
		private string statusMessage;

		// Token: 0x04000581 RID: 1409
		private bool moreData;

		// Token: 0x04000582 RID: 1410
		private ScriptBlock action;
	}
}
