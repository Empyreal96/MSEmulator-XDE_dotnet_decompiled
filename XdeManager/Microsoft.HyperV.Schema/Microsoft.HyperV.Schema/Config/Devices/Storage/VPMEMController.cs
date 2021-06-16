using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000134 RID: 308
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMController
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x0000FE4F File Offset: 0x0000E04F
		public static bool IsJsonDefault(VPMEMController val)
		{
			return VPMEMController._default.JsonEquals(val);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000FE5C File Offset: 0x0000E05C
		public bool JsonEquals(object obj)
		{
			VPMEMController graph = obj as VPMEMController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMController), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000FF04 File Offset: 0x0000E104
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0000FF2E File Offset: 0x0000E12E
		[DataMember(EmitDefaultValue = false, Name = "Backing")]
		private string _Backing
		{
			get
			{
				if (this.Backing == VPMEMBackingType.AutoDetect)
				{
					return null;
				}
				return this.Backing.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Backing = VPMEMBackingType.AutoDetect;
				}
				this.Backing = (VPMEMBackingType)Enum.Parse(typeof(VPMEMBackingType), value, true);
			}
		}

		// Token: 0x0400064A RID: 1610
		private static readonly VPMEMController _default = new VPMEMController();

		// Token: 0x0400064B RID: 1611
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400064C RID: 1612
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400064D RID: 1613
		[DataMember(Name = "device")]
		public Dictionary<uint, VPMEMDevice> Devices;

		// Token: 0x0400064E RID: 1614
		public VPMEMBackingType Backing;
	}
}
