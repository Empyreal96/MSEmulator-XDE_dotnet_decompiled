using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000024 RID: 36
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestCrashReporting
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00003A6F File Offset: 0x00001C6F
		public static bool IsJsonDefault(GuestCrashReporting val)
		{
			return GuestCrashReporting._default.JsonEquals(val);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003A7C File Offset: 0x00001C7C
		public bool JsonEquals(object obj)
		{
			GuestCrashReporting graph = obj as GuestCrashReporting;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestCrashReporting), new DataContractJsonSerializerSettings
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

		// Token: 0x040000A8 RID: 168
		private static readonly GuestCrashReporting _default = new GuestCrashReporting();

		// Token: 0x040000A9 RID: 169
		[DataMember(EmitDefaultValue = false)]
		public WindowsCrashReporting WindowsCrashSettings;
	}
}
