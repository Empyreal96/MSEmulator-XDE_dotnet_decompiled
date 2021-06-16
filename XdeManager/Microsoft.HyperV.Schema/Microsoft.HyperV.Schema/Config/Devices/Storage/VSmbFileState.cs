using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000125 RID: 293
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbFileState
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0000F0FC File Offset: 0x0000D2FC
		public static bool IsJsonDefault(VSmbFileState val)
		{
			return VSmbFileState._default.JsonEquals(val);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000F10C File Offset: 0x0000D30C
		public bool JsonEquals(object obj)
		{
			VSmbFileState graph = obj as VSmbFileState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbFileState), new DataContractJsonSerializerSettings
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

		// Token: 0x040005DC RID: 1500
		private static readonly VSmbFileState _default = new VSmbFileState();

		// Token: 0x040005DD RID: 1501
		[DataMember]
		public ulong VolatileFileId;

		// Token: 0x040005DE RID: 1502
		[DataMember]
		public ulong PersistentFileId;

		// Token: 0x040005DF RID: 1503
		[DataMember]
		public uint TreeId;

		// Token: 0x040005E0 RID: 1504
		[DataMember]
		public uint ShareAccess;

		// Token: 0x040005E1 RID: 1505
		[DataMember]
		public uint CreateOptions;

		// Token: 0x040005E2 RID: 1506
		[DataMember]
		public uint CreateDisposition;

		// Token: 0x040005E3 RID: 1507
		[DataMember]
		public uint DesiredAccess;

		// Token: 0x040005E4 RID: 1508
		[DataMember]
		public uint FileAttributes;

		// Token: 0x040005E5 RID: 1509
		[DataMember]
		public string RelativeName;

		// Token: 0x040005E6 RID: 1510
		[DataMember]
		public byte[] EaBuffer;

		// Token: 0x040005E7 RID: 1511
		[DataMember]
		public uint GrantedAccess;

		// Token: 0x040005E8 RID: 1512
		[DataMember]
		public ulong OnDiskFileId;

		// Token: 0x040005E9 RID: 1513
		[DataMember]
		public uint CreateAction;
	}
}
