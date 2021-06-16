using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FF RID: 255
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class AllocationParameters
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x0000D66C File Offset: 0x0000B86C
		public static bool IsJsonDefault(AllocationParameters val)
		{
			return AllocationParameters._default.JsonEquals(val);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000D67C File Offset: 0x0000B87C
		public bool JsonEquals(object obj)
		{
			AllocationParameters graph = obj as AllocationParameters;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(AllocationParameters), new DataContractJsonSerializerSettings
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0000D724 File Offset: 0x0000B924
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0000D72C File Offset: 0x0000B92C
		[DataMember(Name = "Settings")]
		private MemoryParameters _Settings
		{
			get
			{
				return this.Settings;
			}
			set
			{
				if (value != null)
				{
					this.Settings = value;
				}
			}
		}

		// Token: 0x040004FE RID: 1278
		private static readonly AllocationParameters _default = new AllocationParameters();

		// Token: 0x040004FF RID: 1279
		[DataMember]
		public long InitialPages;

		// Token: 0x04000500 RID: 1280
		[DataMember]
		public long BalloonedPages;

		// Token: 0x04000501 RID: 1281
		[DataMember]
		public sbyte VirtualNodeCount;

		// Token: 0x04000502 RID: 1282
		[DataMember]
		public long[] InitialPagesByVNode;

		// Token: 0x04000503 RID: 1283
		[DataMember]
		public int VirtualProcessorCount;

		// Token: 0x04000504 RID: 1284
		[DataMember]
		public long[] BalloonedPagesByVNode;

		// Token: 0x04000505 RID: 1285
		[DataMember]
		public long[] WorkingSetLowPageByVNode;

		// Token: 0x04000506 RID: 1286
		[DataMember]
		public long[] WorkingSetHighPageByVNode;

		// Token: 0x04000507 RID: 1287
		public MemoryParameters Settings = new MemoryParameters();
	}
}
