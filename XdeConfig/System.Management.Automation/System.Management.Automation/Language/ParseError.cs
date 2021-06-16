using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B3 RID: 1459
	public class ParseError
	{
		// Token: 0x06003E37 RID: 15927 RVA: 0x0014B3BC File Offset: 0x001495BC
		public ParseError(IScriptExtent extent, string errorId, string message) : this(extent, errorId, message, false)
		{
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x0014B3C8 File Offset: 0x001495C8
		internal ParseError(IScriptExtent extent, string errorId, string message, bool incompleteInput)
		{
			this._extent = extent;
			this._errorId = errorId;
			this._message = message;
			this._incompleteInput = incompleteInput;
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x0014B3ED File Offset: 0x001495ED
		public override string ToString()
		{
			return PositionUtilities.VerboseMessage(this._extent) + "\n" + this._message;
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06003E3A RID: 15930 RVA: 0x0014B40A File Offset: 0x0014960A
		public IScriptExtent Extent
		{
			get
			{
				return this._extent;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x0014B412 File Offset: 0x00149612
		public string ErrorId
		{
			get
			{
				return this._errorId;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06003E3C RID: 15932 RVA: 0x0014B41A File Offset: 0x0014961A
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0014B422 File Offset: 0x00149622
		public bool IncompleteInput
		{
			get
			{
				return this._incompleteInput;
			}
		}

		// Token: 0x04001F2C RID: 7980
		private readonly IScriptExtent _extent;

		// Token: 0x04001F2D RID: 7981
		private readonly string _errorId;

		// Token: 0x04001F2E RID: 7982
		private readonly string _message;

		// Token: 0x04001F2F RID: 7983
		private readonly bool _incompleteInput;
	}
}
