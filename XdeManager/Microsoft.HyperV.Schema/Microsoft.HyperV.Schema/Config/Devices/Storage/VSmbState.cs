using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000129 RID: 297
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbState
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x0000F42C File Offset: 0x0000D62C
		public static bool IsJsonDefault(VSmbState val)
		{
			return VSmbState._default.JsonEquals(val);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000F43C File Offset: 0x0000D63C
		public bool JsonEquals(object obj)
		{
			VSmbState graph = obj as VSmbState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbState), new DataContractJsonSerializerSettings
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000F4E4 File Offset: 0x0000D6E4
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000F4EC File Offset: 0x0000D6EC
		[DataMember(Name = "Connection")]
		private VSmbConnectionState _Connection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				if (value != null)
				{
					this.Connection = value;
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0000F500 File Offset: 0x0000D700
		[DataMember(Name = "Transport")]
		private VSmbTransportState _Transport
		{
			get
			{
				return this.Transport;
			}
			set
			{
				if (value != null)
				{
					this.Transport = value;
				}
			}
		}

		// Token: 0x04000605 RID: 1541
		private static readonly VSmbState _default = new VSmbState();

		// Token: 0x04000606 RID: 1542
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000607 RID: 1543
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000608 RID: 1544
		[DataMember]
		public Guid ServerGuid;

		// Token: 0x04000609 RID: 1545
		[DataMember]
		public ulong ServerStartTime;

		// Token: 0x0400060A RID: 1546
		[DataMember]
		public long NextPersistentFileId;

		// Token: 0x0400060B RID: 1547
		[DataMember]
		public VSmbRequest[] ReplayList;

		// Token: 0x0400060C RID: 1548
		public VSmbConnectionState Connection = new VSmbConnectionState();

		// Token: 0x0400060D RID: 1549
		[DataMember]
		public VSmbSessionState[] Sessions;

		// Token: 0x0400060E RID: 1550
		public VSmbTransportState Transport = new VSmbTransportState();

		// Token: 0x0400060F RID: 1551
		[DataMember]
		public VsmbDirectMapSectionState[] DirectMapSections;
	}
}
