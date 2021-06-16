using System;
using System.Runtime.Serialization;
using System.Text;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BC RID: 2236
	[Serializable]
	internal class PSCryptoException : Exception
	{
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x001C0886 File Offset: 0x001BEA86
		internal uint ErrorCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x001C088E File Offset: 0x001BEA8E
		public PSCryptoException() : this(0U, new StringBuilder(string.Empty))
		{
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x001C08A1 File Offset: 0x001BEAA1
		public PSCryptoException(uint errorCode, StringBuilder message) : base(message.ToString())
		{
			this._errorCode = errorCode;
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x001C08B6 File Offset: 0x001BEAB6
		public PSCryptoException(string message) : this(message, null)
		{
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x001C08C0 File Offset: 0x001BEAC0
		public PSCryptoException(string message, Exception innerException) : base(message, innerException)
		{
			this._errorCode = uint.MaxValue;
		}

		// Token: 0x060054E9 RID: 21737 RVA: 0x001C08D1 File Offset: 0x001BEAD1
		protected PSCryptoException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorCode = 268435455U;
		}

		// Token: 0x060054EA RID: 21738 RVA: 0x001C08E6 File Offset: 0x001BEAE6
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x04002C11 RID: 11281
		private uint _errorCode;
	}
}
