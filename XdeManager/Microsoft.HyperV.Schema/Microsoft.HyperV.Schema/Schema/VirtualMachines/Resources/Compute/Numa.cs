using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x0200004B RID: 75
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Numa
	{
		// Token: 0x06000127 RID: 295 RVA: 0x000054E4 File Offset: 0x000036E4
		public static bool IsJsonDefault(Numa val)
		{
			return Numa._default.JsonEquals(val);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000054F4 File Offset: 0x000036F4
		public bool JsonEquals(object obj)
		{
			Numa graph = obj as Numa;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Numa), new DataContractJsonSerializerSettings
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

		// Token: 0x04000184 RID: 388
		private static readonly Numa _default = new Numa();

		// Token: 0x04000185 RID: 389
		[DataMember(EmitDefaultValue = false)]
		public byte VirtualNodeCount;

		// Token: 0x04000186 RID: 390
		[DataMember(EmitDefaultValue = false)]
		public byte[] PreferredPhysicalNodes;

		// Token: 0x04000187 RID: 391
		[DataMember(EmitDefaultValue = false)]
		public NumaSetting[] Settings;
	}
}
