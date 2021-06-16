using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x02000011 RID: 17
	public class MockXdeManagerViewModel : XdeManagerViewModel
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00004131 File Offset: 0x00002331
		public MockXdeManagerViewModel() : base(null, new EnumerateDevices(new MockXdeManagerViewModel.MockXdeDeviceFactory().GetDevices))
		{
		}

		// Token: 0x0200001F RID: 31
		private class MockXdeDeviceFactory
		{
			// Token: 0x06000151 RID: 337 RVA: 0x000052D7 File Offset: 0x000034D7
			public IEnumerable<XdeDevice> GetDevices()
			{
				return MockXdeManagerViewModel.MockXdeDeviceFactory.devices;
			}

			// Token: 0x04000076 RID: 118
			private static XdeDevice[] devices = new XdeDevice[]
			{
				new MockXdeManagerViewModel.MockXdeDeviceFactory.MockXdeDevice
				{
					Name = "Mock Device 10.0.18877.0",
					BackingXdeLocation = "C:\\Program Files (x86)\\Windows Kits\\10\\Microsoft XDE\\10.0.18877.0\\xde.exe",
					Vhd = "C:\\Program Files (x86)\\Windows Kits\\10\\Emulation\\Mock\\10.0.18877.0\\flash.vhdx",
					Sku = "S1",
					Skin = "full",
					MemSize = 2048
				},
				new MockXdeManagerViewModel.MockXdeDeviceFactory.MockXdeDevice
				{
					Name = "Mock Device 10.0.18877.0",
					BackingXdeLocation = "C:\\Program Files (x86)\\Windows Kits\\10\\Microsoft XDE\\10.0.18877.0\\xde.exe",
					Vhd = "C:\\Program Files (x86)\\Windows Kits\\10\\Emulation\\Mock\\10.0.18877.0\\flash.vhdx",
					Sku = "S1",
					Skin = "full",
					MemSize = 2048
				},
				new MockXdeManagerViewModel.MockXdeDeviceFactory.MockXdeDevice
				{
					Name = "Ye Old Mock Phone 10.0.1024.0",
					BackingXdeLocation = "C:\\Program Files (x86)\\Windows Kits\\10\\Microsoft XDE\\10.0.1024.0\\xde.exe",
					Vhd = "C:\\Program Files (x86)\\Windows Kits\\10\\Emulation\\MockWM\\10.0.1024.0\\flash.vhdx",
					Sku = "wm",
					Skin = "full",
					MemSize = 1024
				}
			};

			// Token: 0x02000024 RID: 36
			private class MockXdeDevice : XdeDevice
			{
				// Token: 0x17000086 RID: 134
				// (get) Token: 0x06000164 RID: 356 RVA: 0x000056C4 File Offset: 0x000038C4
				// (set) Token: 0x06000165 RID: 357 RVA: 0x000056CC File Offset: 0x000038CC
				public string BackingXdeLocation { get; set; }

				// Token: 0x17000087 RID: 135
				// (get) Token: 0x06000166 RID: 358 RVA: 0x000056D5 File Offset: 0x000038D5
				// (set) Token: 0x06000167 RID: 359 RVA: 0x000056DD File Offset: 0x000038DD
				public override string Name { get; set; }

				// Token: 0x17000088 RID: 136
				// (get) Token: 0x06000168 RID: 360 RVA: 0x000056E6 File Offset: 0x000038E6
				// (set) Token: 0x06000169 RID: 361 RVA: 0x000056EE File Offset: 0x000038EE
				public override string Vhd { get; set; }

				// Token: 0x17000089 RID: 137
				// (get) Token: 0x0600016A RID: 362 RVA: 0x000056F7 File Offset: 0x000038F7
				// (set) Token: 0x0600016B RID: 363 RVA: 0x000056FF File Offset: 0x000038FF
				public override bool UseCheckpoint { get; set; }

				// Token: 0x1700008A RID: 138
				// (get) Token: 0x0600016C RID: 364 RVA: 0x00005708 File Offset: 0x00003908
				// (set) Token: 0x0600016D RID: 365 RVA: 0x00005710 File Offset: 0x00003910
				public override string Sku { get; set; }

				// Token: 0x1700008B RID: 139
				// (get) Token: 0x0600016E RID: 366 RVA: 0x00005719 File Offset: 0x00003919
				// (set) Token: 0x0600016F RID: 367 RVA: 0x00005721 File Offset: 0x00003921
				public override string Skin { get; set; }

				// Token: 0x1700008C RID: 140
				// (get) Token: 0x06000170 RID: 368 RVA: 0x0000572A File Offset: 0x0000392A
				public override string XdeLocation
				{
					get
					{
						return this.BackingXdeLocation;
					}
				}

				// Token: 0x1700008D RID: 141
				// (get) Token: 0x06000171 RID: 369 RVA: 0x00005732 File Offset: 0x00003932
				// (set) Token: 0x06000172 RID: 370 RVA: 0x0000573A File Offset: 0x0000393A
				public override int MemSize { get; set; }

				// Token: 0x1700008E RID: 142
				// (get) Token: 0x06000173 RID: 371 RVA: 0x00005743 File Offset: 0x00003943
				public override string FileName { get; }

				// Token: 0x1700008F RID: 143
				// (get) Token: 0x06000174 RID: 372 RVA: 0x0000574B File Offset: 0x0000394B
				public override bool CanDelete
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x17000090 RID: 144
				// (get) Token: 0x06000175 RID: 373 RVA: 0x00005752 File Offset: 0x00003952
				// (set) Token: 0x06000176 RID: 374 RVA: 0x0000575A File Offset: 0x0000395A
				public override bool NoGpu { get; set; }

				// Token: 0x17000091 RID: 145
				// (get) Token: 0x06000177 RID: 375 RVA: 0x00005763 File Offset: 0x00003963
				// (set) Token: 0x06000178 RID: 376 RVA: 0x0000576B File Offset: 0x0000396B
				public override bool ShowDisplayName { get; set; }

				// Token: 0x17000092 RID: 146
				// (get) Token: 0x06000179 RID: 377 RVA: 0x00005774 File Offset: 0x00003974
				// (set) Token: 0x0600017A RID: 378 RVA: 0x0000577C File Offset: 0x0000397C
				public override string DisplayName { get; set; }

				// Token: 0x17000093 RID: 147
				// (get) Token: 0x0600017B RID: 379 RVA: 0x00005785 File Offset: 0x00003985
				// (set) Token: 0x0600017C RID: 380 RVA: 0x0000578D File Offset: 0x0000398D
				public override bool UseWmi { get; set; }

				// Token: 0x17000094 RID: 148
				// (get) Token: 0x0600017D RID: 381 RVA: 0x00005796 File Offset: 0x00003996
				// (set) Token: 0x0600017E RID: 382 RVA: 0x0000579E File Offset: 0x0000399E
				public override bool DisableStateSep { get; set; }

				// Token: 0x17000095 RID: 149
				// (get) Token: 0x0600017F RID: 383 RVA: 0x000057A7 File Offset: 0x000039A7
				public override bool IsDirty
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x17000096 RID: 150
				// (get) Token: 0x06000180 RID: 384 RVA: 0x000057AE File Offset: 0x000039AE
				// (set) Token: 0x06000181 RID: 385 RVA: 0x000057B6 File Offset: 0x000039B6
				public override Guid ID
				{
					get
					{
						return this.id;
					}
					set
					{
						this.id = value;
					}
				}

				// Token: 0x17000097 RID: 151
				// (get) Token: 0x06000182 RID: 386 RVA: 0x000057BF File Offset: 0x000039BF
				// (set) Token: 0x06000183 RID: 387 RVA: 0x000057C7 File Offset: 0x000039C7
				public override bool UseDiffDisk { get; set; }

				// Token: 0x17000098 RID: 152
				// (get) Token: 0x06000184 RID: 388 RVA: 0x000057D0 File Offset: 0x000039D0
				public override bool CanKernelDebug
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x17000099 RID: 153
				// (get) Token: 0x06000185 RID: 389 RVA: 0x000057D7 File Offset: 0x000039D7
				public override string UapVersion
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x1700009A RID: 154
				// (get) Token: 0x06000186 RID: 390 RVA: 0x000057DE File Offset: 0x000039DE
				public override string MacAddress
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x1700009B RID: 155
				// (get) Token: 0x06000187 RID: 391 RVA: 0x000057E5 File Offset: 0x000039E5
				protected override bool UsingOldEmulator
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x06000188 RID: 392 RVA: 0x000057EC File Offset: 0x000039EC
				public override Task Delete()
				{
					throw new NotImplementedException();
				}

				// Token: 0x06000189 RID: 393 RVA: 0x000057F3 File Offset: 0x000039F3
				public override void Save()
				{
					throw new NotImplementedException();
				}

				// Token: 0x04000086 RID: 134
				private Guid id = Guid.NewGuid();
			}
		}
	}
}
