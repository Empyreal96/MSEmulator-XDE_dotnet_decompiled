using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000014 RID: 20
	public class NetworkCollection : IEnumerable<Network>, IEnumerable
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x000036FB File Offset: 0x000018FB
		internal NetworkCollection(IEnumerable networkEnumerable)
		{
			this.networkEnumerable = networkEnumerable;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000370A File Offset: 0x0000190A
		public IEnumerator<Network> GetEnumerator()
		{
			foreach (object obj in this.networkEnumerable)
			{
				INetwork network = (INetwork)obj;
				yield return new Network(network);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003719 File Offset: 0x00001919
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (object obj in this.networkEnumerable)
			{
				INetwork network = (INetwork)obj;
				yield return new Network(network);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000105 RID: 261
		private IEnumerable networkEnumerable;
	}
}
