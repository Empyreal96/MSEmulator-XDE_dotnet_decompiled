using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VirtualMachine
{
	// Token: 0x02000106 RID: 262
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MigrationPartitionProperties
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x0000DDBC File Offset: 0x0000BFBC
		public static bool IsJsonDefault(MigrationPartitionProperties val)
		{
			return MigrationPartitionProperties._default.JsonEquals(val);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		public bool JsonEquals(object obj)
		{
			MigrationPartitionProperties graph = obj as MigrationPartitionProperties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MigrationPartitionProperties), new DataContractJsonSerializerSettings
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

		// Token: 0x04000529 RID: 1321
		private static readonly MigrationPartitionProperties _default = new MigrationPartitionProperties();

		// Token: 0x0400052A RID: 1322
		[DataMember]
		public uint HwThreadsPerCore;
	}
}
