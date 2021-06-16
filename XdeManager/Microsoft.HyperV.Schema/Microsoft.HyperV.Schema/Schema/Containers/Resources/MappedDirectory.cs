using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x0200009A RID: 154
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedDirectory
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00008D24 File Offset: 0x00006F24
		public static bool IsJsonDefault(MappedDirectory val)
		{
			return MappedDirectory._default.JsonEquals(val);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00008D34 File Offset: 0x00006F34
		public bool JsonEquals(object obj)
		{
			MappedDirectory graph = obj as MappedDirectory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedDirectory), new DataContractJsonSerializerSettings
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00008DDC File Offset: 0x00006FDC
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00008DF6 File Offset: 0x00006FF6
		[DataMember(Name = "HostPathType")]
		private string _HostPathType
		{
			get
			{
				PathType hostPathType = this.HostPathType;
				return this.HostPathType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.HostPathType = PathType.AbsolutePath;
				}
				this.HostPathType = (PathType)Enum.Parse(typeof(PathType), value, true);
			}
		}

		// Token: 0x0400032E RID: 814
		private static readonly MappedDirectory _default = new MappedDirectory();

		// Token: 0x0400032F RID: 815
		[DataMember]
		public string HostPath;

		// Token: 0x04000330 RID: 816
		public PathType HostPathType;

		// Token: 0x04000331 RID: 817
		[DataMember]
		public string ContainerPath;

		// Token: 0x04000332 RID: 818
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;
	}
}
