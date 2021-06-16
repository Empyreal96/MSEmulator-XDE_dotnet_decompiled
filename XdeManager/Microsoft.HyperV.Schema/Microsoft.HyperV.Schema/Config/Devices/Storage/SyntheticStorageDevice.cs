using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012C RID: 300
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SyntheticStorageDevice
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0000F72C File Offset: 0x0000D92C
		public static bool IsJsonDefault(SyntheticStorageDevice val)
		{
			return SyntheticStorageDevice._default.JsonEquals(val);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000F73C File Offset: 0x0000D93C
		public bool JsonEquals(object obj)
		{
			SyntheticStorageDevice graph = obj as SyntheticStorageDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SyntheticStorageDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0000F7E4 File Offset: 0x0000D9E4
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		[DataMember(Name = "controller0")]
		private ScsiController _Controller
		{
			get
			{
				return this.Controller;
			}
			set
			{
				if (value != null)
				{
					this.Controller = value;
				}
			}
		}

		// Token: 0x04000627 RID: 1575
		private static readonly SyntheticStorageDevice _default = new SyntheticStorageDevice();

		// Token: 0x04000628 RID: 1576
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000629 RID: 1577
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400062A RID: 1578
		[DataMember(EmitDefaultValue = false)]
		public string ElementName;

		// Token: 0x0400062B RID: 1579
		[DataMember]
		public Guid ChannelInstanceGuid;

		// Token: 0x0400062C RID: 1580
		[DataMember(EmitDefaultValue = false)]
		public bool DisableInterruptBatching;

		// Token: 0x0400062D RID: 1581
		[DataMember(EmitDefaultValue = false)]
		public uint VPCPerChannel;

		// Token: 0x0400062E RID: 1582
		[DataMember(EmitDefaultValue = false)]
		public uint ThreadsPerChannel;

		// Token: 0x0400062F RID: 1583
		public ScsiController Controller = new ScsiController();
	}
}
