using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Win32
{
	// Token: 0x02000082 RID: 130
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x040001A8 RID: 424
		private const string EventingProviderApiSet = "advapi32.dll";

		// Token: 0x040001A9 RID: 425
		private const string EventingControllerApiSet = "advapi32.dll";

		// Token: 0x02000083 RID: 131
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		internal static class ManifestEtw
		{
			// Token: 0x06000336 RID: 822
			[SecurityCritical]
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			internal unsafe static extern uint EventRegister([In] ref Guid providerId, [In] UnsafeNativeMethods.ManifestEtw.EtwEnableCallback enableCallback, [In] void* callbackContext, [In] [Out] ref long registrationHandle);

			// Token: 0x06000337 RID: 823
			[SecurityCritical]
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			internal static extern uint EventUnregister([In] long registrationHandle);

			// Token: 0x06000338 RID: 824 RVA: 0x0001024C File Offset: 0x0000E44C
			internal unsafe static int EventWriteTransferWrapper(long registrationHandle, ref EventDescriptor eventDescriptor, Guid* activityId, Guid* relatedActivityId, int userDataCount, EventProvider.EventData* userData)
			{
				int num = UnsafeNativeMethods.ManifestEtw.EventWriteTransfer(registrationHandle, ref eventDescriptor, activityId, relatedActivityId, userDataCount, userData);
				if (num == 87 && relatedActivityId == null)
				{
					Guid empty = Guid.Empty;
					num = UnsafeNativeMethods.ManifestEtw.EventWriteTransfer(registrationHandle, ref eventDescriptor, activityId, &empty, userDataCount, userData);
				}
				return num;
			}

			// Token: 0x06000339 RID: 825
			[SuppressUnmanagedCodeSecurity]
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			private unsafe static extern int EventWriteTransfer([In] long registrationHandle, [In] ref EventDescriptor eventDescriptor, [In] Guid* activityId, [In] Guid* relatedActivityId, [In] int userDataCount, [In] EventProvider.EventData* userData);

			// Token: 0x0600033A RID: 826
			[SuppressUnmanagedCodeSecurity]
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			internal static extern int EventActivityIdControl([In] UnsafeNativeMethods.ManifestEtw.ActivityControl ControlCode, [In] [Out] ref Guid ActivityId);

			// Token: 0x0600033B RID: 827
			[SuppressUnmanagedCodeSecurity]
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			internal unsafe static extern int EventSetInformation([In] long registrationHandle, [In] UnsafeNativeMethods.ManifestEtw.EVENT_INFO_CLASS informationClass, [In] void* eventInformation, [In] int informationLength);

			// Token: 0x040001AA RID: 426
			internal const int ERROR_ARITHMETIC_OVERFLOW = 534;

			// Token: 0x040001AB RID: 427
			internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

			// Token: 0x040001AC RID: 428
			internal const int ERROR_MORE_DATA = 234;

			// Token: 0x040001AD RID: 429
			internal const int ERROR_NOT_SUPPORTED = 50;

			// Token: 0x040001AE RID: 430
			internal const int ERROR_INVALID_PARAMETER = 87;

			// Token: 0x040001AF RID: 431
			internal const int EVENT_CONTROL_CODE_DISABLE_PROVIDER = 0;

			// Token: 0x040001B0 RID: 432
			internal const int EVENT_CONTROL_CODE_ENABLE_PROVIDER = 1;

			// Token: 0x040001B1 RID: 433
			internal const int EVENT_CONTROL_CODE_CAPTURE_STATE = 2;

			// Token: 0x02000084 RID: 132
			// (Invoke) Token: 0x0600033D RID: 829
			[SecurityCritical]
			internal unsafe delegate void EtwEnableCallback([In] ref Guid sourceId, [In] int isEnabled, [In] byte level, [In] long matchAnyKeywords, [In] long matchAllKeywords, [In] UnsafeNativeMethods.ManifestEtw.EVENT_FILTER_DESCRIPTOR* filterData, [In] void* callbackContext);

			// Token: 0x02000085 RID: 133
			internal struct EVENT_FILTER_DESCRIPTOR
			{
				// Token: 0x040001B2 RID: 434
				public long Ptr;

				// Token: 0x040001B3 RID: 435
				public int Size;

				// Token: 0x040001B4 RID: 436
				public int Type;
			}

			// Token: 0x02000086 RID: 134
			internal enum ActivityControl : uint
			{
				// Token: 0x040001B6 RID: 438
				EVENT_ACTIVITY_CTRL_GET_ID = 1U,
				// Token: 0x040001B7 RID: 439
				EVENT_ACTIVITY_CTRL_SET_ID,
				// Token: 0x040001B8 RID: 440
				EVENT_ACTIVITY_CTRL_CREATE_ID,
				// Token: 0x040001B9 RID: 441
				EVENT_ACTIVITY_CTRL_GET_SET_ID,
				// Token: 0x040001BA RID: 442
				EVENT_ACTIVITY_CTRL_CREATE_SET_ID
			}

			// Token: 0x02000087 RID: 135
			internal enum EVENT_INFO_CLASS
			{
				// Token: 0x040001BC RID: 444
				BinaryTrackInfo,
				// Token: 0x040001BD RID: 445
				SetEnableAllKeywords,
				// Token: 0x040001BE RID: 446
				SetTraits
			}
		}
	}
}
