using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000218 RID: 536
	public abstract class Pipeline : IDisposable
	{
		// Token: 0x06001921 RID: 6433 RVA: 0x000984B6 File Offset: 0x000966B6
		internal Pipeline(Runspace runspace) : this(runspace, new CommandCollection())
		{
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x000984C4 File Offset: 0x000966C4
		internal Pipeline(Runspace runspace, CommandCollection command)
		{
			if (runspace == null)
			{
				PSTraceSource.NewArgumentNullException("runspace");
			}
			this._pipelineId = runspace.GeneratePipelineId();
			this._commands = command;
			AmsiUtils.CloseSession();
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001923 RID: 6435
		public abstract Runspace Runspace { get; }

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001924 RID: 6436
		public abstract bool IsNested { get; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x000984F9 File Offset: 0x000966F9
		// (set) Token: 0x06001926 RID: 6438 RVA: 0x000984FC File Offset: 0x000966FC
		internal virtual bool IsChild
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001927 RID: 6439
		public abstract PipelineWriter Input { get; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001928 RID: 6440
		public abstract PipelineReader<PSObject> Output { get; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001929 RID: 6441
		public abstract PipelineReader<object> Error { get; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600192A RID: 6442
		public abstract PipelineStateInfo PipelineStateInfo { get; }

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x000984FE File Offset: 0x000966FE
		public virtual bool HadErrors
		{
			get
			{
				return this._hadErrors;
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00098506 File Offset: 0x00096706
		internal void SetHadErrors(bool status)
		{
			this._hadErrors = (this._hadErrors || status);
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x0009851A File Offset: 0x0009671A
		public long InstanceId
		{
			get
			{
				return this._pipelineId;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x00098522 File Offset: 0x00096722
		public CommandCollection Commands
		{
			get
			{
				return this._commands;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x0009852A File Offset: 0x0009672A
		// (set) Token: 0x06001930 RID: 6448 RVA: 0x00098532 File Offset: 0x00096732
		public bool SetPipelineSessionState
		{
			get
			{
				return this._setPipelineSessionState;
			}
			set
			{
				this._setPipelineSessionState = value;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0009853B File Offset: 0x0009673B
		// (set) Token: 0x06001932 RID: 6450 RVA: 0x00098543 File Offset: 0x00096743
		internal PSInvocationSettings InvocationSettings
		{
			get
			{
				return this._invocationSettings;
			}
			set
			{
				this._invocationSettings = value;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x0009854C File Offset: 0x0009674C
		// (set) Token: 0x06001934 RID: 6452 RVA: 0x00098554 File Offset: 0x00096754
		internal bool RedirectShellErrorOutputPipe
		{
			get
			{
				return this._redirectShellErrorOutputPipe;
			}
			set
			{
				this._redirectShellErrorOutputPipe = value;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001935 RID: 6453
		// (remove) Token: 0x06001936 RID: 6454
		public abstract event EventHandler<PipelineStateEventArgs> StateChanged;

		// Token: 0x06001937 RID: 6455 RVA: 0x0009855D File Offset: 0x0009675D
		public Collection<PSObject> Invoke()
		{
			return this.Invoke(null);
		}

		// Token: 0x06001938 RID: 6456
		public abstract Collection<PSObject> Invoke(IEnumerable input);

		// Token: 0x06001939 RID: 6457
		public abstract void InvokeAsync();

		// Token: 0x0600193A RID: 6458
		public abstract void Stop();

		// Token: 0x0600193B RID: 6459
		public abstract void StopAsync();

		// Token: 0x0600193C RID: 6460
		public abstract Pipeline Copy();

		// Token: 0x0600193D RID: 6461
		public abstract Collection<PSObject> Connect();

		// Token: 0x0600193E RID: 6462
		public abstract void ConnectAsync();

		// Token: 0x0600193F RID: 6463 RVA: 0x00098566 File Offset: 0x00096766
		internal void SetCommandCollection(CommandCollection commands)
		{
			this._commands = commands;
		}

		// Token: 0x06001940 RID: 6464
		internal abstract void SetHistoryString(string historyString);

		// Token: 0x06001941 RID: 6465
		internal abstract void InvokeAsyncAndDisconnect();

		// Token: 0x06001942 RID: 6466 RVA: 0x0009856F File Offset: 0x0009676F
		internal virtual void SuspendIncomingData()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00098576 File Offset: 0x00096776
		internal virtual void ResumeIncomingData()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0009857D File Offset: 0x0009677D
		internal virtual void DrainIncomingData()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00098584 File Offset: 0x00096784
		public void Dispose()
		{
			this.Dispose(!this.IsChild);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0009859B File Offset: 0x0009679B
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x04000A57 RID: 2647
		private long _pipelineId;

		// Token: 0x04000A58 RID: 2648
		private bool _hadErrors;

		// Token: 0x04000A59 RID: 2649
		private CommandCollection _commands;

		// Token: 0x04000A5A RID: 2650
		private bool _setPipelineSessionState = true;

		// Token: 0x04000A5B RID: 2651
		private PSInvocationSettings _invocationSettings;

		// Token: 0x04000A5C RID: 2652
		private bool _redirectShellErrorOutputPipe;
	}
}
