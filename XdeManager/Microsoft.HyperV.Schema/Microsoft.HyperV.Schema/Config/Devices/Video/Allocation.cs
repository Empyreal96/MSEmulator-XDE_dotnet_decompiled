using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Video
{
	// Token: 0x02000114 RID: 276
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Allocation
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0000E89E File Offset: 0x0000CA9E
		public static bool IsJsonDefault(Allocation val)
		{
			return Allocation._default.JsonEquals(val);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000E8AC File Offset: 0x0000CAAC
		public bool JsonEquals(object obj)
		{
			Allocation graph = obj as Allocation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Allocation), new DataContractJsonSerializerSettings
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

		// Token: 0x04000579 RID: 1401
		private static readonly Allocation _default = new Allocation();

		// Token: 0x0400057A RID: 1402
		[DataMember(EmitDefaultValue = false)]
		public string AllocatedGPU;

		// Token: 0x0400057B RID: 1403
		[DataMember(EmitDefaultValue = false)]
		public string AllocatedGPUInstanceId;
	}
}
