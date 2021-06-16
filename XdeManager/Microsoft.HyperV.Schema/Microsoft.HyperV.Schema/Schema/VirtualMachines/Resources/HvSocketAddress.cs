using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000029 RID: 41
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocketAddress
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00003EA6 File Offset: 0x000020A6
		public static bool IsJsonDefault(HvSocketAddress val)
		{
			return HvSocketAddress._default.JsonEquals(val);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003EB4 File Offset: 0x000020B4
		public bool JsonEquals(object obj)
		{
			HvSocketAddress graph = obj as HvSocketAddress;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HvSocketAddress), new DataContractJsonSerializerSettings
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

		// Token: 0x040000BF RID: 191
		private static readonly HvSocketAddress _default = new HvSocketAddress();

		// Token: 0x040000C0 RID: 192
		[DataMember(IsRequired = true)]
		public Guid LocalAddress;

		// Token: 0x040000C1 RID: 193
		[DataMember(IsRequired = true)]
		public Guid ParentAddress;
	}
}
