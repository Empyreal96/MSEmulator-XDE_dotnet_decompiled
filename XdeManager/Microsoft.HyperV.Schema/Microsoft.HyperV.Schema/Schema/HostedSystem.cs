using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostedSystem
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000025D7 File Offset: 0x000007D7
		public static bool IsJsonDefault(HostedSystem val)
		{
			return HostedSystem._default.JsonEquals(val);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025E4 File Offset: 0x000007E4
		public bool JsonEquals(object obj)
		{
			HostedSystem graph = obj as HostedSystem;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostedSystem), new DataContractJsonSerializerSettings
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000268C File Offset: 0x0000088C
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002694 File Offset: 0x00000894
		[DataMember(Name = "SchemaVersion")]
		private Version _SchemaVersion
		{
			get
			{
				return this.SchemaVersion;
			}
			set
			{
				if (value != null)
				{
					this.SchemaVersion = value;
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000026A0 File Offset: 0x000008A0
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000026A8 File Offset: 0x000008A8
		[DataMember(Name = "Container")]
		private Container _Container
		{
			get
			{
				return this.Container;
			}
			set
			{
				if (value != null)
				{
					this.Container = value;
				}
			}
		}

		// Token: 0x04000027 RID: 39
		private static readonly HostedSystem _default = new HostedSystem();

		// Token: 0x04000028 RID: 40
		public Version SchemaVersion = new Version();

		// Token: 0x04000029 RID: 41
		public Container Container = new Container();
	}
}
