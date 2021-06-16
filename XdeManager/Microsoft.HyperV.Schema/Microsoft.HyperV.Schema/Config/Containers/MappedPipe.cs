using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000169 RID: 361
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedPipe
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x00012107 File Offset: 0x00010307
		public static bool IsJsonDefault(MappedPipe val)
		{
			return MappedPipe._default.JsonEquals(val);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00012114 File Offset: 0x00010314
		public bool JsonEquals(object obj)
		{
			MappedPipe graph = obj as MappedPipe;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedPipe), new DataContractJsonSerializerSettings
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

		// Token: 0x04000781 RID: 1921
		private static readonly MappedPipe _default = new MappedPipe();

		// Token: 0x04000782 RID: 1922
		[DataMember(IsRequired = true)]
		public string ContainerPipeName;

		// Token: 0x04000783 RID: 1923
		[DataMember(IsRequired = true)]
		public string HostPath;

		// Token: 0x04000784 RID: 1924
		[DataMember(EmitDefaultValue = false)]
		public bool CreateInUtilityVM;
	}
}
