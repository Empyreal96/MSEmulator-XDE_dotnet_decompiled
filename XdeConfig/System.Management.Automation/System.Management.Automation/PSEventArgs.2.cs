using System;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020000D3 RID: 211
	public class PSEventArgs : EventArgs
	{
		// Token: 0x06000BD5 RID: 3029 RVA: 0x00043CFC File Offset: 0x00041EFC
		internal PSEventArgs(string computerName, Guid runspaceId, int eventIdentifier, string sourceIdentifier, object sender, object[] originalArgs, PSObject additionalData)
		{
			if (originalArgs != null)
			{
				foreach (object obj in originalArgs)
				{
					EventArgs eventArgs = obj as EventArgs;
					if (eventArgs != null)
					{
						this.sourceEventArgs = eventArgs;
						break;
					}
					if (ForwardedEventArgs.IsRemoteSourceEventArgs(obj))
					{
						this.sourceEventArgs = new ForwardedEventArgs((PSObject)obj);
						break;
					}
				}
			}
			this.computerName = computerName;
			this.runspaceId = runspaceId;
			this.eventIdentifier = eventIdentifier;
			this.sender = sender;
			this.sourceArgs = originalArgs;
			this.sourceIdentifier = sourceIdentifier;
			this.timeGenerated = DateTime.Now;
			this.data = additionalData;
			this.forwardEvent = false;
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x00043D9D File Offset: 0x00041F9D
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x00043DA5 File Offset: 0x00041FA5
		public string ComputerName
		{
			get
			{
				return this.computerName;
			}
			internal set
			{
				this.computerName = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00043DAE File Offset: 0x00041FAE
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x00043DB6 File Offset: 0x00041FB6
		public Guid RunspaceId
		{
			get
			{
				return this.runspaceId;
			}
			internal set
			{
				this.runspaceId = value;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00043DBF File Offset: 0x00041FBF
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x00043DC7 File Offset: 0x00041FC7
		public int EventIdentifier
		{
			get
			{
				return this.eventIdentifier;
			}
			internal set
			{
				this.eventIdentifier = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00043DD0 File Offset: 0x00041FD0
		public object Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00043DD8 File Offset: 0x00041FD8
		public EventArgs SourceEventArgs
		{
			get
			{
				return this.sourceEventArgs;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00043DE0 File Offset: 0x00041FE0
		public object[] SourceArgs
		{
			get
			{
				return this.sourceArgs;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00043DE8 File Offset: 0x00041FE8
		public string SourceIdentifier
		{
			get
			{
				return this.sourceIdentifier;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00043DF0 File Offset: 0x00041FF0
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x00043DF8 File Offset: 0x00041FF8
		public DateTime TimeGenerated
		{
			get
			{
				return this.timeGenerated;
			}
			internal set
			{
				this.timeGenerated = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00043E01 File Offset: 0x00042001
		public PSObject MessageData
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00043E09 File Offset: 0x00042009
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00043E11 File Offset: 0x00042011
		internal bool ForwardEvent
		{
			get
			{
				return this.forwardEvent;
			}
			set
			{
				this.forwardEvent = value;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00043E1A File Offset: 0x0004201A
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00043E22 File Offset: 0x00042022
		internal ManualResetEventSlim EventProcessed
		{
			get
			{
				return this.eventProcessed;
			}
			set
			{
				this.eventProcessed = value;
			}
		}

		// Token: 0x04000546 RID: 1350
		private string computerName;

		// Token: 0x04000547 RID: 1351
		private Guid runspaceId;

		// Token: 0x04000548 RID: 1352
		private int eventIdentifier;

		// Token: 0x04000549 RID: 1353
		private object sender;

		// Token: 0x0400054A RID: 1354
		private EventArgs sourceEventArgs;

		// Token: 0x0400054B RID: 1355
		private object[] sourceArgs;

		// Token: 0x0400054C RID: 1356
		private string sourceIdentifier;

		// Token: 0x0400054D RID: 1357
		private DateTime timeGenerated;

		// Token: 0x0400054E RID: 1358
		private PSObject data;

		// Token: 0x0400054F RID: 1359
		private bool forwardEvent;

		// Token: 0x04000550 RID: 1360
		private ManualResetEventSlim eventProcessed;
	}
}
