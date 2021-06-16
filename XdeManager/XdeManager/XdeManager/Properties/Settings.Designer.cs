using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Xde.Common;

namespace XdeManager.Properties
{
	// Token: 0x0200000B RID: 11
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.2.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B17 File Offset: 0x00000D17
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002B1E File Offset: 0x00000D1E
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002B30 File Offset: 0x00000D30
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>rs_wnext_dev_modernux</string>\r\n  <string>rs_onecore_dep_apt7</string>\r\n  <string>rsmaster</string>\r\n</ArrayOfString>")]
		public StringCollection FavoriteBranches
		{
			get
			{
				return (StringCollection)this["FavoriteBranches"];
			}
			set
			{
				this["FavoriteBranches"] = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002B3E File Offset: 0x00000D3E
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002B50 File Offset: 0x00000D50
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string LastUsedBranch
		{
			get
			{
				return (string)this["LastUsedBranch"];
			}
			set
			{
				this["LastUsedBranch"] = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002B7E File Offset: 0x00000D7E
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002B90 File Offset: 0x00000D90
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("100")]
		public double WindowTop
		{
			get
			{
				return (double)this["WindowTop"];
			}
			set
			{
				this["WindowTop"] = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002BA3 File Offset: 0x00000DA3
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002BB5 File Offset: 0x00000DB5
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("100")]
		public double WindowLeft
		{
			get
			{
				return (double)this["WindowLeft"];
			}
			set
			{
				this["WindowLeft"] = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002BC8 File Offset: 0x00000DC8
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002BDA File Offset: 0x00000DDA
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("540")]
		public double WindowHeight
		{
			get
			{
				return (double)this["WindowHeight"];
			}
			set
			{
				this["WindowHeight"] = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002BED File Offset: 0x00000DED
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002BFF File Offset: 0x00000DFF
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("900")]
		public double WindowWidth
		{
			get
			{
				return (double)this["WindowWidth"];
			}
			set
			{
				this["WindowWidth"] = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002C12 File Offset: 0x00000E12
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002C24 File Offset: 0x00000E24
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("Normal")]
		public WindowState WindowState
		{
			get
			{
				return (WindowState)this["WindowState"];
			}
			set
			{
				this["WindowState"] = value;
			}
		}

		// Token: 0x04000020 RID: 32
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
