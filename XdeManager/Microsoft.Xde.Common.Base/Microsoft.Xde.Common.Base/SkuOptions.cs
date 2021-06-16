using System;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001F RID: 31
	public class SkuOptions
	{
		// Token: 0x0600014C RID: 332 RVA: 0x00003B28 File Offset: 0x00001D28
		public SkuOptions()
		{
			this.ProcessorCount = 2;
			this.DefaultMemSize = 4096;
			this.MinMemSize = (this.MaxMemSize = this.DefaultMemSize);
			this.NATDisabled = true;
			this.WindowsKeyCombinationsEnabled = false;
			this.WindowsKeyEnabled = false;
			this.ValidSensors = XdeSensors.All.ToString();
			this.GuestDisplayProvider = "StockPlugin.RemoteDesktopGuestDisplay";
			this.InputMode = TouchMode.Mouse;
			this.HostCursorDisabledInMouseMode = false;
			this.SupportsSDCard = false;
			this.UseHCSIfAvailable = false;
			this.ShowGuestDisplayASAP = false;
			this.GpuAssignmentMode = GpuAssignmentMode.Default;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00003BC5 File Offset: 0x00001DC5
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00003BCD File Offset: 0x00001DCD
		[XmlAttribute]
		public int DefaultMemSize { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00003BD6 File Offset: 0x00001DD6
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00003BDE File Offset: 0x00001DDE
		[XmlAttribute]
		public int MaxMemSize { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00003BE7 File Offset: 0x00001DE7
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00003BEF File Offset: 0x00001DEF
		[XmlAttribute]
		public int MinMemSize { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00003BF8 File Offset: 0x00001DF8
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00003C00 File Offset: 0x00001E00
		[XmlAttribute]
		public int ProcessorCount { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00003C09 File Offset: 0x00001E09
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00003C11 File Offset: 0x00001E11
		[XmlAttribute]
		public bool NATDisabled { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00003C1A File Offset: 0x00001E1A
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00003C22 File Offset: 0x00001E22
		[XmlAttribute]
		public bool WindowsKeyCombinationsEnabled { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00003C2B File Offset: 0x00001E2B
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00003C33 File Offset: 0x00001E33
		[XmlAttribute]
		public bool WindowsKeyEnabled { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00003C3C File Offset: 0x00001E3C
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00003C44 File Offset: 0x00001E44
		[XmlAttribute]
		public bool WriteVhdBootSettingsDisabled { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00003C4D File Offset: 0x00001E4D
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00003C55 File Offset: 0x00001E55
		[XmlAttribute]
		public string ValidSensors { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00003C5E File Offset: 0x00001E5E
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00003C66 File Offset: 0x00001E66
		[XmlAttribute]
		public string GuestDisplayProvider { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00003C6F File Offset: 0x00001E6F
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00003C77 File Offset: 0x00001E77
		[XmlAttribute]
		public bool HostCursorDisabledInMouseMode { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00003C80 File Offset: 0x00001E80
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00003C88 File Offset: 0x00001E88
		[XmlAttribute]
		public TouchMode InputMode { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003C91 File Offset: 0x00001E91
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00003C99 File Offset: 0x00001E99
		[XmlAttribute]
		public bool SupportsSDCard { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00003CA2 File Offset: 0x00001EA2
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00003CAA File Offset: 0x00001EAA
		[XmlAttribute]
		public bool UseHCSIfAvailable { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00003CB3 File Offset: 0x00001EB3
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00003CBB File Offset: 0x00001EBB
		[XmlAttribute]
		public GpuAssignmentMode GpuAssignmentMode { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00003CC4 File Offset: 0x00001EC4
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00003CCC File Offset: 0x00001ECC
		[XmlAttribute]
		public bool ShowGuestDisplayASAP { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00003CD5 File Offset: 0x00001ED5
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00003CDD File Offset: 0x00001EDD
		[XmlAttribute]
		public string DefaultDeviceName { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00003CE6 File Offset: 0x00001EE6
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00003CEE File Offset: 0x00001EEE
		[XmlAttribute]
		public string DefaultSkin { get; set; }

		// Token: 0x040000DE RID: 222
		private const int DefaultProcessorCount = 2;

		// Token: 0x040000DF RID: 223
		private const int DefaultMemorySize = 4096;

		// Token: 0x040000E0 RID: 224
		private const bool DefaultNATDisabled = true;

		// Token: 0x040000E1 RID: 225
		private const bool DefaultWindowsKeyCombinationsEnabled = false;

		// Token: 0x040000E2 RID: 226
		private const bool DefaultWindowsKeyEnabled = false;

		// Token: 0x040000E3 RID: 227
		private const XdeSensors DefaultEnabledSensors = XdeSensors.Default;

		// Token: 0x040000E4 RID: 228
		private const string DefaultGuestDisplayProvider = "StockPlugin.RemoteDesktopGuestDisplay";

		// Token: 0x040000E5 RID: 229
		private const TouchMode DefaultInputMode = TouchMode.Mouse;

		// Token: 0x040000E6 RID: 230
		private const bool DefaultMouseCursorDisabledInMouseMode = false;

		// Token: 0x040000E7 RID: 231
		private const bool DefaultSupportsSDCard = false;

		// Token: 0x040000E8 RID: 232
		private const bool DefaultUseHCSIfAvailable = false;

		// Token: 0x040000E9 RID: 233
		private const bool DefaultShowGuestDisplayASAP = false;

		// Token: 0x040000EA RID: 234
		private const GpuAssignmentMode DefaultGpuAssignmentMode = GpuAssignmentMode.Default;
	}
}
