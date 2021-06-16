using System;
using System.Collections;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020000CF RID: 207
	public class PSEventSubscriber : IEquatable<PSEventSubscriber>
	{
		// Token: 0x06000BB2 RID: 2994 RVA: 0x000439C4 File Offset: 0x00041BC4
		internal PSEventSubscriber(ExecutionContext context, int id, object source, string eventName, string sourceIdentifier, bool supportEvent, bool forwardEvent, int maxTriggerCount)
		{
			this.context = context;
			this.subscriptionId = id;
			this.sourceObject = source;
			this.eventName = eventName;
			this.sourceIdentifier = sourceIdentifier;
			this.supportEvent = supportEvent;
			this.forwardEvent = forwardEvent;
			this.IsBeingUnsubscribed = false;
			this.RemainingActionsToProcess = 0;
			if (maxTriggerCount <= 0)
			{
				this.AutoUnregister = false;
				this.RemainingTriggerCount = -1;
				return;
			}
			this.AutoUnregister = true;
			this.RemainingTriggerCount = maxTriggerCount;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00043A40 File Offset: 0x00041C40
		internal PSEventSubscriber(ExecutionContext context, int id, object source, string eventName, string sourceIdentifier, ScriptBlock action, bool supportEvent, bool forwardEvent, int maxTriggerCount) : this(context, id, source, eventName, sourceIdentifier, supportEvent, forwardEvent, maxTriggerCount)
		{
			if (action != null)
			{
				ScriptBlock scriptBlock = this.CreateBoundScriptBlock(action);
				this.action = new PSEventJob(context.Events, this, scriptBlock, sourceIdentifier);
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00043A84 File Offset: 0x00041C84
		internal void RegisterJob()
		{
			if (!this.supportEvent && this.Action != null)
			{
				JobRepository jobRepository = ((LocalRunspace)this.context.CurrentRunspace).JobRepository;
				jobRepository.Add(this.action);
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00043AC4 File Offset: 0x00041CC4
		internal PSEventSubscriber(ExecutionContext context, int id, object source, string eventName, string sourceIdentifier, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, int maxTriggerCount) : this(context, id, source, eventName, sourceIdentifier, supportEvent, forwardEvent, maxTriggerCount)
		{
			this.handlerDelegate = handlerDelegate;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00043AEC File Offset: 0x00041CEC
		private ScriptBlock CreateBoundScriptBlock(ScriptBlock scriptAction)
		{
			ScriptBlock scriptBlock = this.context.Modules.CreateBoundScriptBlock(this.context, scriptAction, true);
			PSVariable psvariable = new PSVariable("script:Error", new ArrayList(), ScopedItemOptions.Constant);
			SessionStateInternal sessionStateInternal = scriptBlock.SessionStateInternal;
			SessionStateScope scopeByID = sessionStateInternal.GetScopeByID("script");
			scopeByID.SetVariable(psvariable.Name, psvariable, false, true, sessionStateInternal, CommandOrigin.Internal, false);
			return scriptBlock;
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x00043B4A File Offset: 0x00041D4A
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x00043B52 File Offset: 0x00041D52
		public int SubscriptionId
		{
			get
			{
				return this.subscriptionId;
			}
			set
			{
				this.subscriptionId = value;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00043B5B File Offset: 0x00041D5B
		public object SourceObject
		{
			get
			{
				return this.sourceObject;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00043B63 File Offset: 0x00041D63
		public string EventName
		{
			get
			{
				return this.eventName;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00043B6B File Offset: 0x00041D6B
		public string SourceIdentifier
		{
			get
			{
				return this.sourceIdentifier;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00043B73 File Offset: 0x00041D73
		public PSEventJob Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00043B7B File Offset: 0x00041D7B
		public PSEventReceivedEventHandler HandlerDelegate
		{
			get
			{
				return this.handlerDelegate;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00043B83 File Offset: 0x00041D83
		public bool SupportEvent
		{
			get
			{
				return this.supportEvent;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00043B8B File Offset: 0x00041D8B
		public bool ForwardEvent
		{
			get
			{
				return this.forwardEvent;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00043B93 File Offset: 0x00041D93
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x00043B9B File Offset: 0x00041D9B
		internal bool ShouldProcessInExecutionThread
		{
			get
			{
				return this.shouldProcessInExecutionThread;
			}
			set
			{
				this.shouldProcessInExecutionThread = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00043BA4 File Offset: 0x00041DA4
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x00043BAC File Offset: 0x00041DAC
		internal bool AutoUnregister { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00043BB5 File Offset: 0x00041DB5
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00043BBD File Offset: 0x00041DBD
		internal int RemainingTriggerCount { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00043BC6 File Offset: 0x00041DC6
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x00043BCE File Offset: 0x00041DCE
		internal int RemainingActionsToProcess { get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00043BD7 File Offset: 0x00041DD7
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x00043BDF File Offset: 0x00041DDF
		internal bool IsBeingUnsubscribed { get; set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000BCA RID: 3018 RVA: 0x00043BE8 File Offset: 0x00041DE8
		// (remove) Token: 0x06000BCB RID: 3019 RVA: 0x00043C20 File Offset: 0x00041E20
		public event PSEventUnsubscribedEventHandler Unsubscribed;

		// Token: 0x06000BCC RID: 3020 RVA: 0x00043C55 File Offset: 0x00041E55
		public bool Equals(PSEventSubscriber other)
		{
			return other != null && object.Equals(this.SubscriptionId, other.SubscriptionId);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00043C77 File Offset: 0x00041E77
		public override int GetHashCode()
		{
			return this.SubscriptionId;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00043C7F File Offset: 0x00041E7F
		internal void OnPSEventUnsubscribed(object sender, PSEventUnsubscribedEventArgs e)
		{
			if (this.Unsubscribed != null)
			{
				this.Unsubscribed(sender, e);
			}
		}

		// Token: 0x04000531 RID: 1329
		private ExecutionContext context;

		// Token: 0x04000532 RID: 1330
		private int subscriptionId;

		// Token: 0x04000533 RID: 1331
		private object sourceObject;

		// Token: 0x04000534 RID: 1332
		private string eventName;

		// Token: 0x04000535 RID: 1333
		private string sourceIdentifier;

		// Token: 0x04000536 RID: 1334
		private PSEventJob action;

		// Token: 0x04000537 RID: 1335
		private PSEventReceivedEventHandler handlerDelegate;

		// Token: 0x04000538 RID: 1336
		private bool supportEvent;

		// Token: 0x04000539 RID: 1337
		private bool forwardEvent;

		// Token: 0x0400053A RID: 1338
		private bool shouldProcessInExecutionThread;
	}
}
