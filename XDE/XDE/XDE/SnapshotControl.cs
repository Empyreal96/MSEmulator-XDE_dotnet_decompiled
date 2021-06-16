using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Xde.Common;
using Microsoft.Xde.Interface;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200002A RID: 42
	public class SnapshotControl : IXdeSnapshotControl, INotifyPropertyChanged
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000ABCD File Offset: 0x00008DCD
		public SnapshotControl(XdeController xdeController)
		{
			this.xdeController = xdeController;
			this.xdeController.ShellReady += this.XdeController_ShellReady;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002C7 RID: 711 RVA: 0x0000ABF4 File Offset: 0x00008DF4
		// (remove) Token: 0x060002C8 RID: 712 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000AC61 File Offset: 0x00008E61
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000AC69 File Offset: 0x00008E69
		public bool CanDoOperation
		{
			get
			{
				return this.canDoOperation;
			}
			private set
			{
				if (value != this.canDoOperation)
				{
					this.canDoOperation = value;
					this.OnPropertyChanged("CanDoOperation");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000AC86 File Offset: 0x00008E86
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000AC98 File Offset: 0x00008E98
		public string DefaultSnapshot
		{
			get
			{
				return this.xdeController.VmUserSettings.DefaultSnapshot;
			}
			set
			{
				if (value != this.xdeController.VmUserSettings.DefaultSnapshot)
				{
					this.xdeController.VmUserSettings.DefaultSnapshot = value;
					this.OnPropertyChanged("DefaultSnapshot");
				}
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000ACCE File Offset: 0x00008ECE
		public void Initialize(IXdeVirtualMachine virtualMachine)
		{
			this.virtualMachine = virtualMachine;
			this.virtualMachine.SnapshotsChanged += this.VirtualMachine_SnapshotsChanged;
			this.virtualMachine.EnableStateChanged += this.VirtualMachine_EnableStateChanged;
			this.LoadDefaultSnapshotId();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000AD0C File Offset: 0x00008F0C
		public void DeleteSnapshot(string uniqueId)
		{
			if (!this.CanDoOperation)
			{
				return;
			}
			IXdeVirtualMachineSettings snapshot = this.GetSnapshot(uniqueId);
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				this.CanDoOperation = false;
				try
				{
					snapshot.Delete();
				}
				catch (Exception e)
				{
					this.xdeController.ShowErrorMessageForException(Resources.FailedToDeleteSnapshot, e, "SnapshotDeleted");
				}
				this.CanDoOperation = true;
			});
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000AD50 File Offset: 0x00008F50
		public void ResumeSnapshot(string uniqueId)
		{
			if (!this.CanDoOperation)
			{
				return;
			}
			IXdeVirtualMachineSettings snapshot = this.GetSnapshot(uniqueId);
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				try
				{
					DateTime now = DateTime.Now;
					this.xdeController.InternalResumeSnapshot(snapshot);
					uint timeTakenMilliseconds = (uint)(DateTime.Now - now).TotalMilliseconds;
					Logger.Instance.LogTimeTaken("SnapshotResumed", timeTakenMilliseconds);
				}
				catch (Exception e)
				{
					this.xdeController.ShowErrorMessageForException(Resources.FailedToResumeSnapshot, e, "SnapshotResumed");
					this.xdeController.ShutdownXde();
				}
			});
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000AD94 File Offset: 0x00008F94
		public void TakeNewSnapshot(string snapshotName)
		{
			if (!this.CanDoOperation)
			{
				return;
			}
			if (snapshotName == null)
			{
				snapshotName = this.GetNextFreeSnapshotName();
			}
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				this.CanDoOperation = false;
				try
				{
					XdeSensors enabledStates = ((IXdeAutomation)this.xdeController).SensorsConfig.EnabledStates;
					DateTime now = DateTime.Now;
					this.virtualMachine.CreateSnapshot(snapshotName);
					uint timeTakenMilliseconds = (uint)(DateTime.Now - now).TotalMilliseconds;
					Logger.Instance.LogTimeTaken("NewSnapshotTaken", timeTakenMilliseconds);
				}
				catch (Exception e)
				{
					this.xdeController.ShowErrorMessageForException(Resources.FailedToTakeSnapshot, e, "NewSnapshotTaken");
				}
				this.CanDoOperation = true;
			});
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		private void XdeController_ShellReady(object sender, EventArgs e)
		{
			this.CanDoOperation = true;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000ADED File Offset: 0x00008FED
		private void VirtualMachine_EnableStateChanged(object sender, EnabledStateChangedEventArgs e)
		{
			if (e.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				this.CanDoOperation = false;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000AE00 File Offset: 0x00009000
		private void LoadDefaultSnapshotId()
		{
			VmUserSettings vmUserSettings = this.xdeController.VmUserSettings;
			IXdeVirtualMachineSettings xdeVirtualMachineSettings = this.virtualMachine.FindSnapshotSettings(vmUserSettings.DefaultSnapshot);
			if (xdeVirtualMachineSettings == null)
			{
				xdeVirtualMachineSettings = this.virtualMachine.FindSnapshotSettings("Default Emulator Checkpoint");
				if (xdeVirtualMachineSettings == null)
				{
					xdeVirtualMachineSettings = this.virtualMachine.SnapshotSettings.FirstOrDefault<IXdeVirtualMachineSettings>();
				}
			}
			this.DefaultSnapshot = ((xdeVirtualMachineSettings != null) ? xdeVirtualMachineSettings.UniqueId : null);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000AE65 File Offset: 0x00009065
		private void VirtualMachine_SnapshotsChanged(object sender, EventArgs e)
		{
			string defaultSnapshot = this.DefaultSnapshot;
			this.LoadDefaultSnapshotId();
			if (defaultSnapshot != this.DefaultSnapshot)
			{
				this.OnPropertyChanged("DefaultSnapshot");
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000AE8C File Offset: 0x0000908C
		private IXdeVirtualMachineSettings GetSnapshot(string uniqueId)
		{
			IXdeVirtualMachineSettings xdeVirtualMachineSettings = this.virtualMachine.SnapshotSettings.FirstOrDefault((IXdeVirtualMachineSettings s) => s.UniqueId == uniqueId);
			if (xdeVirtualMachineSettings == null)
			{
				throw new ArgumentException(StringUtilities.CurrentCultureFormat(Resources.SnapshotNameNotFound, new object[]
				{
					uniqueId
				}));
			}
			return xdeVirtualMachineSettings;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000AEE8 File Offset: 0x000090E8
		private string GetNextFreeSnapshotName()
		{
			HashSet<string> snapshotNames = this.GetSnapshotNames();
			for (int i = 1; i < 50; i++)
			{
				string text = StringUtilities.CurrentCultureFormat(Resources.NewCheckpointFormat, new object[]
				{
					i
				});
				if (!snapshotNames.Contains(text))
				{
					return text;
				}
			}
			return "Default Emulator Checkpoint";
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000AF34 File Offset: 0x00009134
		private HashSet<string> GetSnapshotNames()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.CurrentCulture);
			foreach (IXdeVirtualMachineSettings xdeVirtualMachineSettings in this.virtualMachine.SnapshotSettings)
			{
				hashSet.Add(xdeVirtualMachineSettings.Name);
			}
			return hashSet;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000AF98 File Offset: 0x00009198
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040000FB RID: 251
		private XdeController xdeController;

		// Token: 0x040000FC RID: 252
		private bool canDoOperation;

		// Token: 0x040000FD RID: 253
		private IXdeVirtualMachine virtualMachine;
	}
}
