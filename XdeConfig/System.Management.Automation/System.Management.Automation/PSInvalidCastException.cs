using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000172 RID: 370
	[Serializable]
	public class PSInvalidCastException : InvalidCastException, IContainsErrorRecord
	{
		// Token: 0x060012A1 RID: 4769 RVA: 0x000741D9 File Offset: 0x000723D9
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorId", this.errorId);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00074202 File Offset: 0x00072402
		protected PSInvalidCastException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.errorId = info.GetString("ErrorId");
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00074228 File Offset: 0x00072428
		public PSInvalidCastException() : base(typeof(PSInvalidCastException).FullName)
		{
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0007424A File Offset: 0x0007244A
		public PSInvalidCastException(string message) : base(message)
		{
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0007425E File Offset: 0x0007245E
		public PSInvalidCastException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00074273 File Offset: 0x00072473
		internal PSInvalidCastException(string errorId, string message, Exception innerException) : base(message, innerException)
		{
			this.errorId = errorId;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0007428F File Offset: 0x0007248F
		internal PSInvalidCastException(string errorId, Exception innerException, string resourceString, params object[] arguments) : this(errorId, StringUtil.Format(resourceString, arguments), innerException)
		{
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x000742A1 File Offset: 0x000724A1
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this.errorRecord == null)
				{
					this.errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this.errorId, ErrorCategory.InvalidArgument, null);
				}
				return this.errorRecord;
			}
		}

		// Token: 0x040007EE RID: 2030
		private ErrorRecord errorRecord;

		// Token: 0x040007EF RID: 2031
		private string errorId = "PSInvalidCastException";
	}
}
