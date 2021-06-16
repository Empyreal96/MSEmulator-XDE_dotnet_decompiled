using System;
using System.Runtime.Serialization;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C1 RID: 705
	[Serializable]
	public class PSRemotingDataStructureException : RuntimeException
	{
		// Token: 0x0600219E RID: 8606 RVA: 0x000C0EB4 File Offset: 0x000BF0B4
		public PSRemotingDataStructureException() : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.DefaultRemotingExceptionMessage, new object[]
		{
			typeof(PSRemotingDataStructureException).FullName
		}))
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000C0EF1 File Offset: 0x000BF0F1
		public PSRemotingDataStructureException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000C0F00 File Offset: 0x000BF100
		public PSRemotingDataStructureException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000C0F10 File Offset: 0x000BF110
		internal PSRemotingDataStructureException(string resourceString, params object[] args) : base(PSRemotingErrorInvariants.FormatResourceString(resourceString, args))
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000C0F25 File Offset: 0x000BF125
		internal PSRemotingDataStructureException(Exception innerException, string resourceString, params object[] args) : base(PSRemotingErrorInvariants.FormatResourceString(resourceString, args), innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000C0F3B File Offset: 0x000BF13B
		protected PSRemotingDataStructureException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000C0F45 File Offset: 0x000BF145
		private void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
			base.SetErrorId(typeof(PSRemotingDataStructureException).FullName);
		}
	}
}
