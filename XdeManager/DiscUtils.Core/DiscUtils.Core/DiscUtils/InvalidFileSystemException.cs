using System;
using System.IO;
using System.Runtime.Serialization;

namespace DiscUtils
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class InvalidFileSystemException : IOException
	{
		// Token: 0x06000125 RID: 293 RVA: 0x000034D0 File Offset: 0x000016D0
		public InvalidFileSystemException()
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000034D8 File Offset: 0x000016D8
		public InvalidFileSystemException(string message) : base(message)
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000034E1 File Offset: 0x000016E1
		public InvalidFileSystemException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000034EB File Offset: 0x000016EB
		protected InvalidFileSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
