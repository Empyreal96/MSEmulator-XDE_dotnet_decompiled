using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020008A3 RID: 2211
	[Serializable]
	public class ProviderInvocationException : RuntimeException
	{
		// Token: 0x0600548D RID: 21645 RVA: 0x001BEE9E File Offset: 0x001BD09E
		public ProviderInvocationException()
		{
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x001BEEA6 File Offset: 0x001BD0A6
		protected ProviderInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x001BEEB0 File Offset: 0x001BD0B0
		public ProviderInvocationException(string message) : base(message)
		{
			this._message = message;
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x001BEEC0 File Offset: 0x001BD0C0
		internal ProviderInvocationException(ProviderInfo provider, Exception innerException) : base(RuntimeException.RetrieveMessage(innerException), innerException)
		{
			this._message = base.Message;
			this._providerInfo = provider;
			IContainsErrorRecord containsErrorRecord = innerException as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				this._errorRecord = new ErrorRecord(containsErrorRecord.ErrorRecord, innerException);
				return;
			}
			this._errorRecord = new ErrorRecord(innerException, "ErrorRecordNotSpecified", ErrorCategory.InvalidOperation, null);
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x001BEF25 File Offset: 0x001BD125
		internal ProviderInvocationException(ProviderInfo provider, ErrorRecord errorRecord) : base(RuntimeException.RetrieveMessage(errorRecord), RuntimeException.RetrieveException(errorRecord))
		{
			if (errorRecord == null)
			{
				throw new ArgumentNullException("errorRecord");
			}
			this._message = base.Message;
			this._providerInfo = provider;
			this._errorRecord = errorRecord;
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x001BEF61 File Offset: 0x001BD161
		public ProviderInvocationException(string message, Exception innerException) : base(message, innerException)
		{
			this._message = message;
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x001BEF72 File Offset: 0x001BD172
		internal ProviderInvocationException(string errorId, string resourceStr, ProviderInfo provider, string path, Exception innerException) : this(errorId, resourceStr, provider, path, innerException, true)
		{
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x001BEF84 File Offset: 0x001BD184
		internal ProviderInvocationException(string errorId, string resourceStr, ProviderInfo provider, string path, Exception innerException, bool useInnerExceptionMessage) : base(ProviderInvocationException.RetrieveMessage(errorId, resourceStr, provider, path, innerException), innerException)
		{
			this._providerInfo = provider;
			this._message = base.Message;
			Exception ex;
			if (useInnerExceptionMessage)
			{
				ex = innerException;
			}
			else
			{
				ex = new ParentContainsErrorRecordException(this);
			}
			IContainsErrorRecord containsErrorRecord = innerException as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				this._errorRecord = new ErrorRecord(containsErrorRecord.ErrorRecord, ex);
				return;
			}
			this._errorRecord = new ErrorRecord(ex, errorId, ErrorCategory.InvalidOperation, null);
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x06005495 RID: 21653 RVA: 0x001BEFFF File Offset: 0x001BD1FF
		public ProviderInfo ProviderInfo
		{
			get
			{
				return this._providerInfo;
			}
		}

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x06005496 RID: 21654 RVA: 0x001BF007 File Offset: 0x001BD207
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "ProviderInvocationException", ErrorCategory.NotSpecified, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001BF030 File Offset: 0x001BD230
		private static string RetrieveMessage(string errorId, string resourceStr, ProviderInfo provider, string path, Exception innerException)
		{
			if (innerException == null)
			{
				return "";
			}
			if (string.IsNullOrEmpty(errorId))
			{
				return RuntimeException.RetrieveMessage(innerException);
			}
			if (provider == null)
			{
				return RuntimeException.RetrieveMessage(innerException);
			}
			if (string.IsNullOrEmpty(resourceStr))
			{
				return RuntimeException.RetrieveMessage(innerException);
			}
			string result;
			if (path == null)
			{
				result = string.Format(CultureInfo.CurrentCulture, resourceStr, new object[]
				{
					provider.Name,
					RuntimeException.RetrieveMessage(innerException)
				});
			}
			else
			{
				result = string.Format(CultureInfo.CurrentCulture, resourceStr, new object[]
				{
					provider.Name,
					path,
					RuntimeException.RetrieveMessage(innerException)
				});
			}
			return result;
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x06005498 RID: 21656 RVA: 0x001BF0CE File Offset: 0x001BD2CE
		public override string Message
		{
			get
			{
				if (!string.IsNullOrEmpty(this._message))
				{
					return this._message;
				}
				return base.Message;
			}
		}

		// Token: 0x04002B6B RID: 11115
		[NonSerialized]
		internal ProviderInfo _providerInfo;

		// Token: 0x04002B6C RID: 11116
		[NonSerialized]
		private ErrorRecord _errorRecord;

		// Token: 0x04002B6D RID: 11117
		[NonSerialized]
		private string _message;
	}
}
