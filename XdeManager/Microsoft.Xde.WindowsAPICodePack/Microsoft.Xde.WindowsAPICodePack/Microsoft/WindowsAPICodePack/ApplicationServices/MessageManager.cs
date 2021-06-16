using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000043 RID: 67
	internal static class MessageManager
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000612C File Offset: 0x0000432C
		internal static void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
		{
			MessageManager.EnsureInitialized();
			MessageManager.window.RegisterPowerEvent(eventId, eventToRegister);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000613F File Offset: 0x0000433F
		internal static void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
		{
			MessageManager.EnsureInitialized();
			MessageManager.window.UnregisterPowerEvent(eventId, eventToUnregister);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00006154 File Offset: 0x00004354
		private static void EnsureInitialized()
		{
			object obj = MessageManager.lockObject;
			lock (obj)
			{
				if (MessageManager.window == null)
				{
					MessageManager.window = new MessageManager.PowerRegWindow();
				}
			}
		}

		// Token: 0x040001C6 RID: 454
		private static object lockObject = new object();

		// Token: 0x040001C7 RID: 455
		private static MessageManager.PowerRegWindow window;

		// Token: 0x02000067 RID: 103
		internal class PowerRegWindow : Form
		{
			// Token: 0x0600029F RID: 671 RVA: 0x00006D5E File Offset: 0x00004F5E
			internal PowerRegWindow()
			{
			}

			// Token: 0x060002A0 RID: 672 RVA: 0x00006D7C File Offset: 0x00004F7C
			internal void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
			{
				this.readerWriterLock.AcquireWriterLock(-1);
				if (!this.eventList.Contains(eventId))
				{
					Power.RegisterPowerSettingNotification(base.Handle, eventId);
					ArrayList arrayList = new ArrayList();
					arrayList.Add(eventToRegister);
					this.eventList.Add(eventId, arrayList);
				}
				else
				{
					((ArrayList)this.eventList[eventId]).Add(eventToRegister);
				}
				this.readerWriterLock.ReleaseWriterLock();
			}

			// Token: 0x060002A1 RID: 673 RVA: 0x00006E00 File Offset: 0x00005000
			internal void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
			{
				this.readerWriterLock.AcquireWriterLock(-1);
				if (this.eventList.Contains(eventId))
				{
					((ArrayList)this.eventList[eventId]).Remove(eventToUnregister);
					this.readerWriterLock.ReleaseWriterLock();
					return;
				}
				throw new InvalidOperationException(LocalizedMessages.MessageManagerHandlerNotRegistered);
			}

			// Token: 0x060002A2 RID: 674 RVA: 0x00006E60 File Offset: 0x00005060
			private static void ExecuteEvents(ArrayList eventHandlerList)
			{
				foreach (object obj in eventHandlerList)
				{
					((EventHandler)obj)(null, new EventArgs());
				}
			}

			// Token: 0x060002A3 RID: 675 RVA: 0x00006EB8 File Offset: 0x000050B8
			protected override void WndProc(ref Message m)
			{
				if ((long)m.Msg == 536L && (long)((int)m.WParam) == 32787L)
				{
					PowerManagementNativeMethods.PowerBroadcastSetting powerBroadcastSetting = (PowerManagementNativeMethods.PowerBroadcastSetting)Marshal.PtrToStructure(m.LParam, typeof(PowerManagementNativeMethods.PowerBroadcastSetting));
					IntPtr ptr = new IntPtr(m.LParam.ToInt64() + (long)Marshal.SizeOf<PowerManagementNativeMethods.PowerBroadcastSetting>(powerBroadcastSetting));
					Guid powerSetting = powerBroadcastSetting.PowerSetting;
					if (powerBroadcastSetting.PowerSetting == EventManager.MonitorPowerStatus && powerBroadcastSetting.DataLength == Marshal.SizeOf(typeof(int)))
					{
						PowerManager.IsMonitorOn = ((int)Marshal.PtrToStructure(ptr, typeof(int)) != 0);
						EventManager.monitorOnReset.Set();
					}
					if (!EventManager.IsMessageCaught(powerSetting))
					{
						MessageManager.PowerRegWindow.ExecuteEvents((ArrayList)this.eventList[powerSetting]);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
				}
			}

			// Token: 0x040002D5 RID: 725
			private Hashtable eventList = new Hashtable();

			// Token: 0x040002D6 RID: 726
			private ReaderWriterLock readerWriterLock = new ReaderWriterLock();
		}
	}
}
