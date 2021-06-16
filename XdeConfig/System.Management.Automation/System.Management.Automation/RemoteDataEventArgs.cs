using System;
using System.Management.Automation.Remoting;

namespace System.Management.Automation
{
	// Token: 0x020002B6 RID: 694
	internal sealed class RemoteDataEventArgs : EventArgs
	{
		// Token: 0x0600218A RID: 8586 RVA: 0x000C0D92 File Offset: 0x000BEF92
		internal RemoteDataEventArgs(RemoteDataObject<PSObject> receivedData)
		{
			if (receivedData == null)
			{
				throw PSTraceSource.NewArgumentNullException("receivedData");
			}
			this._rcvdData = receivedData;
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000C0DAF File Offset: 0x000BEFAF
		public RemoteDataObject<PSObject> ReceivedData
		{
			get
			{
				return this._rcvdData;
			}
		}

		// Token: 0x04000EE3 RID: 3811
		private RemoteDataObject<PSObject> _rcvdData;
	}
}
