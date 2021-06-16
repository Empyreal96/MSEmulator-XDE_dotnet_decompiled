using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000439 RID: 1081
	[Serializable]
	public class WildcardPatternException : RuntimeException
	{
		// Token: 0x06002F86 RID: 12166 RVA: 0x00104BB2 File Offset: 0x00102DB2
		internal WildcardPatternException(ErrorRecord errorRecord) : base(RuntimeException.RetrieveMessage(errorRecord))
		{
			if (errorRecord == null)
			{
				throw new ArgumentNullException("errorRecord");
			}
			this._errorRecord = errorRecord;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x00104BD5 File Offset: 0x00102DD5
		public WildcardPatternException()
		{
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x00104BDD File Offset: 0x00102DDD
		public WildcardPatternException(string message) : base(message)
		{
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x00104BE6 File Offset: 0x00102DE6
		public WildcardPatternException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x00104BF0 File Offset: 0x00102DF0
		protected WildcardPatternException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040019C4 RID: 6596
		[NonSerialized]
		private ErrorRecord _errorRecord;
	}
}
