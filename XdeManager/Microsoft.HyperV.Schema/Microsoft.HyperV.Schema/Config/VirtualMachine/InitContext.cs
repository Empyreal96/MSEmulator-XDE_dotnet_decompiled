using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VirtualMachine
{
	// Token: 0x02000107 RID: 263
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class InitContext
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x0000DE88 File Offset: 0x0000C088
		public static bool IsJsonDefault(InitContext val)
		{
			return InitContext._default.JsonEquals(val);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000DE98 File Offset: 0x0000C098
		public bool JsonEquals(object obj)
		{
			InitContext graph = obj as InitContext;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(InitContext), new DataContractJsonSerializerSettings
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000DF40 File Offset: 0x0000C140
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0000DF48 File Offset: 0x0000C148
		[DataMember(Name = "MemorySettings")]
		private MemorySettings _MemorySettings
		{
			get
			{
				return this.MemorySettings;
			}
			set
			{
				if (value != null)
				{
					this.MemorySettings = value;
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000DF54 File Offset: 0x0000C154
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x0000DF5C File Offset: 0x0000C15C
		[DataMember(Name = "MigrationProcessorFeatures")]
		private MigrationProcessorFeatures _MigrationProcessorFeatures
		{
			get
			{
				return this.MigrationProcessorFeatures;
			}
			set
			{
				if (value != null)
				{
					this.MigrationProcessorFeatures = value;
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000DF68 File Offset: 0x0000C168
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0000DF70 File Offset: 0x0000C170
		[DataMember(Name = "MigrationPartitionProperties")]
		private MigrationPartitionProperties _MigrationPartitionProperties
		{
			get
			{
				return this.MigrationPartitionProperties;
			}
			set
			{
				if (value != null)
				{
					this.MigrationPartitionProperties = value;
				}
			}
		}

		// Token: 0x0400052B RID: 1323
		private static readonly InitContext _default = new InitContext();

		// Token: 0x0400052C RID: 1324
		[DataMember]
		public int InitReason;

		// Token: 0x0400052D RID: 1325
		[DataMember]
		public bool CreateAsPPL;

		// Token: 0x0400052E RID: 1326
		[DataMember]
		public string RuntimeStateFile;

		// Token: 0x0400052F RID: 1327
		[DataMember]
		public bool UseTransientState;

		// Token: 0x04000530 RID: 1328
		[DataMember]
		public bool ResetRuntimeState;

		// Token: 0x04000531 RID: 1329
		[DataMember]
		public string RuntimeStateTemplate;

		// Token: 0x04000532 RID: 1330
		[DataMember]
		public bool EnableFastSaveToMemoryBlock;

		// Token: 0x04000533 RID: 1331
		[DataMember]
		public string FailoverSavedStateFile;

		// Token: 0x04000534 RID: 1332
		[DataMember]
		public bool SkipSavedStateFileCleanup;

		// Token: 0x04000535 RID: 1333
		[DataMember]
		public bool SaveVmOnBugcheck;

		// Token: 0x04000536 RID: 1334
		[DataMember]
		public string BugcheckSavedStateFile;

		// Token: 0x04000537 RID: 1335
		[DataMember]
		public bool SkipMetrics;

		// Token: 0x04000538 RID: 1336
		public MemorySettings MemorySettings = new MemorySettings();

		// Token: 0x04000539 RID: 1337
		[DataMember]
		public bool IdeControllerPresent;

		// Token: 0x0400053A RID: 1338
		[DataMember]
		public uint ScsiControllerCount;

		// Token: 0x0400053B RID: 1339
		[DataMember]
		public bool PMemControllerPresent;

		// Token: 0x0400053C RID: 1340
		public MigrationProcessorFeatures MigrationProcessorFeatures = new MigrationProcessorFeatures();

		// Token: 0x0400053D RID: 1341
		public MigrationPartitionProperties MigrationPartitionProperties = new MigrationPartitionProperties();

		// Token: 0x0400053E RID: 1342
		[DataMember]
		public ulong StartupTimeoutSeconds;
	}
}
