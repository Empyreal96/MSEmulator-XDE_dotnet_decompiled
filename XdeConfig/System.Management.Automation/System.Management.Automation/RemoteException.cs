using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200009B RID: 155
	[Serializable]
	public class RemoteException : RuntimeException
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x00024A24 File Offset: 0x00022C24
		public RemoteException()
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00024A2C File Offset: 0x00022C2C
		public RemoteException(string message) : base(message)
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00024A35 File Offset: 0x00022C35
		public RemoteException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00024A3F File Offset: 0x00022C3F
		internal RemoteException(string message, PSObject serializedRemoteException, PSObject serializedRemoteInvocationInfo) : base(message)
		{
			this._serializedRemoteException = serializedRemoteException;
			this._serializedRemoteInvocationInfo = serializedRemoteInvocationInfo;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00024A56 File Offset: 0x00022C56
		protected RemoteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00024A60 File Offset: 0x00022C60
		public PSObject SerializedRemoteException
		{
			get
			{
				return this._serializedRemoteException;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00024A68 File Offset: 0x00022C68
		public PSObject SerializedRemoteInvocationInfo
		{
			get
			{
				return this._serializedRemoteInvocationInfo;
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00024A70 File Offset: 0x00022C70
		internal void SetRemoteErrorRecord(ErrorRecord remoteError)
		{
			this._remoteErrorRecord = remoteError;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00024A79 File Offset: 0x00022C79
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._remoteErrorRecord != null)
				{
					return this._remoteErrorRecord;
				}
				return base.ErrorRecord;
			}
		}

		// Token: 0x0400036B RID: 875
		[NonSerialized]
		private PSObject _serializedRemoteException;

		// Token: 0x0400036C RID: 876
		[NonSerialized]
		private PSObject _serializedRemoteInvocationInfo;

		// Token: 0x0400036D RID: 877
		private ErrorRecord _remoteErrorRecord;
	}
}
