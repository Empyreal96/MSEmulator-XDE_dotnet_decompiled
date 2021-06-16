using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C3 RID: 707
	[Serializable]
	public class PSRemotingTransportRedirectException : PSRemotingTransportException
	{
		// Token: 0x060021B1 RID: 8625 RVA: 0x000C10A8 File Offset: 0x000BF2A8
		public PSRemotingTransportRedirectException() : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.DefaultRemotingExceptionMessage, new object[]
		{
			typeof(PSRemotingTransportRedirectException).FullName
		}))
		{
			base.SetDefaultErrorRecord();
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000C10E5 File Offset: 0x000BF2E5
		public PSRemotingTransportRedirectException(string message) : base(message)
		{
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000C10EE File Offset: 0x000BF2EE
		public PSRemotingTransportRedirectException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000C10F8 File Offset: 0x000BF2F8
		internal PSRemotingTransportRedirectException(Exception innerException, string resourceString, params object[] args) : base(innerException, resourceString, args)
		{
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000C1103 File Offset: 0x000BF303
		protected PSRemotingTransportRedirectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			this.redirectLocation = info.GetString("RedirectLocation");
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000C112C File Offset: 0x000BF32C
		internal PSRemotingTransportRedirectException(string redirectLocation, PSRemotingErrorId errorId, string resourceString, params object[] args) : base(errorId, resourceString, args)
		{
			this.redirectLocation = redirectLocation;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000C113F File Offset: 0x000BF33F
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("RedirectLocation", this.redirectLocation);
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000C1168 File Offset: 0x000BF368
		public string RedirectLocation
		{
			get
			{
				return this.redirectLocation;
			}
		}

		// Token: 0x04000FF6 RID: 4086
		private string redirectLocation;
	}
}
