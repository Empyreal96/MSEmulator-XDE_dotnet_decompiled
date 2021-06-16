using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Video
{
	// Token: 0x02000115 RID: 277
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Synthetic3dVideo
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x0000E968 File Offset: 0x0000CB68
		public static bool IsJsonDefault(Synthetic3dVideo val)
		{
			return Synthetic3dVideo._default.JsonEquals(val);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000E978 File Offset: 0x0000CB78
		public bool JsonEquals(object obj)
		{
			Synthetic3dVideo graph = obj as Synthetic3dVideo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Synthetic3dVideo), new DataContractJsonSerializerSettings
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000EA20 File Offset: 0x0000CC20
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0000EA37 File Offset: 0x0000CC37
		[DataMember(EmitDefaultValue = false, Name = "allocations")]
		private Allocation _Allocations
		{
			get
			{
				if (!Allocation.IsJsonDefault(this.Allocations))
				{
					return this.Allocations;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Allocations = value;
				}
			}
		}

		// Token: 0x0400057C RID: 1404
		private static readonly Synthetic3dVideo _default = new Synthetic3dVideo();

		// Token: 0x0400057D RID: 1405
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400057E RID: 1406
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400057F RID: 1407
		[DataMember]
		public byte MaximumMonitors;

		// Token: 0x04000580 RID: 1408
		[DataMember(Name = "MaximumScreenResolution")]
		public byte MaximumScreenResolutionIndex;

		// Token: 0x04000581 RID: 1409
		[DataMember(EmitDefaultValue = false)]
		public ulong VRAMSizeBytes;

		// Token: 0x04000582 RID: 1410
		[DataMember]
		public Guid ChannelInstanceGuid;

		// Token: 0x04000583 RID: 1411
		public Allocation Allocations = new Allocation();

		// Token: 0x04000584 RID: 1412
		[DataMember(EmitDefaultValue = false)]
		public bool SharedMemoryMode;
	}
}
