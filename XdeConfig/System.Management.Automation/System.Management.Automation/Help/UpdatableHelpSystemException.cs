using System;
using System.Runtime.Serialization;

namespace System.Management.Automation.Help
{
	// Token: 0x020001DA RID: 474
	[Serializable]
	internal class UpdatableHelpSystemException : Exception
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x0008A599 File Offset: 0x00088799
		internal UpdatableHelpSystemException(string errorId, string message, ErrorCategory cat, object targetObject, Exception innerException) : base(message, innerException)
		{
			this._errorId = errorId;
			this._cat = cat;
			this._targetObject = targetObject;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0008A5BA File Offset: 0x000887BA
		protected UpdatableHelpSystemException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x0008A5C4 File Offset: 0x000887C4
		internal string FullyQualifiedErrorId
		{
			get
			{
				return this._errorId;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x0008A5CC File Offset: 0x000887CC
		internal ErrorCategory ErrorCategory
		{
			get
			{
				return this._cat;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x0008A5D4 File Offset: 0x000887D4
		internal object TargetObject
		{
			get
			{
				return this._targetObject;
			}
		}

		// Token: 0x04000941 RID: 2369
		private string _errorId;

		// Token: 0x04000942 RID: 2370
		private ErrorCategory _cat;

		// Token: 0x04000943 RID: 2371
		private object _targetObject;
	}
}
