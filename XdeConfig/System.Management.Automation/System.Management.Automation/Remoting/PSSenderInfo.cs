using System;
using System.Runtime.Serialization;
using Microsoft.PowerShell;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000358 RID: 856
	[Serializable]
	public sealed class PSSenderInfo : ISerializable
	{
		// Token: 0x06002A91 RID: 10897 RVA: 0x000EB254 File Offset: 0x000E9454
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			PSObject psobject = PSObject.AsPSObject(this);
			psobject.GetObjectData(info, context);
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000EB270 File Offset: 0x000E9470
		private PSSenderInfo(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				return;
			}
			string text = null;
			try
			{
				text = (info.GetValue("CliXml", typeof(string)) as string);
			}
			catch (Exception)
			{
				return;
			}
			if (text != null)
			{
				try
				{
					PSObject pso = PSObject.AsPSObject(PSSerializer.Deserialize(text));
					PSSenderInfo pssenderInfo = DeserializingTypeConverter.RehydratePSSenderInfo(pso);
					this.userPrinicpal = pssenderInfo.userPrinicpal;
					this.connectionString = pssenderInfo.connectionString;
					this.applicationArguments = pssenderInfo.applicationArguments;
					this.clientTimeZone = pssenderInfo.ClientTimeZone;
				}
				catch (Exception)
				{
					return;
				}
				return;
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000EB314 File Offset: 0x000E9514
		public PSSenderInfo(PSPrincipal userPrincipal, string httpUrl)
		{
			this.userPrinicpal = userPrincipal;
			this.connectionString = httpUrl;
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000EB32A File Offset: 0x000E952A
		public PSPrincipal UserInfo
		{
			get
			{
				return this.userPrinicpal;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002A95 RID: 10901 RVA: 0x000EB332 File Offset: 0x000E9532
		// (set) Token: 0x06002A96 RID: 10902 RVA: 0x000EB33A File Offset: 0x000E953A
		public TimeZone ClientTimeZone
		{
			get
			{
				return this.clientTimeZone;
			}
			internal set
			{
				this.clientTimeZone = value;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002A97 RID: 10903 RVA: 0x000EB343 File Offset: 0x000E9543
		public string ConnectionString
		{
			get
			{
				return this.connectionString;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x000EB34B File Offset: 0x000E954B
		// (set) Token: 0x06002A99 RID: 10905 RVA: 0x000EB353 File Offset: 0x000E9553
		public PSPrimitiveDictionary ApplicationArguments
		{
			get
			{
				return this.applicationArguments;
			}
			internal set
			{
				this.applicationArguments = value;
			}
		}

		// Token: 0x0400150A RID: 5386
		private PSPrincipal userPrinicpal;

		// Token: 0x0400150B RID: 5387
		private string connectionString;

		// Token: 0x0400150C RID: 5388
		private PSPrimitiveDictionary applicationArguments;

		// Token: 0x0400150D RID: 5389
		private TimeZone clientTimeZone;
	}
}
