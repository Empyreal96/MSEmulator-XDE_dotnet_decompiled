using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000019 RID: 25
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class LinuxKernelDirect
	{
		// Token: 0x06000067 RID: 103 RVA: 0x0000312B File Offset: 0x0000132B
		public static bool IsJsonDefault(LinuxKernelDirect val)
		{
			return LinuxKernelDirect._default.JsonEquals(val);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003138 File Offset: 0x00001338
		public bool JsonEquals(object obj)
		{
			LinuxKernelDirect graph = obj as LinuxKernelDirect;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LinuxKernelDirect), new DataContractJsonSerializerSettings
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

		// Token: 0x04000085 RID: 133
		private static readonly LinuxKernelDirect _default = new LinuxKernelDirect();

		// Token: 0x04000086 RID: 134
		[DataMember(EmitDefaultValue = false)]
		public string KernelFilePath;

		// Token: 0x04000087 RID: 135
		[DataMember(EmitDefaultValue = false)]
		public string InitRdPath;

		// Token: 0x04000088 RID: 136
		[DataMember(EmitDefaultValue = false)]
		public string KernelCmdLine;
	}
}
