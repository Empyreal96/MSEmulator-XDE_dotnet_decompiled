using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Resource.Vpci;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x0200010E RID: 270
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VpciDevice
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		public static bool IsJsonDefault(VpciDevice val)
		{
			return VpciDevice._default.JsonEquals(val);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
		public bool JsonEquals(object obj)
		{
			VpciDevice graph = obj as VpciDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VpciDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		[DataMember(Name = "HostResources")]
		private HostResources _HostResources
		{
			get
			{
				return this.HostResources;
			}
			set
			{
				if (value != null)
				{
					this.HostResources = value;
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x0000E4CB File Offset: 0x0000C6CB
		[DataMember(EmitDefaultValue = false, Name = "Preallocation")]
		private Allocation _Preallocation
		{
			get
			{
				if (!Allocation.IsJsonDefault(this.Preallocation))
				{
					return this.Preallocation;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Preallocation = value;
				}
			}
		}

		// Token: 0x04000552 RID: 1362
		private static readonly VpciDevice _default = new VpciDevice();

		// Token: 0x04000553 RID: 1363
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000554 RID: 1364
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000555 RID: 1365
		[DataMember(EmitDefaultValue = false)]
		public Guid InstanceGuid;

		// Token: 0x04000556 RID: 1366
		[DataMember(EmitDefaultValue = false)]
		public string PoolId;

		// Token: 0x04000557 RID: 1367
		public HostResources HostResources = new HostResources();

		// Token: 0x04000558 RID: 1368
		public Allocation Preallocation = new Allocation();
	}
}
