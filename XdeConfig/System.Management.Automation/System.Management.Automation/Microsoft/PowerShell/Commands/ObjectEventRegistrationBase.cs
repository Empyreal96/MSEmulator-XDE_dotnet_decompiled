using System;
using System.Globalization;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000DB RID: 219
	public abstract class ObjectEventRegistrationBase : PSCmdlet
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x000459A7 File Offset: 0x00043BA7
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x000459AF File Offset: 0x00043BAF
		[Parameter(Position = 100)]
		public string SourceIdentifier
		{
			get
			{
				return this.sourceIdentifier;
			}
			set
			{
				this.sourceIdentifier = value;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000459B8 File Offset: 0x00043BB8
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x000459C0 File Offset: 0x00043BC0
		[Parameter(Position = 101)]
		public ScriptBlock Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000459C9 File Offset: 0x00043BC9
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x000459D1 File Offset: 0x00043BD1
		[Parameter]
		public PSObject MessageData
		{
			get
			{
				return this.messageData;
			}
			set
			{
				this.messageData = value;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000459DA File Offset: 0x00043BDA
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x000459E2 File Offset: 0x00043BE2
		[Parameter]
		public SwitchParameter SupportEvent
		{
			get
			{
				return this.supportEvent;
			}
			set
			{
				this.supportEvent = value;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x000459EB File Offset: 0x00043BEB
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x000459F3 File Offset: 0x00043BF3
		[Parameter]
		public SwitchParameter Forward
		{
			get
			{
				return this.forward;
			}
			set
			{
				this.forward = value;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000459FC File Offset: 0x00043BFC
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x00045A04 File Offset: 0x00043C04
		[Parameter]
		public int MaxTriggerCount
		{
			get
			{
				return this._maxTriggerCount;
			}
			set
			{
				this._maxTriggerCount = ((value <= 0) ? 0 : value);
			}
		}

		// Token: 0x06000C74 RID: 3188
		protected abstract object GetSourceObject();

		// Token: 0x06000C75 RID: 3189
		protected abstract string GetSourceObjectEventName();

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00045A14 File Offset: 0x00043C14
		protected PSEventSubscriber NewSubscriber
		{
			get
			{
				return this.newSubscriber;
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00045A1C File Offset: 0x00043C1C
		protected override void BeginProcessing()
		{
			if (this.forward && this.action != null)
			{
				base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(EventingResources.ActionAndForwardNotSupported), "ACTION_AND_FORWARD_NOT_SUPPORTED", ErrorCategory.InvalidOperation, null));
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00045A50 File Offset: 0x00043C50
		protected override void EndProcessing()
		{
			object obj = PSObject.Base(this.GetSourceObject());
			string sourceObjectEventName = this.GetSourceObjectEventName();
			try
			{
				if ((obj != null || sourceObjectEventName != null) && base.Events.GetEventSubscribers(this.sourceIdentifier).GetEnumerator().MoveNext())
				{
					ErrorRecord errorRecord = new ErrorRecord(new ArgumentException(string.Format(CultureInfo.CurrentCulture, EventingResources.SubscriberExists, new object[]
					{
						this.sourceIdentifier
					})), "SUBSCRIBER_EXISTS", ErrorCategory.InvalidArgument, obj);
					base.WriteError(errorRecord);
				}
				else
				{
					this.newSubscriber = base.Events.SubscribeEvent(obj, sourceObjectEventName, this.sourceIdentifier, this.messageData, this.action, this.supportEvent, this.forward, this._maxTriggerCount);
					if (this.action != null && !this.supportEvent)
					{
						base.WriteObject(this.newSubscriber.Action);
					}
				}
			}
			catch (ArgumentException exception)
			{
				ErrorRecord errorRecord2 = new ErrorRecord(exception, "INVALID_REGISTRATION", ErrorCategory.InvalidArgument, obj);
				base.WriteError(errorRecord2);
			}
			catch (InvalidOperationException exception2)
			{
				ErrorRecord errorRecord3 = new ErrorRecord(exception2, "INVALID_REGISTRATION", ErrorCategory.InvalidOperation, obj);
				base.WriteError(errorRecord3);
			}
		}

		// Token: 0x04000583 RID: 1411
		private string sourceIdentifier = Guid.NewGuid().ToString();

		// Token: 0x04000584 RID: 1412
		private ScriptBlock action;

		// Token: 0x04000585 RID: 1413
		private PSObject messageData;

		// Token: 0x04000586 RID: 1414
		private SwitchParameter supportEvent = default(SwitchParameter);

		// Token: 0x04000587 RID: 1415
		private SwitchParameter forward = default(SwitchParameter);

		// Token: 0x04000588 RID: 1416
		private int _maxTriggerCount;

		// Token: 0x04000589 RID: 1417
		private PSEventSubscriber newSubscriber;
	}
}
