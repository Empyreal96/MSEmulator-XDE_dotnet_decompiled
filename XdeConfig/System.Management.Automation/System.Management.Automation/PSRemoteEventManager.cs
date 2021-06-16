using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000CD RID: 205
	internal class PSRemoteEventManager : PSEventManager
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x000437A8 File Offset: 0x000419A8
		internal PSRemoteEventManager(string computerName, Guid runspaceId)
		{
			this.computerName = computerName;
			this.runspaceId = runspaceId;
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000437BE File Offset: 0x000419BE
		public override List<PSEventSubscriber> Subscribers
		{
			get
			{
				throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000437CA File Offset: 0x000419CA
		protected override PSEventArgs CreateEvent(string sourceIdentifier, object sender, object[] args, PSObject extraData)
		{
			return new PSEventArgs(null, this.runspaceId, base.GetNextEventId(), sourceIdentifier, sender, args, extraData);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000437E4 File Offset: 0x000419E4
		internal override void AddForwardedEvent(PSEventArgs forwardedEvent)
		{
			forwardedEvent.EventIdentifier = base.GetNextEventId();
			forwardedEvent.ForwardEvent = false;
			if (forwardedEvent.ComputerName == null || forwardedEvent.ComputerName.Length == 0)
			{
				forwardedEvent.ComputerName = this.computerName;
				forwardedEvent.RunspaceId = this.runspaceId;
			}
			this.ProcessNewEvent(forwardedEvent, false);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00043839 File Offset: 0x00041A39
		protected override void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread)
		{
			this.ProcessNewEvent(newEvent, processInCurrentThread, false);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00043844 File Offset: 0x00041A44
		protected internal override void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread, bool waitForCompletionInCurrentThread)
		{
			lock (base.ReceivedEvents.SyncRoot)
			{
				if (newEvent.ForwardEvent)
				{
					this.OnForwardEvent(newEvent);
				}
				else
				{
					base.ReceivedEvents.Add(newEvent);
				}
			}
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000438A0 File Offset: 0x00041AA0
		public override IEnumerable<PSEventSubscriber> GetEventSubscribers(string sourceIdentifier)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000438AC File Offset: 0x00041AAC
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000438B8 File Offset: 0x00041AB8
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent, int maxTriggerCount)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000438C4 File Offset: 0x00041AC4
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000438D0 File Offset: 0x00041AD0
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, int maxTriggerCount)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000438DC File Offset: 0x00041ADC
		public override void UnsubscribeEvent(PSEventSubscriber subscriber)
		{
			throw new NotSupportedException(EventingResources.RemoteOperationNotSupported);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000BAD RID: 2989 RVA: 0x000438E8 File Offset: 0x00041AE8
		// (remove) Token: 0x06000BAE RID: 2990 RVA: 0x00043920 File Offset: 0x00041B20
		internal override event EventHandler<PSEventArgs> ForwardEvent;

		// Token: 0x06000BAF RID: 2991 RVA: 0x00043958 File Offset: 0x00041B58
		protected virtual void OnForwardEvent(PSEventArgs e)
		{
			EventHandler<PSEventArgs> forwardEvent = this.ForwardEvent;
			if (forwardEvent != null)
			{
				forwardEvent(this, e);
			}
		}

		// Token: 0x04000529 RID: 1321
		private string computerName;

		// Token: 0x0400052A RID: 1322
		private Guid runspaceId;
	}
}
