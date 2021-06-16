using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000D7 RID: 215
	public class PSEventArgsCollection : IEnumerable<PSEventArgs>, IEnumerable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000BF2 RID: 3058 RVA: 0x00043E4C File Offset: 0x0004204C
		// (remove) Token: 0x06000BF3 RID: 3059 RVA: 0x00043E84 File Offset: 0x00042084
		public event PSEventReceivedEventHandler PSEventReceived;

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00043EB9 File Offset: 0x000420B9
		internal void Add(PSEventArgs eventToAdd)
		{
			if (eventToAdd == null)
			{
				throw new ArgumentNullException("eventToAdd");
			}
			this.eventCollection.Add(eventToAdd);
			this.OnPSEventReceived(eventToAdd.Sender, eventToAdd);
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00043EE2 File Offset: 0x000420E2
		public int Count
		{
			get
			{
				return this.eventCollection.Count;
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00043EEF File Offset: 0x000420EF
		public void RemoveAt(int index)
		{
			this.eventCollection.RemoveAt(index);
		}

		// Token: 0x17000351 RID: 849
		public PSEventArgs this[int index]
		{
			get
			{
				return this.eventCollection[index];
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00043F0B File Offset: 0x0004210B
		private void OnPSEventReceived(object sender, PSEventArgs e)
		{
			if (this.PSEventReceived != null)
			{
				this.PSEventReceived(sender, e);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00043F22 File Offset: 0x00042122
		public IEnumerator<PSEventArgs> GetEnumerator()
		{
			return this.eventCollection.GetEnumerator();
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00043F34 File Offset: 0x00042134
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.eventCollection.GetEnumerator();
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00043F46 File Offset: 0x00042146
		public object SyncRoot
		{
			get
			{
				return this.syncRoot;
			}
		}

		// Token: 0x04000553 RID: 1363
		private List<PSEventArgs> eventCollection = new List<PSEventArgs>();

		// Token: 0x04000554 RID: 1364
		private object syncRoot = new object();
	}
}
