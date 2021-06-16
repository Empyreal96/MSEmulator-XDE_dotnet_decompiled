using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class PowerManagerException : Exception
	{
		// Token: 0x06000214 RID: 532 RVA: 0x00005D4E File Offset: 0x00003F4E
		public PowerManagerException()
		{
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00005D56 File Offset: 0x00003F56
		public PowerManagerException(string message) : base(message)
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00005D5F File Offset: 0x00003F5F
		public PowerManagerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00005D69 File Offset: 0x00003F69
		protected PowerManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
