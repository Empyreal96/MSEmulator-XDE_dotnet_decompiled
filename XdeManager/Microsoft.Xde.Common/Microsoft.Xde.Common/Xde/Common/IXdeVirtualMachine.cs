using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005B RID: 91
	public interface IXdeVirtualMachine : IDisposable
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060001D1 RID: 465
		// (remove) Token: 0x060001D2 RID: 466
		event EventHandler<EnabledStateChangedEventArgs> EnableStateChanged;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060001D3 RID: 467
		// (remove) Token: 0x060001D4 RID: 468
		event EventHandler<EventArgs> SnapshotsChanged;

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001D5 RID: 469
		VirtualMachineEnabledState EnabledState { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001D6 RID: 470
		string Guid { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001D7 RID: 471
		string Name { get; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001D8 RID: 472
		IXdeVirtualMachineSettings CurrentSettings { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001D9 RID: 473
		ReadOnlyCollection<IXdeVirtualMachineSettings> SnapshotSettings { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001DA RID: 474
		IXdeVirtualMachineNicInformation InternalNATNic { get; }

		// Token: 0x060001DB RID: 475
		Size GetCurrentResolution();

		// Token: 0x060001DC RID: 476
		void DeleteVirtualMachine();

		// Token: 0x060001DD RID: 477
		void Start();

		// Token: 0x060001DE RID: 478
		void Start(bool bootingToTakeSnapshot);

		// Token: 0x060001DF RID: 479
		void Stop();

		// Token: 0x060001E0 RID: 480
		void IntentionalShutdownComing();

		// Token: 0x060001E1 RID: 481
		void TypeKey(Keys key);

		// Token: 0x060001E2 RID: 482
		void PressKey(Keys key);

		// Token: 0x060001E3 RID: 483
		void ReleaseKey(Keys key);

		// Token: 0x060001E4 RID: 484
		void SendMouseEvent(MouseEventArgs args);

		// Token: 0x060001E5 RID: 485
		Image GetScreenShot(int startX, int startY, int width, int height);

		// Token: 0x060001E6 RID: 486
		IXdeVirtualMachineSettings FindSnapshotSettings(string snapshotName);

		// Token: 0x060001E7 RID: 487
		void CreateSnapshot(string snapshotName);

		// Token: 0x060001E8 RID: 488
		void ApplySnapshot(IXdeVirtualMachineSettings snapshotSettings);

		// Token: 0x060001E9 RID: 489
		void RemoveSnapshot(string snapshotName);

		// Token: 0x060001EA RID: 490
		void RevertToStoppedState();

		// Token: 0x060001EB RID: 491
		void TrackCreatedFile(string diffDisk);

		// Token: 0x060001EC RID: 492
		void WriteSettingsToVhd(WindowsImageVhd windowsImage);
	}
}
