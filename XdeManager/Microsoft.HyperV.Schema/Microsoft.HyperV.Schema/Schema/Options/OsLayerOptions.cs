using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x0200008D RID: 141
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class OsLayerOptions
	{
		// Token: 0x06000223 RID: 547 RVA: 0x00008374 File Offset: 0x00006574
		public static bool IsJsonDefault(OsLayerOptions val)
		{
			return OsLayerOptions._default.JsonEquals(val);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008384 File Offset: 0x00006584
		public bool JsonEquals(object obj)
		{
			OsLayerOptions graph = obj as OsLayerOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(OsLayerOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000842C File Offset: 0x0000662C
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00008456 File Offset: 0x00006656
		[DataMember(EmitDefaultValue = false, Name = "Type")]
		private string _Type
		{
			get
			{
				if (this.Type == OsLayerType.Container)
				{
					return null;
				}
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = OsLayerType.Container;
				}
				this.Type = (OsLayerType)Enum.Parse(typeof(OsLayerType), value, true);
			}
		}

		// Token: 0x040002F9 RID: 761
		private static readonly OsLayerOptions _default = new OsLayerOptions();

		// Token: 0x040002FA RID: 762
		public OsLayerType Type;

		// Token: 0x040002FB RID: 763
		[DataMember(EmitDefaultValue = false)]
		public bool DisableCiCacheOptimization;

		// Token: 0x040002FC RID: 764
		[DataMember(EmitDefaultValue = false)]
		public bool IsDynamic;

		// Token: 0x040002FD RID: 765
		[DataMember(EmitDefaultValue = false)]
		public bool SkipSandboxPreExpansion;
	}
}
