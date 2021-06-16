using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema;
using HCS.Schema.Responses.System;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AC RID: 428
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GcsCapabilities
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x00015CC4 File Offset: 0x00013EC4
		public static bool IsJsonDefault(GcsCapabilities val)
		{
			return GcsCapabilities._default.JsonEquals(val);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00015CD4 File Offset: 0x00013ED4
		public bool JsonEquals(object obj)
		{
			GcsCapabilities graph = obj as GcsCapabilities;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GcsCapabilities), new DataContractJsonSerializerSettings
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00015D7C File Offset: 0x00013F7C
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00015DA6 File Offset: 0x00013FA6
		[DataMember(EmitDefaultValue = false, Name = "RuntimeOsType")]
		private string _RuntimeOsType
		{
			get
			{
				if (this.RuntimeOsType == OsType.Unknown)
				{
					return null;
				}
				return this.RuntimeOsType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RuntimeOsType = OsType.Unknown;
				}
				this.RuntimeOsType = (OsType)Enum.Parse(typeof(OsType), value, true);
			}
		}

		// Token: 0x04000982 RID: 2434
		private static readonly GcsCapabilities _default = new GcsCapabilities();

		// Token: 0x04000983 RID: 2435
		[DataMember(EmitDefaultValue = false)]
		public bool SendHostCreateMessage;

		// Token: 0x04000984 RID: 2436
		[DataMember(EmitDefaultValue = false)]
		public bool SendHostStartMessage;

		// Token: 0x04000985 RID: 2437
		[DataMember(EmitDefaultValue = false)]
		public bool HvSocketConfigOnStartup;

		// Token: 0x04000986 RID: 2438
		[DataMember(EmitDefaultValue = false)]
		public bool SendLifecycleNotifications;

		// Token: 0x04000987 RID: 2439
		[DataMember(EmitDefaultValue = false)]
		public HCS.Schema.Version[] SupportedSchemaVersions;

		// Token: 0x04000988 RID: 2440
		public OsType RuntimeOsType;

		// Token: 0x04000989 RID: 2441
		[DataMember(EmitDefaultValue = false)]
		public object GuestDefinedCapabilities;
	}
}
