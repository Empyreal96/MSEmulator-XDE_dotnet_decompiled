using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000329 RID: 809
	internal class OpenRunspaceOperation : IThrottleOperation, IDisposable
	{
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000DAE7E File Offset: 0x000D907E
		internal RemoteRunspace OperatedRunspace
		{
			get
			{
				return this.runspace;
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000DAE88 File Offset: 0x000D9088
		internal OpenRunspaceOperation(RemoteRunspace runspace)
		{
			this.startComplete = true;
			this.stopComplete = true;
			this.runspace = runspace;
			this.runspace.StateChanged += this.HandleRunspaceStateChanged;
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000DAEE0 File Offset: 0x000D90E0
		internal override void StartOperation()
		{
			lock (this._syncObject)
			{
				this.startComplete = false;
			}
			this.runspace.OpenAsync();
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000DAF2C File Offset: 0x000D912C
		internal override void StopOperation()
		{
			OperationStateEventArgs operationStateEventArgs = null;
			lock (this._syncObject)
			{
				if (this.startComplete)
				{
					this.stopComplete = true;
					this.startComplete = true;
					operationStateEventArgs = new OperationStateEventArgs();
					operationStateEventArgs.BaseEvent = new RunspaceStateEventArgs(this.runspace.RunspaceStateInfo);
					operationStateEventArgs.OperationState = OperationState.StopComplete;
				}
				else
				{
					this.stopComplete = false;
				}
			}
			if (operationStateEventArgs != null)
			{
				this.FireEvent(operationStateEventArgs);
				return;
			}
			this.runspace.CloseAsync();
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x0600270F RID: 9999 RVA: 0x000DAFC0 File Offset: 0x000D91C0
		// (remove) Token: 0x06002710 RID: 10000 RVA: 0x000DB008 File Offset: 0x000D9208
		internal override event EventHandler<OperationStateEventArgs> OperationComplete
		{
			add
			{
				lock (this._internalCallbacks)
				{
					this._internalCallbacks.Add(value);
				}
			}
			remove
			{
				lock (this._internalCallbacks)
				{
					this._internalCallbacks.Remove(value);
				}
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000DB050 File Offset: 0x000D9250
		private void HandleRunspaceStateChanged(object source, RunspaceStateEventArgs stateEventArgs)
		{
			switch (stateEventArgs.RunspaceStateInfo.State)
			{
			case RunspaceState.BeforeOpen:
			case RunspaceState.Opening:
			case RunspaceState.Closing:
				return;
			}
			OperationStateEventArgs operationStateEventArgs = null;
			lock (this._syncObject)
			{
				if (!this.stopComplete)
				{
					this.stopComplete = true;
					this.startComplete = true;
					operationStateEventArgs = new OperationStateEventArgs();
					operationStateEventArgs.BaseEvent = stateEventArgs;
					operationStateEventArgs.OperationState = OperationState.StopComplete;
				}
				else if (!this.startComplete)
				{
					this.startComplete = true;
					operationStateEventArgs = new OperationStateEventArgs();
					operationStateEventArgs.BaseEvent = stateEventArgs;
					operationStateEventArgs.OperationState = OperationState.StartComplete;
				}
			}
			if (operationStateEventArgs != null)
			{
				this.FireEvent(operationStateEventArgs);
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000DB110 File Offset: 0x000D9310
		private void FireEvent(OperationStateEventArgs operationStateEventArgs)
		{
			EventHandler<OperationStateEventArgs>[] array;
			lock (this._internalCallbacks)
			{
				array = new EventHandler<OperationStateEventArgs>[this._internalCallbacks.Count];
				this._internalCallbacks.CopyTo(array);
			}
			foreach (EventHandler<OperationStateEventArgs> eventHandler in array)
			{
				try
				{
					eventHandler.SafeInvoke(this, operationStateEventArgs);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000DB1A4 File Offset: 0x000D93A4
		public void Dispose()
		{
			this.runspace.StateChanged -= this.HandleRunspaceStateChanged;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400134B RID: 4939
		private bool startComplete;

		// Token: 0x0400134C RID: 4940
		private bool stopComplete;

		// Token: 0x0400134D RID: 4941
		private object _syncObject = new object();

		// Token: 0x0400134E RID: 4942
		private RemoteRunspace runspace;

		// Token: 0x0400134F RID: 4943
		private List<EventHandler<OperationStateEventArgs>> _internalCallbacks = new List<EventHandler<OperationStateEventArgs>>();
	}
}
