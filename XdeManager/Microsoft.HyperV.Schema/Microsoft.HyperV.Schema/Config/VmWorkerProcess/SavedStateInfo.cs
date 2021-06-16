using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x02000102 RID: 258
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SavedStateInfo
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0000D908 File Offset: 0x0000BB08
		public static bool IsJsonDefault(SavedStateInfo val)
		{
			return SavedStateInfo._default.JsonEquals(val);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000D918 File Offset: 0x0000BB18
		public bool JsonEquals(object obj)
		{
			SavedStateInfo graph = obj as SavedStateInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SavedStateInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000D9C0 File Offset: 0x0000BBC0
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000D9EA File Offset: 0x0000BBEA
		[DataMember(EmitDefaultValue = false, Name = "type")]
		private string _Type
		{
			get
			{
				if (this.Type == SavedStateType.None)
				{
					return null;
				}
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = SavedStateType.None;
				}
				this.Type = (SavedStateType)Enum.Parse(typeof(SavedStateType), value, true);
			}
		}

		// Token: 0x0400050F RID: 1295
		private static readonly SavedStateInfo _default = new SavedStateInfo();

		// Token: 0x04000510 RID: 1296
		public SavedStateType Type;

		// Token: 0x04000511 RID: 1297
		[DataMember(EmitDefaultValue = false, Name = "vsvlocation")]
		public string FilePath;
	}
}
