using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000022 RID: 34
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class EnhancedModeVideo
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003854 File Offset: 0x00001A54
		public static bool IsJsonDefault(EnhancedModeVideo val)
		{
			return EnhancedModeVideo._default.JsonEquals(val);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003864 File Offset: 0x00001A64
		public bool JsonEquals(object obj)
		{
			EnhancedModeVideo graph = obj as EnhancedModeVideo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(EnhancedModeVideo), new DataContractJsonSerializerSettings
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000390C File Offset: 0x00001B0C
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003923 File Offset: 0x00001B23
		[DataMember(EmitDefaultValue = false, Name = "ConnectionOptions")]
		private RdpConnectionOptions _ConnectionOptions
		{
			get
			{
				if (!RdpConnectionOptions.IsJsonDefault(this.ConnectionOptions))
				{
					return this.ConnectionOptions;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.ConnectionOptions = value;
				}
			}
		}

		// Token: 0x040000A2 RID: 162
		private static readonly EnhancedModeVideo _default = new EnhancedModeVideo();

		// Token: 0x040000A3 RID: 163
		public RdpConnectionOptions ConnectionOptions = new RdpConnectionOptions();
	}
}
