using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200022B RID: 555
	[Serializable]
	public class InvalidPipelineStateException : SystemException
	{
		// Token: 0x06001A0C RID: 6668 RVA: 0x0009B5D9 File Offset: 0x000997D9
		public InvalidPipelineStateException() : base(StringUtil.Format(RunspaceStrings.InvalidPipelineStateStateGeneral, new object[0]))
		{
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0009B5F1 File Offset: 0x000997F1
		public InvalidPipelineStateException(string message) : base(message)
		{
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0009B5FA File Offset: 0x000997FA
		public InvalidPipelineStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0009B604 File Offset: 0x00099804
		internal InvalidPipelineStateException(string message, PipelineState currentState, PipelineState expectedState) : base(message)
		{
			this._expectedState = expectedState;
			this._currentState = currentState;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0009B61B File Offset: 0x0009981B
		private InvalidPipelineStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0009B625 File Offset: 0x00099825
		public PipelineState CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0009B62D File Offset: 0x0009982D
		public PipelineState ExpectedState
		{
			get
			{
				return this._expectedState;
			}
		}

		// Token: 0x04000AB6 RID: 2742
		[NonSerialized]
		private PipelineState _currentState;

		// Token: 0x04000AB7 RID: 2743
		[NonSerialized]
		private PipelineState _expectedState;
	}
}
