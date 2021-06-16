using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.VirtualMachines.Resources;

namespace HCS.Config.Devices.Bios
{
	// Token: 0x02000161 RID: 353
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class BiosLoader
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x00011E05 File Offset: 0x00010005
		public static bool IsJsonDefault(BiosLoader val)
		{
			return BiosLoader._default.JsonEquals(val);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00011E14 File Offset: 0x00010014
		public bool JsonEquals(object obj)
		{
			BiosLoader graph = obj as BiosLoader;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(BiosLoader), new DataContractJsonSerializerSettings
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

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00011EBC File Offset: 0x000100BC
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00011EC4 File Offset: 0x000100C4
		[DataMember(Name = "base_board")]
		private BaseBoard _BaseBoard
		{
			get
			{
				return this.BaseBoard;
			}
			set
			{
				if (value != null)
				{
					this.BaseBoard = value;
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00011ED0 File Offset: 0x000100D0
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00011ED8 File Offset: 0x000100D8
		[DataMember(Name = "chassis")]
		private Chassis _Chassis
		{
			get
			{
				return this.Chassis;
			}
			set
			{
				if (value != null)
				{
					this.Chassis = value;
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00011EE4 File Offset: 0x000100E4
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00011EF4 File Offset: 0x000100F4
		[DataMember(EmitDefaultValue = false, Name = "bios_flags")]
		private ulong _BiosFlags
		{
			get
			{
				BiosFlags biosFlags = this.BiosFlags;
				return (ulong)((long)this.BiosFlags);
			}
			set
			{
				this.BiosFlags = (BiosFlags)value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00011EFE File Offset: 0x000100FE
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00011F0E File Offset: 0x0001010E
		[DataMember(EmitDefaultValue = false, Name = "pxe_preferred_protocol")]
		private ulong _PxePreferredProtocol
		{
			get
			{
				PxeProtocol pxePreferredProtocol = this.PxePreferredProtocol;
				return (ulong)((long)this.PxePreferredProtocol);
			}
			set
			{
				this.PxePreferredProtocol = (PxeProtocol)value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00011F18 File Offset: 0x00010118
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00011F28 File Offset: 0x00010128
		[DataMember(EmitDefaultValue = false, Name = "console_mode")]
		private ulong _ConsoleMode
		{
			get
			{
				ConsoleMode consoleMode = this.ConsoleMode;
				return (ulong)((long)this.ConsoleMode);
			}
			set
			{
				this.ConsoleMode = (ConsoleMode)value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00011F32 File Offset: 0x00010132
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00011F49 File Offset: 0x00010149
		[DataMember(EmitDefaultValue = false, Name = "boot")]
		private Boot _Boot
		{
			get
			{
				if (!Boot.IsJsonDefault(this.Boot))
				{
					return this.Boot;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Boot = value;
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00011F58 File Offset: 0x00010158
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00011F82 File Offset: 0x00010182
		[DataMember(EmitDefaultValue = false, Name = "FirmwareMode")]
		private string _FirmwareMode
		{
			get
			{
				if (this.FirmwareMode == FirmwareMode.UEFI)
				{
					return null;
				}
				return this.FirmwareMode.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.FirmwareMode = FirmwareMode.UEFI;
				}
				this.FirmwareMode = (FirmwareMode)Enum.Parse(typeof(FirmwareMode), value, true);
			}
		}

		// Token: 0x04000741 RID: 1857
		private static readonly BiosLoader _default = new BiosLoader();

		// Token: 0x04000742 RID: 1858
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000743 RID: 1859
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000744 RID: 1860
		[DataMember(Name = "bios_guid")]
		public Guid BiosGuid;

		// Token: 0x04000745 RID: 1861
		[DataMember(Name = "bios_serial_number")]
		public string SerialNumber;

		// Token: 0x04000746 RID: 1862
		[DataMember(Name = "num_lock")]
		public bool NumLockEnabled;

		// Token: 0x04000747 RID: 1863
		public BaseBoard BaseBoard = new BaseBoard();

		// Token: 0x04000748 RID: 1864
		public Chassis Chassis = new Chassis();

		// Token: 0x04000749 RID: 1865
		[DataMember(Name = "secure_boot_enabled")]
		public bool SecureBootEnabled;

		// Token: 0x0400074A RID: 1866
		[DataMember(Name = "secure_boot_template_id")]
		public Guid SecureBootTemplateId;

		// Token: 0x0400074B RID: 1867
		public BiosFlags BiosFlags;

		// Token: 0x0400074C RID: 1868
		[DataMember(Name = "pause_after_boot_failure")]
		public bool PauseAfterBootFailure;

		// Token: 0x0400074D RID: 1869
		public PxeProtocol PxePreferredProtocol;

		// Token: 0x0400074E RID: 1870
		public ConsoleMode ConsoleMode;

		// Token: 0x0400074F RID: 1871
		public Boot Boot = new Boot();

		// Token: 0x04000750 RID: 1872
		[DataMember(EmitDefaultValue = false, Name = "imc_data")]
		public byte[] ImcData;

		// Token: 0x04000751 RID: 1873
		[DataMember(EmitDefaultValue = false, Name = "memory_attributes_table")]
		public bool? MemoryAttributes;

		// Token: 0x04000752 RID: 1874
		[DataMember(EmitDefaultValue = false, Name = "nvram")]
		public object Nvram;

		// Token: 0x04000753 RID: 1875
		[DataMember(EmitDefaultValue = false, Name = "boot_next")]
		public UefiBootEntry BootThis;

		// Token: 0x04000754 RID: 1876
		public FirmwareMode FirmwareMode;

		// Token: 0x04000755 RID: 1877
		[DataMember(EmitDefaultValue = false)]
		public string BiosLockString;

		// Token: 0x04000756 RID: 1878
		[DataMember(EmitDefaultValue = false)]
		public bool EnableHibernation;

		// Token: 0x04000757 RID: 1879
		[DataMember(EmitDefaultValue = false)]
		public LinuxKernelDirect LinuxKernelDirect;

		// Token: 0x04000758 RID: 1880
		[DataMember(EmitDefaultValue = false)]
		public bool DisableFrontpage;
	}
}
