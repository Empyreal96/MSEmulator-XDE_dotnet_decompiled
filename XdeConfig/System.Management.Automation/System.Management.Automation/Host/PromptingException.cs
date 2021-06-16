using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation.Host
{
	// Token: 0x0200087A RID: 2170
	[Serializable]
	public class PromptingException : HostException
	{
		// Token: 0x0600530B RID: 21259 RVA: 0x001B9EEA File Offset: 0x001B80EA
		public PromptingException() : base(StringUtil.Format(HostInterfaceExceptionsStrings.DefaultCtorMessageTemplate, typeof(PromptingException).FullName))
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001B9F11 File Offset: 0x001B8111
		public PromptingException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x001B9F20 File Offset: 0x001B8120
		public PromptingException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x001B9F30 File Offset: 0x001B8130
		public PromptingException(string message, Exception innerException, string errorId, ErrorCategory errorCategory) : base(message, innerException, errorId, errorCategory)
		{
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x001B9F3D File Offset: 0x001B813D
		protected PromptingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x001B9F47 File Offset: 0x001B8147
		private void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
			base.SetErrorId(typeof(PromptingException).FullName);
		}
	}
}
