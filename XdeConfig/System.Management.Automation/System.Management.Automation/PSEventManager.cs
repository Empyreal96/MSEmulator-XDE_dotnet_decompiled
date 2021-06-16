using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000CB RID: 203
	public abstract class PSEventManager
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00041EB8 File Offset: 0x000400B8
		protected int GetNextEventId()
		{
			return this.nextEventId++;
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00041ED6 File Offset: 0x000400D6
		public PSEventArgsCollection ReceivedEvents
		{
			get
			{
				return this.receivedEvents;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000B6B RID: 2923
		public abstract List<PSEventSubscriber> Subscribers { get; }

		// Token: 0x06000B6C RID: 2924
		protected abstract PSEventArgs CreateEvent(string sourceIdentifier, object sender, object[] args, PSObject extraData);

		// Token: 0x06000B6D RID: 2925 RVA: 0x00041EDE File Offset: 0x000400DE
		public PSEventArgs GenerateEvent(string sourceIdentifier, object sender, object[] args, PSObject extraData)
		{
			return this.GenerateEvent(sourceIdentifier, sender, args, extraData, false, false);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00041EF0 File Offset: 0x000400F0
		public PSEventArgs GenerateEvent(string sourceIdentifier, object sender, object[] args, PSObject extraData, bool processInCurrentThread, bool waitForCompletionInCurrentThread)
		{
			PSEventArgs pseventArgs = this.CreateEvent(sourceIdentifier, sender, args, extraData);
			this.ProcessNewEvent(pseventArgs, processInCurrentThread, waitForCompletionInCurrentThread);
			return pseventArgs;
		}

		// Token: 0x06000B6F RID: 2927
		internal abstract void AddForwardedEvent(PSEventArgs forwardedEvent);

		// Token: 0x06000B70 RID: 2928
		protected abstract void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread);

		// Token: 0x06000B71 RID: 2929 RVA: 0x00041F15 File Offset: 0x00040115
		protected internal virtual void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread, bool waitForCompletionWhenInCurrentThread)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B72 RID: 2930
		public abstract IEnumerable<PSEventSubscriber> GetEventSubscribers(string sourceIdentifier);

		// Token: 0x06000B73 RID: 2931
		public abstract PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent);

		// Token: 0x06000B74 RID: 2932
		public abstract PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent, int maxTriggerCount);

		// Token: 0x06000B75 RID: 2933
		public abstract PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent);

		// Token: 0x06000B76 RID: 2934
		public abstract PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, int maxTriggerCount);

		// Token: 0x06000B77 RID: 2935 RVA: 0x00041F1C File Offset: 0x0004011C
		internal virtual PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, bool shouldQueueAndProcessInExecutionThread, int maxTriggerCount = 0)
		{
			return this.SubscribeEvent(source, eventName, sourceIdentifier, data, handlerDelegate, supportEvent, forwardEvent, maxTriggerCount);
		}

		// Token: 0x06000B78 RID: 2936
		public abstract void UnsubscribeEvent(PSEventSubscriber subscriber);

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000B79 RID: 2937
		// (remove) Token: 0x06000B7A RID: 2938
		internal abstract event EventHandler<PSEventArgs> ForwardEvent;

		// Token: 0x04000514 RID: 1300
		private int nextEventId = 1;

		// Token: 0x04000515 RID: 1301
		private PSEventArgsCollection receivedEvents = new PSEventArgsCollection();
	}
}
