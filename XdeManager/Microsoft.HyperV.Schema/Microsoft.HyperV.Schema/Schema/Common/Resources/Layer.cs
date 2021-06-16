using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Common.Resources
{
	// Token: 0x0200000B RID: 11
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Layer
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000027A8 File Offset: 0x000009A8
		public static bool IsJsonDefault(Layer val)
		{
			return Layer._default.JsonEquals(val);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027B8 File Offset: 0x000009B8
		public bool JsonEquals(object obj)
		{
			Layer graph = obj as Layer;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Layer), new DataContractJsonSerializerSettings
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002860 File Offset: 0x00000A60
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000287A File Offset: 0x00000A7A
		[DataMember(Name = "PathType")]
		private string _PathType
		{
			get
			{
				PathType pathType = this.PathType;
				return this.PathType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.PathType = PathType.AbsolutePath;
				}
				this.PathType = (PathType)Enum.Parse(typeof(PathType), value, true);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000028A8 File Offset: 0x00000AA8
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000028D2 File Offset: 0x00000AD2
		[DataMember(EmitDefaultValue = false, Name = "Cache")]
		private string _Cache
		{
			get
			{
				if (this.Cache == CacheMode.Unspecified)
				{
					return null;
				}
				return this.Cache.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Cache = CacheMode.Unspecified;
				}
				this.Cache = (CacheMode)Enum.Parse(typeof(CacheMode), value, true);
			}
		}

		// Token: 0x04000036 RID: 54
		private static readonly Layer _default = new Layer();

		// Token: 0x04000037 RID: 55
		[DataMember]
		public Guid Id;

		// Token: 0x04000038 RID: 56
		[DataMember]
		public string Path;

		// Token: 0x04000039 RID: 57
		public PathType PathType;

		// Token: 0x0400003A RID: 58
		public CacheMode Cache;
	}
}
