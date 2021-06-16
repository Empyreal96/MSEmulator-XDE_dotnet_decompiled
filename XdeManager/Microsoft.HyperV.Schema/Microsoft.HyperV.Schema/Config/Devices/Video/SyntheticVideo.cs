using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Video
{
	// Token: 0x02000113 RID: 275
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SyntheticVideo
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		public static bool IsJsonDefault(SyntheticVideo val)
		{
			return SyntheticVideo._default.JsonEquals(val);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		public bool JsonEquals(object obj)
		{
			SyntheticVideo graph = obj as SyntheticVideo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SyntheticVideo), new DataContractJsonSerializerSettings
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000E870 File Offset: 0x0000CA70
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000E880 File Offset: 0x0000CA80
		[DataMember(EmitDefaultValue = false, Name = "ResolutionType")]
		private ulong _ResolutionType
		{
			get
			{
				ResolutionType resolutionType = this.ResolutionType;
				return (ulong)((long)this.ResolutionType);
			}
			set
			{
				this.ResolutionType = (ResolutionType)value;
			}
		}

		// Token: 0x04000572 RID: 1394
		private static readonly SyntheticVideo _default = new SyntheticVideo();

		// Token: 0x04000573 RID: 1395
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000574 RID: 1396
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000575 RID: 1397
		[DataMember(EmitDefaultValue = false)]
		public ushort HorizontalResolution;

		// Token: 0x04000576 RID: 1398
		[DataMember(EmitDefaultValue = false)]
		public ushort VerticalResolution;

		// Token: 0x04000577 RID: 1399
		public ResolutionType ResolutionType;

		// Token: 0x04000578 RID: 1400
		[DataMember(EmitDefaultValue = false)]
		public bool SecondaryDevice;
	}
}
