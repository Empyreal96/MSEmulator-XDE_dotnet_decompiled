using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000063 RID: 99
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class WindowsCrashReport
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00006720 File Offset: 0x00004920
		public static bool IsJsonDefault(WindowsCrashReport val)
		{
			return WindowsCrashReport._default.JsonEquals(val);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006730 File Offset: 0x00004930
		public bool JsonEquals(object obj)
		{
			WindowsCrashReport graph = obj as WindowsCrashReport;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(WindowsCrashReport), new DataContractJsonSerializerSettings
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000067D8 File Offset: 0x000049D8
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00006802 File Offset: 0x00004A02
		[DataMember(EmitDefaultValue = false, Name = "FinalPhase")]
		private string _FinalPhase
		{
			get
			{
				if (this.FinalPhase == WindowsCrashPhase.Inactive)
				{
					return null;
				}
				return this.FinalPhase.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.FinalPhase = WindowsCrashPhase.Inactive;
				}
				this.FinalPhase = (WindowsCrashPhase)Enum.Parse(typeof(WindowsCrashPhase), value, true);
			}
		}

		// Token: 0x04000212 RID: 530
		private static readonly WindowsCrashReport _default = new WindowsCrashReport();

		// Token: 0x04000213 RID: 531
		[DataMember(EmitDefaultValue = false)]
		public string DumpFile;

		// Token: 0x04000214 RID: 532
		[DataMember(EmitDefaultValue = false)]
		public int BugcheckCode;

		// Token: 0x04000215 RID: 533
		[DataMember(EmitDefaultValue = false)]
		public ulong[] BugcheckParameters;

		// Token: 0x04000216 RID: 534
		[DataMember(EmitDefaultValue = false)]
		public uint OsMajorVersion;

		// Token: 0x04000217 RID: 535
		[DataMember(EmitDefaultValue = false)]
		public uint OsMinorVersion;

		// Token: 0x04000218 RID: 536
		[DataMember(EmitDefaultValue = false)]
		public uint OsBuildNumber;

		// Token: 0x04000219 RID: 537
		[DataMember(EmitDefaultValue = false)]
		public uint OsServicePackMajorVersion;

		// Token: 0x0400021A RID: 538
		[DataMember(EmitDefaultValue = false)]
		public uint OsServicePackMinorVersion;

		// Token: 0x0400021B RID: 539
		[DataMember(EmitDefaultValue = false)]
		public uint OsSuiteMask;

		// Token: 0x0400021C RID: 540
		[DataMember(EmitDefaultValue = false)]
		public uint OsProductType;

		// Token: 0x0400021D RID: 541
		[DataMember(EmitDefaultValue = false)]
		public int Status;

		// Token: 0x0400021E RID: 542
		public WindowsCrashPhase FinalPhase;
	}
}
