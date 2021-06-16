using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000124 RID: 292
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbConnectionState
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x0000F032 File Offset: 0x0000D232
		public static bool IsJsonDefault(VSmbConnectionState val)
		{
			return VSmbConnectionState._default.JsonEquals(val);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000F040 File Offset: 0x0000D240
		public bool JsonEquals(object obj)
		{
			VSmbConnectionState graph = obj as VSmbConnectionState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbConnectionState), new DataContractJsonSerializerSettings
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

		// Token: 0x040005CB RID: 1483
		private static readonly VSmbConnectionState _default = new VSmbConnectionState();

		// Token: 0x040005CC RID: 1484
		[DataMember]
		public bool IsActive;

		// Token: 0x040005CD RID: 1485
		[DataMember]
		public ushort Dialect;

		// Token: 0x040005CE RID: 1486
		[DataMember]
		public bool Authenticated;

		// Token: 0x040005CF RID: 1487
		[DataMember]
		public bool LargeMTU;

		// Token: 0x040005D0 RID: 1488
		[DataMember]
		public bool Leasing;

		// Token: 0x040005D1 RID: 1489
		[DataMember]
		public uint MaxReadWriteSize;

		// Token: 0x040005D2 RID: 1490
		[DataMember]
		public Guid ClientGuid;

		// Token: 0x040005D3 RID: 1491
		[DataMember]
		public bool SigningRequired;

		// Token: 0x040005D4 RID: 1492
		[DataMember]
		public byte[] SequenceTableData;

		// Token: 0x040005D5 RID: 1493
		[DataMember]
		public uint MaxWindowSize;

		// Token: 0x040005D6 RID: 1494
		[DataMember]
		public uint AllocatedWindowSize;

		// Token: 0x040005D7 RID: 1495
		[DataMember]
		public uint ActualWindowSize;

		// Token: 0x040005D8 RID: 1496
		[DataMember]
		public uint StartIndex;

		// Token: 0x040005D9 RID: 1497
		[DataMember]
		public uint EndIndex;

		// Token: 0x040005DA RID: 1498
		[DataMember]
		public ulong StartMid;

		// Token: 0x040005DB RID: 1499
		[DataMember]
		public ulong EndMid;
	}
}
