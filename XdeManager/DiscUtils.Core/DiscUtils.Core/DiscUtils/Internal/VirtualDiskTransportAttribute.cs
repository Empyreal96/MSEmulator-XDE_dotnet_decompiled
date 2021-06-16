using System;

namespace DiscUtils.Internal
{
	// Token: 0x02000077 RID: 119
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal sealed class VirtualDiskTransportAttribute : Attribute
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x0000CE66 File Offset: 0x0000B066
		public VirtualDiskTransportAttribute(string scheme)
		{
			this.Scheme = scheme;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000CE75 File Offset: 0x0000B075
		public string Scheme { get; }
	}
}
