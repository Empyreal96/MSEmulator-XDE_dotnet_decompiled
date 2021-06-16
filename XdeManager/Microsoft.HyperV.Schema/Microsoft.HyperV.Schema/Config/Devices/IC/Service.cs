using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x0200014E RID: 334
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Service
	{
		// Token: 0x0600053F RID: 1343 RVA: 0x00011198 File Offset: 0x0000F398
		public static bool IsJsonDefault(Service val)
		{
			return Service._default.JsonEquals(val);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000111A8 File Offset: 0x0000F3A8
		public bool JsonEquals(object obj)
		{
			Service graph = obj as Service;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Service), new DataContractJsonSerializerSettings
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00011250 File Offset: 0x0000F450
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x00011260 File Offset: 0x0000F460
		[DataMember(EmitDefaultValue = false, Name = "EnabledStatePolicy")]
		private ulong _EnabledStatePolicy
		{
			get
			{
				EnabledState enabledStatePolicy = this.EnabledStatePolicy;
				return (ulong)((long)this.EnabledStatePolicy);
			}
			set
			{
				this.EnabledStatePolicy = (EnabledState)value;
			}
		}

		// Token: 0x040006E2 RID: 1762
		private static readonly Service _default = new Service();

		// Token: 0x040006E3 RID: 1763
		[DataMember(Name = "id")]
		public Guid Id;

		// Token: 0x040006E4 RID: 1764
		public EnabledState EnabledStatePolicy;
	}
}
