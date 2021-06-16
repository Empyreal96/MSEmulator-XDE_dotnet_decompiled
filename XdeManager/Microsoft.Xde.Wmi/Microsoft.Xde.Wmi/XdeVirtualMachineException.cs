using System;
using System.Runtime.Serialization;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class XdeVirtualMachineException : Exception
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x0000635F File Offset: 0x0000455F
		public XdeVirtualMachineException(string str) : base(str)
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006368 File Offset: 0x00004568
		public XdeVirtualMachineException(string str, Exception e) : base(str, e)
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006372 File Offset: 0x00004572
		protected XdeVirtualMachineException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
