using System;

namespace System.Management.Automation
{
	// Token: 0x020000D1 RID: 209
	public class ForwardedEventArgs : EventArgs
	{
		// Token: 0x06000BD1 RID: 3025 RVA: 0x00043CC3 File Offset: 0x00041EC3
		internal ForwardedEventArgs(PSObject serializedRemoteEventArgs)
		{
			this.serializedRemoteEventArgs = serializedRemoteEventArgs;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00043CD2 File Offset: 0x00041ED2
		public PSObject SerializedRemoteEventArgs
		{
			get
			{
				return this.serializedRemoteEventArgs;
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00043CDA File Offset: 0x00041EDA
		internal static bool IsRemoteSourceEventArgs(object argument)
		{
			return Deserializer.IsDeserializedInstanceOfType(argument, typeof(EventArgs));
		}

		// Token: 0x04000544 RID: 1348
		private PSObject serializedRemoteEventArgs;
	}
}
