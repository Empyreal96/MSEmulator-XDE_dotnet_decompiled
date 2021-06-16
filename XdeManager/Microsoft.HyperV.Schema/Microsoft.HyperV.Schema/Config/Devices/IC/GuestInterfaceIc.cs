using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x02000151 RID: 337
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestInterfaceIc
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x0001141B File Offset: 0x0000F61B
		public static bool IsJsonDefault(GuestInterfaceIc val)
		{
			return GuestInterfaceIc._default.JsonEquals(val);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00011428 File Offset: 0x0000F628
		public bool JsonEquals(object obj)
		{
			GuestInterfaceIc graph = obj as GuestInterfaceIc;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestInterfaceIc), new DataContractJsonSerializerSettings
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

		// Token: 0x040006ED RID: 1773
		private static readonly GuestInterfaceIc _default = new GuestInterfaceIc();

		// Token: 0x040006EE RID: 1774
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040006EF RID: 1775
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040006F0 RID: 1776
		[DataMember]
		public bool Enabled = true;

		// Token: 0x040006F1 RID: 1777
		[DataMember(EmitDefaultValue = false)]
		public bool ParentDisabled;

		// Token: 0x040006F2 RID: 1778
		[DataMember(EmitDefaultValue = false)]
		public Service[] Services;

		// Token: 0x040006F3 RID: 1779
		[DataMember(EmitDefaultValue = false)]
		public bool DefaultEnabledStatePolicy = true;

		// Token: 0x040006F4 RID: 1780
		[DataMember(EmitDefaultValue = false)]
		public HvSocketSystemWpConfig SystemConfig;
	}
}
