using System;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A43 RID: 2627
	internal class MISerializer
	{
		// Token: 0x0600697E RID: 27006 RVA: 0x00213C60 File Offset: 0x00211E60
		public MISerializer(int depth)
		{
			this.internalSerializer = new InternalMISerializer(depth);
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x00213C74 File Offset: 0x00211E74
		public CimInstance Serialize(object source)
		{
			return this.internalSerializer.Serialize(source);
		}

		// Token: 0x04003281 RID: 12929
		internal InternalMISerializer internalSerializer;
	}
}
