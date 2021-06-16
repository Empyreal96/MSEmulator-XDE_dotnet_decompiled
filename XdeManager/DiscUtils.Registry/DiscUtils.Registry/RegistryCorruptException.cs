using System;
using System.Runtime.Serialization;

namespace DiscUtils.Registry
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class RegistryCorruptException : Exception
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002D9E File Offset: 0x00000F9E
		public RegistryCorruptException()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002DA6 File Offset: 0x00000FA6
		public RegistryCorruptException(string message) : base(message)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002DAF File Offset: 0x00000FAF
		public RegistryCorruptException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DB9 File Offset: 0x00000FB9
		protected RegistryCorruptException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
