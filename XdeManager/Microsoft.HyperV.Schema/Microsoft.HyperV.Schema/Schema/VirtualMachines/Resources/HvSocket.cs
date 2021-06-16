using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.HvSocket;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200002A RID: 42
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocket
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00003F70 File Offset: 0x00002170
		public static bool IsJsonDefault(HvSocket val)
		{
			return HvSocket._default.JsonEquals(val);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003F80 File Offset: 0x00002180
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004028 File Offset: 0x00002228
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x0000403F File Offset: 0x0000223F
		[DataMember(EmitDefaultValue = false, Name = "HvSocketConfig")]
		private HvSocketSystemConfig _HvSocketConfig
		{
			get
			{
				if (!HvSocketSystemConfig.IsJsonDefault(this.HvSocketConfig))
				{
					return this.HvSocketConfig;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.HvSocketConfig = value;
				}
			}
		}

		// Token: 0x040000C2 RID: 194
		private static readonly HvSocket _default = new HvSocket();

		// Token: 0x040000C3 RID: 195
		public HvSocketSystemConfig HvSocketConfig = new HvSocketSystemConfig();
	}
}
