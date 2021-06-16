using System;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x020002A4 RID: 676
	internal sealed class PSConnectionRetryStatusEventArgs : EventArgs
	{
		// Token: 0x06002089 RID: 8329 RVA: 0x000BC981 File Offset: 0x000BAB81
		internal PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus notification, string computerName, int maxRetryConnectionTime, object infoRecord)
		{
			this._notification = notification;
			this._computerName = computerName;
			this._maxRetryConnectionTime = maxRetryConnectionTime;
			this._infoRecord = infoRecord;
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000BC9A6 File Offset: 0x000BABA6
		internal PSConnectionRetryStatus Notification
		{
			get
			{
				return this._notification;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600208B RID: 8331 RVA: 0x000BC9AE File Offset: 0x000BABAE
		internal string ComputerName
		{
			get
			{
				return this._computerName;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000BC9B6 File Offset: 0x000BABB6
		internal int MaxRetryConnectionTime
		{
			get
			{
				return this._maxRetryConnectionTime;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x000BC9BE File Offset: 0x000BABBE
		internal object InformationRecord
		{
			get
			{
				return this._infoRecord;
			}
		}

		// Token: 0x04000E5E RID: 3678
		internal const string FQIDNetworkFailureDetected = "PowerShellNetworkFailureDetected";

		// Token: 0x04000E5F RID: 3679
		internal const string FQIDConnectionRetryAttempt = "PowerShellConnectionRetryAttempt";

		// Token: 0x04000E60 RID: 3680
		internal const string FQIDConnectionRetrySucceeded = "PowerShellConnectionRetrySucceeded";

		// Token: 0x04000E61 RID: 3681
		internal const string FQIDAutoDisconnectStarting = "PowerShellNetworkFailedStartDisconnect";

		// Token: 0x04000E62 RID: 3682
		internal const string FQIDAutoDisconnectSucceeded = "PowerShellAutoDisconnectSucceeded";

		// Token: 0x04000E63 RID: 3683
		internal const string FQIDNetworkOrDisconnectFailed = "PowerShellNetworkOrDisconnectFailed";

		// Token: 0x04000E64 RID: 3684
		private PSConnectionRetryStatus _notification;

		// Token: 0x04000E65 RID: 3685
		private string _computerName;

		// Token: 0x04000E66 RID: 3686
		private int _maxRetryConnectionTime;

		// Token: 0x04000E67 RID: 3687
		private object _infoRecord;
	}
}
