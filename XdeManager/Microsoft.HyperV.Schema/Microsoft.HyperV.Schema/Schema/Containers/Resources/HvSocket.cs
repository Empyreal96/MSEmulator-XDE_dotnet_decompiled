using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.HvSocket;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000096 RID: 150
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocket
	{
		// Token: 0x06000245 RID: 581 RVA: 0x000089F4 File Offset: 0x00006BF4
		public static bool IsJsonDefault(HvSocket val)
		{
			return HvSocket._default.JsonEquals(val);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008A04 File Offset: 0x00006C04
		public bool JsonEquals(object obj)
		{
			HvSocket graph = obj as HvSocket;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HvSocket), new DataContractJsonSerializerSettings
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

		// Token: 0x0400031B RID: 795
		private static readonly HvSocket _default = new HvSocket();

		// Token: 0x0400031C RID: 796
		[DataMember(EmitDefaultValue = false)]
		public HvSocketSystemConfig Config;

		// Token: 0x0400031D RID: 797
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePowerShellDirect;

		// Token: 0x0400031E RID: 798
		[DataMember(EmitDefaultValue = false)]
		public bool EnableUtcRelay;

		// Token: 0x0400031F RID: 799
		[DataMember(EmitDefaultValue = false)]
		public bool EnableAuditing;
	}
}
