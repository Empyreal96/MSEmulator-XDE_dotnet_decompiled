using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001F RID: 31
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VideoMonitor
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000035F0 File Offset: 0x000017F0
		public static bool IsJsonDefault(VideoMonitor val)
		{
			return VideoMonitor._default.JsonEquals(val);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003600 File Offset: 0x00001800
		public bool JsonEquals(object obj)
		{
			VideoMonitor graph = obj as VideoMonitor;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VideoMonitor), new DataContractJsonSerializerSettings
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

		// Token: 0x0400009C RID: 156
		private static readonly VideoMonitor _default = new VideoMonitor();

		// Token: 0x0400009D RID: 157
		[DataMember(EmitDefaultValue = false)]
		public ushort HorizontalResolution;

		// Token: 0x0400009E RID: 158
		[DataMember(EmitDefaultValue = false)]
		public ushort VerticalResolution;

		// Token: 0x0400009F RID: 159
		[DataMember(EmitDefaultValue = false)]
		public RdpConnectionOptions ConnectionOptions;
	}
}
