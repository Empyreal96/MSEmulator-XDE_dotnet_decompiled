using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000127 RID: 295
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbSessionState
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x0000F294 File Offset: 0x0000D494
		public static bool IsJsonDefault(VSmbSessionState val)
		{
			return VSmbSessionState._default.JsonEquals(val);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000F2A4 File Offset: 0x0000D4A4
		public bool JsonEquals(object obj)
		{
			VSmbSessionState graph = obj as VSmbSessionState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbSessionState), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x040005EF RID: 1519
		private static readonly VSmbSessionState _default = new VSmbSessionState();

		// Token: 0x040005F0 RID: 1520
		[DataMember]
		public ulong SessionId;

		// Token: 0x040005F1 RID: 1521
		[DataMember]
		public bool IsAdmin;

		// Token: 0x040005F2 RID: 1522
		[DataMember]
		public bool IsAnonymous;

		// Token: 0x040005F3 RID: 1523
		[DataMember]
		public bool IsGuest;

		// Token: 0x040005F4 RID: 1524
		[DataMember]
		public bool IsSessionSetupInProgress;

		// Token: 0x040005F5 RID: 1525
		[DataMember]
		public bool IsLogonSequenceInProgress;

		// Token: 0x040005F6 RID: 1526
		[DataMember]
		public bool IsExpired;

		// Token: 0x040005F7 RID: 1527
		[DataMember]
		public bool SigningRequired;

		// Token: 0x040005F8 RID: 1528
		[DataMember]
		public bool IsMasterSession;

		// Token: 0x040005F9 RID: 1529
		[DataMember]
		public uint Capabilities;

		// Token: 0x040005FA RID: 1530
		[DataMember]
		public ushort Dialect;

		// Token: 0x040005FB RID: 1531
		[DataMember]
		public VSmbTreeConnectState[] TreeConnects;

		// Token: 0x040005FC RID: 1532
		[DataMember]
		public VSmbFileState[] Files;
	}
}
