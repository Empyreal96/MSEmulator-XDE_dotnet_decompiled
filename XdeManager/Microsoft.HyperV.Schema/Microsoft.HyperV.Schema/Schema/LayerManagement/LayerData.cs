using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Schema.LayerManagement
{
	// Token: 0x0200004D RID: 77
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class LayerData
	{
		// Token: 0x06000133 RID: 307 RVA: 0x000056BA File Offset: 0x000038BA
		public static bool IsJsonDefault(LayerData val)
		{
			return LayerData._default.JsonEquals(val);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000056C8 File Offset: 0x000038C8
		public bool JsonEquals(object obj)
		{
			LayerData graph = obj as LayerData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LayerData), new DataContractJsonSerializerSettings
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005770 File Offset: 0x00003970
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00005787 File Offset: 0x00003987
		[DataMember(EmitDefaultValue = false, Name = "SchemaVersion")]
		private Version _SchemaVersion
		{
			get
			{
				if (!Version.IsJsonDefault(this.SchemaVersion))
				{
					return this.SchemaVersion;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.SchemaVersion = value;
				}
			}
		}

		// Token: 0x0400018C RID: 396
		private static readonly LayerData _default = new LayerData();

		// Token: 0x0400018D RID: 397
		public Version SchemaVersion = new Version();

		// Token: 0x0400018E RID: 398
		[DataMember(EmitDefaultValue = false)]
		public Layer[] Layers;
	}
}
