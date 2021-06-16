using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000880 RID: 2176
	[Serializable]
	public class ArgumentTransformationMetadataException : MetadataException
	{
		// Token: 0x0600533F RID: 21311 RVA: 0x001BA1ED File Offset: 0x001B83ED
		protected ArgumentTransformationMetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x001BA1F7 File Offset: 0x001B83F7
		public ArgumentTransformationMetadataException() : base(typeof(ArgumentTransformationMetadataException).FullName)
		{
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x001BA20E File Offset: 0x001B840E
		public ArgumentTransformationMetadataException(string message) : base(message)
		{
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x001BA217 File Offset: 0x001B8417
		public ArgumentTransformationMetadataException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x001BA221 File Offset: 0x001B8421
		internal ArgumentTransformationMetadataException(string errorId, Exception innerException, string resourceStr, params object[] arguments) : base(errorId, innerException, resourceStr, arguments)
		{
		}

		// Token: 0x04002AD0 RID: 10960
		internal const string ArgumentTransformationArgumentsShouldBeStrings = "ArgumentTransformationArgumentsShouldBeStrings";
	}
}
