using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005D RID: 93
	public interface IXdeVirtualMachineSettings : IDisposable, INotifyPropertyChanged
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001ED RID: 493
		int Generation { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001EE RID: 494
		DateTime CreationTime { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001EF RID: 495
		// (set) Token: 0x060001F0 RID: 496
		string Name { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F1 RID: 497
		string UniqueId { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001F2 RID: 498
		// (set) Token: 0x060001F3 RID: 499
		string VhdPath { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001F4 RID: 500
		// (set) Token: 0x060001F5 RID: 501
		int NumProcessors { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001F6 RID: 502
		// (set) Token: 0x060001F7 RID: 503
		int RamSize { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001F8 RID: 504
		// (set) Token: 0x060001F9 RID: 505
		string Com1NamedPipe { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FA RID: 506
		// (set) Token: 0x060001FB RID: 507
		string Com2NamedPipe { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001FC RID: 508
		ReadOnlyCollection<IXdeVirtualMachineNicInformation> Nics { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001FD RID: 509
		bool IsInvalid { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001FE RID: 510
		InvalidSettingsReason InvalidSettingsReason { get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001FF RID: 511
		// (set) Token: 0x06000200 RID: 512
		string Notes { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000201 RID: 513
		Exception AsyncLoadException { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000202 RID: 514
		bool IsUsingGpu { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000203 RID: 515
		bool IsSnapshot { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000204 RID: 516
		VGPUInformation VGPUInformation { get; }

		// Token: 0x06000205 RID: 517
		ulong GetFilesSize();

		// Token: 0x06000206 RID: 518
		Bitmap GetThumbnail(int width);

		// Token: 0x06000207 RID: 519
		void WaitForLoadedSettings();

		// Token: 0x06000208 RID: 520
		void EnsureHasDisplayAdapter(VGPUStatus vgpuStatus, Size requestedResolution, int vgpuRamMB, GpuAssignmentMode gpuAssignmentMode);

		// Token: 0x06000209 RID: 521
		void CleanupForDeletion();

		// Token: 0x0600020A RID: 522
		void Delete();
	}
}
