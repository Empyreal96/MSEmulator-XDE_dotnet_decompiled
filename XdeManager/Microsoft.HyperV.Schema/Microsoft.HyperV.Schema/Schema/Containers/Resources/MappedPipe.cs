using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x0200009B RID: 155
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedPipe
	{
		// Token: 0x0600025B RID: 603 RVA: 0x00008E37 File Offset: 0x00007037
		public static bool IsJsonDefault(MappedPipe val)
		{
			return MappedPipe._default.JsonEquals(val);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00008E44 File Offset: 0x00007044
		public bool JsonEquals(object obj)
		{
			MappedPipe graph = obj as MappedPipe;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedPipe), new DataContractJsonSerializerSettings
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00008EEC File Offset: 0x000070EC
		// (set) Token: 0x0600025E RID: 606 RVA: 0x00008F06 File Offset: 0x00007106
		[DataMember(Name = "HostPathType")]
		private string _HostPathType
		{
			get
			{
				MappedPipePathType hostPathType = this.HostPathType;
				return this.HostPathType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.HostPathType = MappedPipePathType.AbsolutePath;
				}
				this.HostPathType = (MappedPipePathType)Enum.Parse(typeof(MappedPipePathType), value, true);
			}
		}

		// Token: 0x04000333 RID: 819
		private static readonly MappedPipe _default = new MappedPipe();

		// Token: 0x04000334 RID: 820
		[DataMember]
		public string ContainerPipeName;

		// Token: 0x04000335 RID: 821
		[DataMember]
		public string HostPath;

		// Token: 0x04000336 RID: 822
		public MappedPipePathType HostPathType;
	}
}
