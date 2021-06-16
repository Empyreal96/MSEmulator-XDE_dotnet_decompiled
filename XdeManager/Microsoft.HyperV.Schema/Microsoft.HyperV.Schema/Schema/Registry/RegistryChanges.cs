using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Registry
{
	// Token: 0x0200007D RID: 125
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RegistryChanges
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00007A96 File Offset: 0x00005C96
		public static bool IsJsonDefault(RegistryChanges val)
		{
			return RegistryChanges._default.JsonEquals(val);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007AA4 File Offset: 0x00005CA4
		public bool JsonEquals(object obj)
		{
			RegistryChanges graph = obj as RegistryChanges;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RegistryChanges), new DataContractJsonSerializerSettings
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

		// Token: 0x040002B5 RID: 693
		private static readonly RegistryChanges _default = new RegistryChanges();

		// Token: 0x040002B6 RID: 694
		[DataMember(EmitDefaultValue = false)]
		public RegistryValue[] AddValues;

		// Token: 0x040002B7 RID: 695
		[DataMember(EmitDefaultValue = false)]
		public RegistryKey[] DeleteKeys;
	}
}
