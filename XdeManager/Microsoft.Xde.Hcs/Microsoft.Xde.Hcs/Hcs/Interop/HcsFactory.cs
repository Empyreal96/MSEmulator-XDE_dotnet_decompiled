using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HCS.Schema.Responses.System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x0200000D RID: 13
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public static class HcsFactory
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00003FB6 File Offset: 0x000021B6
		public static IHcs GetHcs()
		{
			return new HcsFactory.Hcs();
		}

		// Token: 0x02000024 RID: 36
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class Hcs : IHcs
		{
			// Token: 0x06000112 RID: 274 RVA: 0x00004BA0 File Offset: 0x00002DA0
			string IHcs.EnumerateComputeSystems(string query)
			{
				string result;
				string result2;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsEnumerateComputeSystems(query, out result, out result2), result2);
				return result;
			}

			// Token: 0x06000113 RID: 275 RVA: 0x00004BC0 File Offset: 0x00002DC0
			async Task<SafeHcsSystemHandle> IHcs.CreateComputeSystemAsync(string id, string configuration, SafeAccessTokenHandle identity)
			{
				SafeHcsSystemHandle computeSystem;
				string result;
				bool flag = HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsCreateComputeSystem(id, configuration, identity ?? new SafeAccessTokenHandle(IntPtr.Zero), out computeSystem, out result), result);
				computeSystem.Id = id;
				computeSystem.Watcher = new HcsNotificationWatcher<SafeHcsSystemHandle>(computeSystem, new RegisterHcsNotificationCallback<SafeHcsSystemHandle>(this.RegisterComputeSystemCallback), new UnregisterHcsNotificationCallback(this.UnregisterComputeSystemCallback), HcsFactory.Hcs.OpenComputeCreateSystemNotifications);
				if (flag)
				{
					await computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemCreateCompleted).ConfigureAwait(false);
				}
				return computeSystem;
			}

			// Token: 0x06000114 RID: 276 RVA: 0x00004C20 File Offset: 0x00002E20
			SafeHcsSystemHandle IHcs.OpenComputeSystem(string id)
			{
				SafeHcsSystemHandle safeHcsSystemHandle;
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsOpenComputeSystem(id, out safeHcsSystemHandle, out result), result);
				safeHcsSystemHandle.Watcher = new HcsNotificationWatcher<SafeHcsSystemHandle>(safeHcsSystemHandle, new RegisterHcsNotificationCallback<SafeHcsSystemHandle>(this.RegisterComputeSystemCallback), new UnregisterHcsNotificationCallback(this.UnregisterComputeSystemCallback), HcsFactory.Hcs.OpenComputeCreateSystemNotifications);
				safeHcsSystemHandle.Id = id;
				return safeHcsSystemHandle;
			}

			// Token: 0x06000115 RID: 277 RVA: 0x00004C6F File Offset: 0x00002E6F
			bool IHcs.CloseComputeSystem(IntPtr computeSystem)
			{
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsCloseComputeSystem(computeSystem), null);
			}

			// Token: 0x06000116 RID: 278 RVA: 0x00004C80 File Offset: 0x00002E80
			async Task IHcs.StartComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				Task<NotificationResult> task = computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemStartCompleted);
				string result;
				if (HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsStartComputeSystem(computeSystem, options, out result), result))
				{
					await task;
				}
			}

			// Token: 0x06000117 RID: 279 RVA: 0x00004CD0 File Offset: 0x00002ED0
			async Task IHcs.ShutdownComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsShutdownComputeSystem(computeSystem, options, out result), result);
				await this.WaitOnComputeSystemExitAsync(computeSystem);
			}

			// Token: 0x06000118 RID: 280 RVA: 0x00004D28 File Offset: 0x00002F28
			async Task IHcs.TerminateComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsTerminateComputeSystem(computeSystem, options, out result), result);
				await this.WaitOnComputeSystemExitAsync(computeSystem);
			}

			// Token: 0x06000119 RID: 281 RVA: 0x00004D7D File Offset: 0x00002F7D
			public Task WaitOnComputeSystemExitAsync(SafeHcsSystemHandle computeSystem)
			{
				return computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemExited);
			}

			// Token: 0x0600011A RID: 282 RVA: 0x00004D8C File Offset: 0x00002F8C
			async Task IHcs.PauseComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				Task<NotificationResult> task = computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemPauseCompleted);
				string result;
				if (HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsPauseComputeSystem(computeSystem, options, out result), result))
				{
					await task;
				}
			}

			// Token: 0x0600011B RID: 283 RVA: 0x00004DDC File Offset: 0x00002FDC
			async Task IHcs.ResumeComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				Task<NotificationResult> task = computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemResumeCompleted);
				string result;
				if (HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsResumeComputeSystem(computeSystem, options, out result), result))
				{
					await task;
				}
			}

			// Token: 0x0600011C RID: 284 RVA: 0x00004E2C File Offset: 0x0000302C
			async Task IHcs.SaveComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options)
			{
				Task<NotificationResult> task = computeSystem.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationSystemSaveCompleted);
				string result;
				if (HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsSaveComputeSystem(computeSystem, options, out result), result))
				{
					await task;
				}
			}

			// Token: 0x0600011D RID: 285 RVA: 0x00004E7C File Offset: 0x0000307C
			string IHcs.GetComputeSystemProperties(SafeHcsSystemHandle computeSystem, string propertyQuery)
			{
				string result;
				string result2;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsGetComputeSystemProperties(computeSystem, propertyQuery, out result, out result2), result2);
				return result;
			}

			// Token: 0x0600011E RID: 286 RVA: 0x00004E9C File Offset: 0x0000309C
			bool IHcs.ModifyComputeSystem(SafeHcsSystemHandle computeSystem, string configuration)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsModifyComputeSystem(computeSystem, configuration, out result), result);
			}

			// Token: 0x0600011F RID: 287 RVA: 0x00004EB8 File Offset: 0x000030B8
			bool IHcs.AddComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsAddComputeSystemResource(computeSystem, location, resource, out result), result);
			}

			// Token: 0x06000120 RID: 288 RVA: 0x00004ED8 File Offset: 0x000030D8
			bool IHcs.ModifyComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsModifyComputeSystemResource(computeSystem, location, resource, out result), result);
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00004EF8 File Offset: 0x000030F8
			bool IHcs.RemoveComputeSystemResource(SafeHcsSystemHandle computeSystem, string location)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsRemoveComputeSystemResource(computeSystem, location, out result), result);
			}

			// Token: 0x06000122 RID: 290 RVA: 0x00004F14 File Offset: 0x00003114
			SafeHcsProcessHandle IHcs.CreateProcess(SafeHcsSystemHandle computeSystem, string processParameters)
			{
				HCS_PROCESS_INFORMATION info;
				SafeHcsProcessHandle safeHcsProcessHandle;
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsCreateProcess(computeSystem, processParameters, out info, out safeHcsProcessHandle, out result), result);
				safeHcsProcessHandle.Watcher = new HcsNotificationWatcher<SafeHcsProcessHandle>(safeHcsProcessHandle, new RegisterHcsNotificationCallback<SafeHcsProcessHandle>(this.RegisterProcessCallback), new UnregisterHcsNotificationCallback(this.UnregisterProcessCallback), HcsFactory.Hcs.ProcessExitedNotifications);
				safeHcsProcessHandle.System = computeSystem;
				safeHcsProcessHandle.Info = info;
				this.SetupStdioStreams(safeHcsProcessHandle);
				return safeHcsProcessHandle;
			}

			// Token: 0x06000123 RID: 291 RVA: 0x00004F74 File Offset: 0x00003174
			SafeHcsProcessHandle IHcs.OpenProcess(SafeHcsSystemHandle computeSystem, uint processId)
			{
				SafeHcsProcessHandle safeHcsProcessHandle;
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsOpenProcess(computeSystem, processId, out safeHcsProcessHandle, out result), result);
				safeHcsProcessHandle.Watcher = new HcsNotificationWatcher<SafeHcsProcessHandle>(safeHcsProcessHandle, new RegisterHcsNotificationCallback<SafeHcsProcessHandle>(this.RegisterProcessCallback), new UnregisterHcsNotificationCallback(this.UnregisterProcessCallback), HcsFactory.Hcs.ProcessExitedNotifications);
				safeHcsProcessHandle.System = computeSystem;
				safeHcsProcessHandle.Info = this.GetProcessInfo(safeHcsProcessHandle);
				this.SetupStdioStreams(safeHcsProcessHandle);
				return safeHcsProcessHandle;
			}

			// Token: 0x06000124 RID: 292 RVA: 0x00004FD8 File Offset: 0x000031D8
			bool IHcs.CloseProcess(IntPtr process)
			{
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsCloseProcess(process), null);
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00004FE8 File Offset: 0x000031E8
			async Task<ProcessStatus> IHcs.TerminateProcessAsync(SafeHcsProcessHandle process)
			{
				string result;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsTerminateProcess(process, out result), result);
				return await this.WaitOnProcessExitAsync(process);
			}

			// Token: 0x06000126 RID: 294 RVA: 0x00005038 File Offset: 0x00003238
			bool IHcs.SignalProcess(SafeHcsProcessHandle process, string options)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsSignalProcess(process, options, out result), result);
			}

			// Token: 0x06000127 RID: 295 RVA: 0x00005054 File Offset: 0x00003254
			public async Task<ProcessStatus> WaitOnProcessExitAsync(SafeHcsProcessHandle process)
			{
				return JsonHelper.FromJson<ProcessStatus>((await process.Watcher.WatchAsync(HCS_NOTIFICATIONS.HcsNotificationProcessExited)).Data);
			}

			// Token: 0x06000128 RID: 296 RVA: 0x0000509C File Offset: 0x0000329C
			string IHcs.GetProcessProperties(SafeHcsProcessHandle process)
			{
				string result;
				string result2;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsGetProcessProperties(process, out result, out result2), result2);
				return result;
			}

			// Token: 0x06000129 RID: 297 RVA: 0x000050BC File Offset: 0x000032BC
			bool IHcs.ModifyProcess(SafeHcsProcessHandle process, string settings)
			{
				string result;
				return HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsModifyProcess(process, settings, out result), result);
			}

			// Token: 0x0600012A RID: 298 RVA: 0x000050D8 File Offset: 0x000032D8
			private static bool ProcessHcsCall(int resultCode, string result)
			{
				if (resultCode == -1070137085)
				{
					return true;
				}
				if (HcsException.Failed(resultCode))
				{
					throw new HcsException(resultCode, result);
				}
				return false;
			}

			// Token: 0x0600012B RID: 299 RVA: 0x000050F8 File Offset: 0x000032F8
			private void SetupStdioStreams(SafeHcsProcessHandle process)
			{
				process.StdInStream = this.SetupPipeStream(PipeDirection.Out, process.Info.StdInput);
				process.StdOutStream = this.SetupPipeStream(PipeDirection.In, process.Info.StdOutput);
				process.StdErrStream = this.SetupPipeStream(PipeDirection.In, process.Info.StdError);
			}

			// Token: 0x0600012C RID: 300 RVA: 0x00005150 File Offset: 0x00003350
			private AnonymousPipeClientStream SetupPipeStream(PipeDirection direction, IntPtr handle)
			{
				SafePipeHandle safePipeHandle = new SafePipeHandle(handle, true);
				if (safePipeHandle.IsInvalid)
				{
					return null;
				}
				return new AnonymousPipeClientStream(direction, safePipeHandle);
			}

			// Token: 0x0600012D RID: 301 RVA: 0x00005176 File Offset: 0x00003376
			private void RegisterComputeSystemCallback(SafeHcsSystemHandle computeSystem, NotificationCallback callback, IntPtr context, out IntPtr callbackHandle)
			{
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsRegisterComputeSystemCallback(computeSystem, callback, context, out callbackHandle), null);
			}

			// Token: 0x0600012E RID: 302 RVA: 0x00005189 File Offset: 0x00003389
			private void UnregisterComputeSystemCallback(IntPtr callbackHandle)
			{
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsUnregisterComputeSystemCallback(callbackHandle), null);
			}

			// Token: 0x0600012F RID: 303 RVA: 0x00005198 File Offset: 0x00003398
			private void RegisterProcessCallback(SafeHcsProcessHandle process, NotificationCallback callback, IntPtr context, out IntPtr callbackHandle)
			{
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsRegisterProcessCallback(process, callback, context, out callbackHandle), null);
			}

			// Token: 0x06000130 RID: 304 RVA: 0x000051AB File Offset: 0x000033AB
			private void UnregisterProcessCallback(IntPtr callbackHandle)
			{
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsUnregisterProcessCallback(callbackHandle), null);
			}

			// Token: 0x06000131 RID: 305 RVA: 0x000051BC File Offset: 0x000033BC
			private HCS_PROCESS_INFORMATION GetProcessInfo(SafeHcsProcessHandle process)
			{
				HCS_PROCESS_INFORMATION result;
				string result2;
				HcsFactory.Hcs.ProcessHcsCall(HcsFactory.Hcs.NativeMethods.HcsGetProcessInfo(process, out result, out result2), result2);
				return result;
			}

			// Token: 0x0400005E RID: 94
			private static readonly HCS_NOTIFICATIONS[] OpenComputeCreateSystemNotifications = new HCS_NOTIFICATIONS[]
			{
				HCS_NOTIFICATIONS.HcsNotificationSystemExited,
				HCS_NOTIFICATIONS.HcsNotificationSystemCreateCompleted,
				HCS_NOTIFICATIONS.HcsNotificationSystemStartCompleted,
				HCS_NOTIFICATIONS.HcsNotificationSystemPauseCompleted,
				HCS_NOTIFICATIONS.HcsNotificationSystemResumeCompleted
			};

			// Token: 0x0400005F RID: 95
			private static readonly HCS_NOTIFICATIONS[] ProcessExitedNotifications = new HCS_NOTIFICATIONS[]
			{
				HCS_NOTIFICATIONS.HcsNotificationProcessExited
			};

			// Token: 0x02000028 RID: 40
			[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
			private static class NativeMethods
			{
				// Token: 0x0600013B RID: 315
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsEnumerateComputeSystems(string query, [MarshalAs(UnmanagedType.LPWStr)] out string computeSystems, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600013C RID: 316
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsCreateComputeSystem(string id, string configuration, SafeAccessTokenHandle identity, out SafeHcsSystemHandle computeSystem, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600013D RID: 317
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsOpenComputeSystem(string id, out SafeHcsSystemHandle computeSystem, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600013E RID: 318
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsCloseComputeSystem(IntPtr computeSystem);

				// Token: 0x0600013F RID: 319
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsStartComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000140 RID: 320
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsShutdownComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000141 RID: 321
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsTerminateComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000142 RID: 322
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsPauseComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000143 RID: 323
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsResumeComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000144 RID: 324
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsSaveComputeSystem(SafeHcsSystemHandle computeSystem, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000145 RID: 325
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsGetComputeSystemProperties(SafeHcsSystemHandle computeSystem, string propertyQuery, [MarshalAs(UnmanagedType.LPWStr)] out string properties, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000146 RID: 326
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsModifyComputeSystem(SafeHcsSystemHandle computeSystem, string configuration, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000147 RID: 327
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsAddComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000148 RID: 328
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsModifyComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000149 RID: 329
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsRemoveComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600014A RID: 330
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsRegisterComputeSystemCallback(SafeHcsSystemHandle computeSystem, NotificationCallback callback, IntPtr context, out IntPtr callbackHandle);

				// Token: 0x0600014B RID: 331
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsUnregisterComputeSystemCallback(IntPtr callbackHandle);

				// Token: 0x0600014C RID: 332
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsCreateProcess(SafeHcsSystemHandle computeSystem, string processParameters, out HCS_PROCESS_INFORMATION processInformation, out SafeHcsProcessHandle process, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600014D RID: 333
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsOpenProcess(SafeHcsSystemHandle computeSystem, uint processId, out SafeHcsProcessHandle process, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x0600014E RID: 334
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsCloseProcess(IntPtr process);

				// Token: 0x0600014F RID: 335
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsTerminateProcess(SafeHcsProcessHandle process, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000150 RID: 336
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsSignalProcess(SafeHcsProcessHandle process, string options, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000151 RID: 337
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsGetProcessInfo(SafeHcsProcessHandle process, out HCS_PROCESS_INFORMATION processInformation, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000152 RID: 338
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsGetProcessProperties(SafeHcsProcessHandle process, [MarshalAs(UnmanagedType.LPWStr)] out string properties, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000153 RID: 339
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsModifyProcess(SafeHcsProcessHandle process, string settings, [MarshalAs(UnmanagedType.LPWStr)] out string result);

				// Token: 0x06000154 RID: 340
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsRegisterProcessCallback(SafeHcsProcessHandle process, NotificationCallback callback, IntPtr context, out IntPtr callbackHandle);

				// Token: 0x06000155 RID: 341
				[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
				public static extern int HcsUnregisterProcessCallback(IntPtr callbackHandle);
			}
		}
	}
}
