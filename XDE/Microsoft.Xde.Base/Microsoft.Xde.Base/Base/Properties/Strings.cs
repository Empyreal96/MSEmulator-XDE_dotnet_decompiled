using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Base.Properties
{
	// Token: 0x0200000C RID: 12
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00003C04 File Offset: 0x00001E04
		internal Strings()
		{
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004F60 File Offset: 0x00003160
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Strings.resourceMan == null)
				{
					Strings.resourceMan = new ResourceManager("Microsoft.Xde.Base.Properties.Strings", typeof(Strings).Assembly);
				}
				return Strings.resourceMan;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004F8C File Offset: 0x0000318C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004F93 File Offset: 0x00003193
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Strings.resourceCulture;
			}
			set
			{
				Strings.resourceCulture = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004F9B File Offset: 0x0000319B
		internal static string CannotModifyInternalAdapter
		{
			get
			{
				return Strings.ResourceManager.GetString("CannotModifyInternalAdapter", Strings.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004FB1 File Offset: 0x000031B1
		internal static string CanOnlySetInformationTextOnce
		{
			get
			{
				return Strings.ResourceManager.GetString("CanOnlySetInformationTextOnce", Strings.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004FC7 File Offset: 0x000031C7
		internal static string ConfigureExternalNicInstruction
		{
			get
			{
				return Strings.ResourceManager.GetString("ConfigureExternalNicInstruction", Strings.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004FDD File Offset: 0x000031DD
		internal static string ConfigureExternalNicText
		{
			get
			{
				return Strings.ResourceManager.GetString("ConfigureExternalNicText", Strings.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004FF3 File Offset: 0x000031F3
		internal static string FailedToInitializeExternalSwitchesFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToInitializeExternalSwitchesFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005009 File Offset: 0x00003209
		internal static string FailedToInitializeInternalSwitchFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToInitializeInternalSwitchFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000501F File Offset: 0x0000321F
		internal static string FailedToInitNatConfig
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToInitNatConfig", Strings.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005035 File Offset: 0x00003235
		internal static string FailedToResolveIPConflict
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToResolveIPConflict", Strings.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000504B File Offset: 0x0000324B
		internal static string FailedToSetIPAddressOnNatNic
		{
			get
			{
				return Strings.ResourceManager.GetString("FailedToSetIPAddressOnNatNic", Strings.resourceCulture);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00005061 File Offset: 0x00003261
		internal static string FixNetworkAdapterSettingsInstruction
		{
			get
			{
				return Strings.ResourceManager.GetString("FixNetworkAdapterSettingsInstruction", Strings.resourceCulture);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005077 File Offset: 0x00003277
		internal static string FixNetworkAdapterSettingsText
		{
			get
			{
				return Strings.ResourceManager.GetString("FixNetworkAdapterSettingsText", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000508D File Offset: 0x0000328D
		internal static string GuestDidntReturnValidIP
		{
			get
			{
				return Strings.ResourceManager.GetString("GuestDidntReturnValidIP", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000050A3 File Offset: 0x000032A3
		internal static string HostAddressNotFound
		{
			get
			{
				return Strings.ResourceManager.GetString("HostAddressNotFound", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000050B9 File Offset: 0x000032B9
		internal static string RetryRunningAsAdmin
		{
			get
			{
				return Strings.ResourceManager.GetString("RetryRunningAsAdmin", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000050CF File Offset: 0x000032CF
		internal static string SkuLoad_NumProcsInvalid
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoad_NumProcsInvalid", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000050E5 File Offset: 0x000032E5
		internal static string SkuLoad_RamSizeInvalid
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoad_RamSizeInvalid", Strings.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000050FB File Offset: 0x000032FB
		internal static string SkuLoader_BrandingNotFoundFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_BrandingNotFoundFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005111 File Offset: 0x00003311
		internal static string SkuLoader_ButtonNotFoundFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_ButtonNotFoundFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005127 File Offset: 0x00003327
		internal static string SkuLoader_FeatureNotFoundFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_FeatureNotFoundFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000513D File Offset: 0x0000333D
		internal static string SkuLoader_GuestDisplayFactoryNotFoundFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_GuestDisplayFactoryNotFoundFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005153 File Offset: 0x00003353
		internal static string SkuLoader_OptionUnrecognized
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_OptionUnrecognized", Strings.resourceCulture);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005169 File Offset: 0x00003369
		internal static string SkuLoader_OptionValueInvalid
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_OptionValueInvalid", Strings.resourceCulture);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000517F File Offset: 0x0000337F
		internal static string SkuLoader_TabNotFoundFormat
		{
			get
			{
				return Strings.ResourceManager.GetString("SkuLoader_TabNotFoundFormat", Strings.resourceCulture);
			}
		}

		// Token: 0x0400004E RID: 78
		private static ResourceManager resourceMan;

		// Token: 0x0400004F RID: 79
		private static CultureInfo resourceCulture;
	}
}
