using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000881 RID: 2177
	[Serializable]
	public class ParsingMetadataException : MetadataException
	{
		// Token: 0x06005344 RID: 21316 RVA: 0x001BA22E File Offset: 0x001B842E
		protected ParsingMetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001BA238 File Offset: 0x001B8438
		public ParsingMetadataException() : base(typeof(ParsingMetadataException).FullName)
		{
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x001BA24F File Offset: 0x001B844F
		public ParsingMetadataException(string message) : base(message)
		{
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x001BA258 File Offset: 0x001B8458
		public ParsingMetadataException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x001BA262 File Offset: 0x001B8462
		internal ParsingMetadataException(string errorId, Exception innerException, string resourceStr, params object[] arguments) : base(errorId, innerException, resourceStr, arguments)
		{
		}

		// Token: 0x04002AD1 RID: 10961
		internal const string ParsingTooManyParameterSets = "ParsingTooManyParameterSets";
	}
}
