using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation
{
	// Token: 0x020002BC RID: 700
	internal class RemoteSessionStateMachineEventArgs : EventArgs
	{
		// Token: 0x06002195 RID: 8597 RVA: 0x000C0E48 File Offset: 0x000BF048
		internal RemoteSessionStateMachineEventArgs(RemoteSessionEvent stateEvent) : this(stateEvent, null)
		{
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000C0E52 File Offset: 0x000BF052
		internal RemoteSessionStateMachineEventArgs(RemoteSessionEvent stateEvent, Exception reason)
		{
			this._stateEvent = stateEvent;
			this._reason = reason;
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x000C0E68 File Offset: 0x000BF068
		internal RemoteSessionEvent StateEvent
		{
			get
			{
				return this._stateEvent;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x000C0E70 File Offset: 0x000BF070
		internal Exception Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x000C0E78 File Offset: 0x000BF078
		// (set) Token: 0x0600219A RID: 8602 RVA: 0x000C0E80 File Offset: 0x000BF080
		internal RemoteSessionCapability RemoteSessionCapability
		{
			get
			{
				return this._capability;
			}
			set
			{
				this._capability = value;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x000C0E89 File Offset: 0x000BF089
		// (set) Token: 0x0600219C RID: 8604 RVA: 0x000C0E91 File Offset: 0x000BF091
		internal RemoteDataObject<PSObject> RemoteData
		{
			get
			{
				return this._remoteObject;
			}
			set
			{
				this._remoteObject = value;
			}
		}

		// Token: 0x04000F20 RID: 3872
		private RemoteSessionEvent _stateEvent;

		// Token: 0x04000F21 RID: 3873
		private RemoteSessionCapability _capability;

		// Token: 0x04000F22 RID: 3874
		private RemoteDataObject<PSObject> _remoteObject;

		// Token: 0x04000F23 RID: 3875
		private Exception _reason;
	}
}
