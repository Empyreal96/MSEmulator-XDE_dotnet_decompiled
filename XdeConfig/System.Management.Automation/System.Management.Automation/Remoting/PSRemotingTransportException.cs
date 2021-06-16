using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C2 RID: 706
	[Serializable]
	public class PSRemotingTransportException : RuntimeException
	{
		// Token: 0x060021A5 RID: 8613 RVA: 0x000C0F64 File Offset: 0x000BF164
		public PSRemotingTransportException() : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.DefaultRemotingExceptionMessage, new object[]
		{
			typeof(PSRemotingTransportException).FullName
		}))
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000C0FA1 File Offset: 0x000BF1A1
		public PSRemotingTransportException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000C0FB0 File Offset: 0x000BF1B0
		public PSRemotingTransportException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000C0FC0 File Offset: 0x000BF1C0
		internal PSRemotingTransportException(PSRemotingErrorId errorId, string resourceString, params object[] args) : base(PSRemotingErrorInvariants.FormatResourceString(resourceString, args))
		{
			this.SetDefaultErrorRecord();
			this._errorCode = (int)errorId;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000C0FDC File Offset: 0x000BF1DC
		internal PSRemotingTransportException(Exception innerException, string resourceString, params object[] args) : base(PSRemotingErrorInvariants.FormatResourceString(resourceString, args), innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000C0FF2 File Offset: 0x000BF1F2
		protected PSRemotingTransportException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			this._errorCode = info.GetInt32("ErrorCode");
			this._transportMessage = info.GetString("TransportMessage");
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000C102C File Offset: 0x000BF22C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorCode", this._errorCode);
			info.AddValue("TransportMessage", this._transportMessage);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000C1066 File Offset: 0x000BF266
		protected void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.ResourceUnavailable);
			base.SetErrorId(typeof(PSRemotingDataStructureException).FullName);
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x000C1085 File Offset: 0x000BF285
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x000C108D File Offset: 0x000BF28D
		public int ErrorCode
		{
			get
			{
				return this._errorCode;
			}
			set
			{
				this._errorCode = value;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x000C1096 File Offset: 0x000BF296
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x000C109E File Offset: 0x000BF29E
		public string TransportMessage
		{
			get
			{
				return this._transportMessage;
			}
			set
			{
				this._transportMessage = value;
			}
		}

		// Token: 0x04000FF4 RID: 4084
		private int _errorCode;

		// Token: 0x04000FF5 RID: 4085
		private string _transportMessage;
	}
}
