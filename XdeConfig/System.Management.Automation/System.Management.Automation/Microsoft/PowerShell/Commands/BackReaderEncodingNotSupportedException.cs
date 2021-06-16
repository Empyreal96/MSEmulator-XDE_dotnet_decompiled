using System;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000769 RID: 1897
	internal sealed class BackReaderEncodingNotSupportedException : NotSupportedException
	{
		// Token: 0x06004BCB RID: 19403 RVA: 0x0018D5C1 File Offset: 0x0018B7C1
		internal BackReaderEncodingNotSupportedException(string message, string encodingName) : base(message)
		{
			this._encodingName = encodingName;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x0018D5D1 File Offset: 0x0018B7D1
		internal BackReaderEncodingNotSupportedException(string encodingName)
		{
			this._encodingName = encodingName;
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06004BCD RID: 19405 RVA: 0x0018D5E0 File Offset: 0x0018B7E0
		internal string EncodingName
		{
			get
			{
				return this._encodingName;
			}
		}

		// Token: 0x040024A9 RID: 9385
		private readonly string _encodingName;
	}
}
