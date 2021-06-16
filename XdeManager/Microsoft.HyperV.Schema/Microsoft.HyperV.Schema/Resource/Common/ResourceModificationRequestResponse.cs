using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Compute.System;

namespace HCS.Resource.Common
{
	// Token: 0x020000D4 RID: 212
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ResourceModificationRequestResponse
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000B6DF File Offset: 0x000098DF
		public static bool IsJsonDefault(ResourceModificationRequestResponse val)
		{
			return ResourceModificationRequestResponse._default.JsonEquals(val);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B6EC File Offset: 0x000098EC
		public bool JsonEquals(object obj)
		{
			ResourceModificationRequestResponse graph = obj as ResourceModificationRequestResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ResourceModificationRequestResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000B794 File Offset: 0x00009994
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000B7AE File Offset: 0x000099AE
		[DataMember(Name = "ResourceType")]
		private string _ResourceType
		{
			get
			{
				PropertyType resourceType = this.ResourceType;
				return this.ResourceType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ResourceType = PropertyType.Memory;
				}
				this.ResourceType = (PropertyType)Enum.Parse(typeof(PropertyType), value, true);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000B7DC File Offset: 0x000099DC
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000B806 File Offset: 0x00009A06
		[DataMember(EmitDefaultValue = false, Name = "RequestType")]
		private string _RequestType
		{
			get
			{
				if (this.RequestType == RequestType.Add)
				{
					return null;
				}
				return this.RequestType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RequestType = RequestType.Add;
				}
				this.RequestType = (RequestType)Enum.Parse(typeof(RequestType), value, true);
			}
		}

		// Token: 0x0400041F RID: 1055
		private static readonly ResourceModificationRequestResponse _default = new ResourceModificationRequestResponse();

		// Token: 0x04000420 RID: 1056
		public PropertyType ResourceType;

		// Token: 0x04000421 RID: 1057
		public RequestType RequestType;

		// Token: 0x04000422 RID: 1058
		[DataMember(EmitDefaultValue = false)]
		public object Settings;
	}
}
