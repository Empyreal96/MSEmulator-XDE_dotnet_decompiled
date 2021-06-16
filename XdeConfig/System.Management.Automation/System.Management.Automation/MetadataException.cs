using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200087E RID: 2174
	[Serializable]
	public class MetadataException : RuntimeException
	{
		// Token: 0x06005333 RID: 21299 RVA: 0x001BA11F File Offset: 0x001B831F
		protected MetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			base.SetErrorCategory(ErrorCategory.MetadataError);
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001BA131 File Offset: 0x001B8331
		public MetadataException() : base(typeof(MetadataException).FullName)
		{
			base.SetErrorCategory(ErrorCategory.MetadataError);
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001BA150 File Offset: 0x001B8350
		public MetadataException(string message) : base(message)
		{
			base.SetErrorCategory(ErrorCategory.MetadataError);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x001BA161 File Offset: 0x001B8361
		public MetadataException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCategory(ErrorCategory.MetadataError);
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x001BA173 File Offset: 0x001B8373
		internal MetadataException(string errorId, Exception innerException, string resourceStr, params object[] arguments) : base(StringUtil.Format(resourceStr, arguments), innerException)
		{
			base.SetErrorCategory(ErrorCategory.MetadataError);
			base.SetErrorId(errorId);
		}

		// Token: 0x04002AB9 RID: 10937
		internal const string MetadataMemberInitialization = "MetadataMemberInitialization";

		// Token: 0x04002ABA RID: 10938
		internal const string BaseName = "Metadata";
	}
}
