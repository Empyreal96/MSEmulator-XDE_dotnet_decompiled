using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000098 RID: 152
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CombinedLayers
	{
		// Token: 0x0600024D RID: 589 RVA: 0x00008B8C File Offset: 0x00006D8C
		public static bool IsJsonDefault(CombinedLayers val)
		{
			return CombinedLayers._default.JsonEquals(val);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008B9C File Offset: 0x00006D9C
		public bool JsonEquals(object obj)
		{
			CombinedLayers graph = obj as CombinedLayers;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CombinedLayers), new DataContractJsonSerializerSettings
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

		// Token: 0x04000326 RID: 806
		private static readonly CombinedLayers _default = new CombinedLayers();

		// Token: 0x04000327 RID: 807
		[DataMember(EmitDefaultValue = false)]
		public Layer[] Layers;

		// Token: 0x04000328 RID: 808
		[DataMember(EmitDefaultValue = false)]
		public string ScratchPath;

		// Token: 0x04000329 RID: 809
		[DataMember]
		public string ContainerRootPath;
	}
}
