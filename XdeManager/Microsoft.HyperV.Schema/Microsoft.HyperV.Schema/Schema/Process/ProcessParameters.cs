using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Process
{
	// Token: 0x02000080 RID: 128
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessParameters
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00007B60 File Offset: 0x00005D60
		public static bool IsJsonDefault(ProcessParameters val)
		{
			return ProcessParameters._default.JsonEquals(val);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007B70 File Offset: 0x00005D70
		public bool JsonEquals(object obj)
		{
			ProcessParameters graph = obj as ProcessParameters;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessParameters), new DataContractJsonSerializerSettings
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

		// Token: 0x040002C0 RID: 704
		private static readonly ProcessParameters _default = new ProcessParameters();

		// Token: 0x040002C1 RID: 705
		[DataMember(EmitDefaultValue = false)]
		public string ApplicationName;

		// Token: 0x040002C2 RID: 706
		[DataMember(EmitDefaultValue = false)]
		public string CommandLine;

		// Token: 0x040002C3 RID: 707
		[DataMember(EmitDefaultValue = false)]
		public string[] CommandArgs;

		// Token: 0x040002C4 RID: 708
		[DataMember(EmitDefaultValue = false)]
		public string User;

		// Token: 0x040002C5 RID: 709
		[DataMember(EmitDefaultValue = false)]
		public string WorkingDirectory;

		// Token: 0x040002C6 RID: 710
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<string, string> Environment;

		// Token: 0x040002C7 RID: 711
		[DataMember(EmitDefaultValue = false)]
		public bool RestrictedToken;

		// Token: 0x040002C8 RID: 712
		[DataMember(EmitDefaultValue = false)]
		public bool EmulateConsole;

		// Token: 0x040002C9 RID: 713
		[DataMember(EmitDefaultValue = false)]
		public bool CreateStdInPipe;

		// Token: 0x040002CA RID: 714
		[DataMember(EmitDefaultValue = false)]
		public bool CreateStdOutPipe;

		// Token: 0x040002CB RID: 715
		[DataMember(EmitDefaultValue = false)]
		public bool CreateStdErrPipe;

		// Token: 0x040002CC RID: 716
		[DataMember(EmitDefaultValue = false)]
		public ushort[] ConsoleSize;

		// Token: 0x040002CD RID: 717
		[DataMember(EmitDefaultValue = false)]
		public bool CreateInUtilityVM;

		// Token: 0x040002CE RID: 718
		[DataMember(EmitDefaultValue = false)]
		public bool UseExistingLogin;

		// Token: 0x040002CF RID: 719
		[DataMember(EmitDefaultValue = false)]
		public bool? UseLegacyConsole;
	}
}
