using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000016 RID: 22
	public class NetworkConnectionCollection : IEnumerable<NetworkConnection>, IEnumerable
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00003797 File Offset: 0x00001997
		internal NetworkConnectionCollection(IEnumerable networkConnectionEnumerable)
		{
			this.networkConnectionEnumerable = networkConnectionEnumerable;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000037A6 File Offset: 0x000019A6
		public IEnumerator<NetworkConnection> GetEnumerator()
		{
			foreach (object obj in this.networkConnectionEnumerable)
			{
				INetworkConnection networkConnection = (INetworkConnection)obj;
				yield return new NetworkConnection(networkConnection);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000037B5 File Offset: 0x000019B5
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (object obj in this.networkConnectionEnumerable)
			{
				INetworkConnection networkConnection = (INetworkConnection)obj;
				yield return new NetworkConnection(networkConnection);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000107 RID: 263
		private IEnumerable networkConnectionEnumerable;
	}
}
