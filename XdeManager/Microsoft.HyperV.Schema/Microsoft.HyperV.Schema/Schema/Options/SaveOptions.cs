using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x02000089 RID: 137
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SaveOptions
	{
		// Token: 0x0600020B RID: 523 RVA: 0x00007F1B File Offset: 0x0000611B
		public static bool IsJsonDefault(SaveOptions val)
		{
			return SaveOptions._default.JsonEquals(val);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00007F28 File Offset: 0x00006128
		public bool JsonEquals(object obj)
		{
			SaveOptions graph = obj as SaveOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SaveOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007FD0 File Offset: 0x000061D0
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00007FFA File Offset: 0x000061FA
		[DataMember(EmitDefaultValue = false, Name = "SaveType")]
		private string _SaveType
		{
			get
			{
				if (this.SaveType == SaveType.ToFile)
				{
					return null;
				}
				return this.SaveType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SaveType = SaveType.ToFile;
				}
				this.SaveType = (SaveType)Enum.Parse(typeof(SaveType), value, true);
			}
		}

		// Token: 0x040002EE RID: 750
		private static readonly SaveOptions _default = new SaveOptions();

		// Token: 0x040002EF RID: 751
		public SaveType SaveType;

		// Token: 0x040002F0 RID: 752
		[DataMember]
		public string RuntimeStateFilePath;

		// Token: 0x040002F1 RID: 753
		[DataMember]
		public string SaveStateFilePath;
	}
}
