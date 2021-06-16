using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x02000100 RID: 256
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ResourceAllocations
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x0000D757 File Offset: 0x0000B957
		public static bool IsJsonDefault(ResourceAllocations val)
		{
			return ResourceAllocations._default.JsonEquals(val);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000D764 File Offset: 0x0000B964
		public bool JsonEquals(object obj)
		{
			ResourceAllocations graph = obj as ResourceAllocations;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ResourceAllocations), new DataContractJsonSerializerSettings
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000D80C File Offset: 0x0000BA0C
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0000D814 File Offset: 0x0000BA14
		[DataMember(Name = "numa_mappings")]
		private NumaNodeMappings _NumaNodes
		{
			get
			{
				return this.NumaNodes;
			}
			set
			{
				if (value != null)
				{
					this.NumaNodes = value;
				}
			}
		}

		// Token: 0x04000508 RID: 1288
		private static readonly ResourceAllocations _default = new ResourceAllocations();

		// Token: 0x04000509 RID: 1289
		public NumaNodeMappings NumaNodes = new NumaNodeMappings();
	}
}
