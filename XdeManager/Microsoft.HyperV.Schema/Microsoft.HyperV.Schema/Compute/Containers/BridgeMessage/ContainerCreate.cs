using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B0 RID: 432
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCreate
	{
		// Token: 0x060006F7 RID: 1783 RVA: 0x00016048 File Offset: 0x00014248
		public static bool IsJsonDefault(ContainerCreate val)
		{
			return ContainerCreate._default.JsonEquals(val);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00016058 File Offset: 0x00014258
		public bool JsonEquals(object obj)
		{
			ContainerCreate graph = obj as ContainerCreate;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCreate), new DataContractJsonSerializerSettings
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00016100 File Offset: 0x00014300
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00016117 File Offset: 0x00014317
		[DataMember(EmitDefaultValue = false, Name = "SupportedVersions")]
		private ProtocolSupport _SupportedVersions
		{
			get
			{
				if (!ProtocolSupport.IsJsonDefault(this.SupportedVersions))
				{
					return this.SupportedVersions;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.SupportedVersions = value;
				}
			}
		}

		// Token: 0x04000995 RID: 2453
		private static readonly ContainerCreate _default = new ContainerCreate();

		// Token: 0x04000996 RID: 2454
		[DataMember]
		public string ContainerId;

		// Token: 0x04000997 RID: 2455
		[DataMember]
		public Guid ActivityId;

		// Token: 0x04000998 RID: 2456
		[DataMember]
		public string ContainerConfig;

		// Token: 0x04000999 RID: 2457
		public ProtocolSupport SupportedVersions = new ProtocolSupport();
	}
}
