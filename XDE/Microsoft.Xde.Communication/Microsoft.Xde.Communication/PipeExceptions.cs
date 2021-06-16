using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000005 RID: 5
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class PipeExceptions
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000024F8 File Offset: 0x000006F8
		internal PipeExceptions()
		{
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002500 File Offset: 0x00000700
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (PipeExceptions.resourceMan == null)
				{
					PipeExceptions.resourceMan = new ResourceManager("Microsoft.Xde.Communication.PipeExceptions", typeof(PipeExceptions).Assembly);
				}
				return PipeExceptions.resourceMan;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000252C File Offset: 0x0000072C
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002533 File Offset: 0x00000733
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return PipeExceptions.resourceCulture;
			}
			set
			{
				PipeExceptions.resourceCulture = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000253B File Offset: 0x0000073B
		internal static string CaptureScreenFailedOnGuestSystem
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("CaptureScreenFailedOnGuestSystem", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002551 File Offset: 0x00000751
		internal static string ConnectionClosedByTheDevice
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("ConnectionClosedByTheDevice", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002567 File Offset: 0x00000767
		internal static string DnsServerSetupFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("DnsServerSetupFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000257D File Offset: 0x0000077D
		internal static string FailedToDetectGuest
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("FailedToDetectGuest", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002593 File Offset: 0x00000793
		internal static string FailedToSetupUDPPort
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("FailedToSetupUDPPort", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000025A9 File Offset: 0x000007A9
		internal static string GetAdapterAddressesFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("GetAdapterAddressesFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025BF File Offset: 0x000007BF
		internal static string GetSensorsEnabledStatesFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("GetSensorsEnabledStatesFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000025D5 File Offset: 0x000007D5
		internal static string HttpProxySetupFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("HttpProxySetupFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025EB File Offset: 0x000007EB
		internal static string IpRenewalFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("IpRenewalFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002601 File Offset: 0x00000801
		internal static string NotificationDisableSimulationError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("NotificationDisableSimulationError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002617 File Offset: 0x00000817
		internal static string NotificationEnableSimulationError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("NotificationEnableSimulationError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000262D File Offset: 0x0000082D
		internal static string NotificationSendPayloadError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("NotificationSendPayloadError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002643 File Offset: 0x00000843
		internal static string NotificationUriRetrievalError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("NotificationUriRetrievalError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002659 File Offset: 0x00000859
		internal static string PipeMessageFormat
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("PipeMessageFormat", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000266F File Offset: 0x0000086F
		internal static string PipeNotConnected
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("PipeNotConnected", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002685 File Offset: 0x00000885
		internal static string PipeShutdown
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("PipeShutdown", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000269B File Offset: 0x0000089B
		internal static string SDCardGuestCloseFileHandleError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestCloseFileHandleError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000026B1 File Offset: 0x000008B1
		internal static string SDCardGuestCreateDirectoryError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestCreateDirectoryError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000026C7 File Offset: 0x000008C7
		internal static string SDCardGuestCreateOrOpenFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestCreateOrOpenFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000026DD File Offset: 0x000008DD
		internal static string SDCardGuestDeleteFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestDeleteFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000026F3 File Offset: 0x000008F3
		internal static string SDCardGuestEjectSDError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestEjectSDError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002709 File Offset: 0x00000909
		internal static string SDCardGuestFindFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestFindFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000271F File Offset: 0x0000091F
		internal static string SDCardGuestFindNextFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestFindNextFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002735 File Offset: 0x00000935
		internal static string SDCardGuestGetFileAttribError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestGetFileAttribError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000274B File Offset: 0x0000094B
		internal static string SDCardGuestGetMountRootError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestGetMountRootError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002761 File Offset: 0x00000961
		internal static string SDCardGuestGetMountStatusError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestGetMountStatusError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002777 File Offset: 0x00000977
		internal static string SDCardGuestInsertSDError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestInsertSDError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000278D File Offset: 0x0000098D
		internal static string SDCardGuestReadFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestReadFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000027A3 File Offset: 0x000009A3
		internal static string SDCardGuestSetFileAttribError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestSetFileAttribError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000027B9 File Offset: 0x000009B9
		internal static string SDCardGuestSetFileTimeError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestSetFileTimeError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000027CF File Offset: 0x000009CF
		internal static string SDCardGuestWriteFileError
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardGuestWriteFileError", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000027E5 File Offset: 0x000009E5
		internal static string SDCardHostFileMaxSizeExceeded
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardHostFileMaxSizeExceeded", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000027FB File Offset: 0x000009FB
		internal static string SDCardLabelNotSet
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardLabelNotSet", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002811 File Offset: 0x00000A11
		internal static string SDCardLocalFolderPathTooLong
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardLocalFolderPathTooLong", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002827 File Offset: 0x00000A27
		internal static string SDCardProtocolBreached
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardProtocolBreached", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000283D File Offset: 0x00000A3D
		internal static string SDCardRootNotSet
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SDCardRootNotSet", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002853 File Offset: 0x00000A53
		internal static string SendHostIDFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SendHostIDFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002869 File Offset: 0x00000A69
		internal static string ServerReturnedIncorrectAck
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("ServerReturnedIncorrectAck", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000287F File Offset: 0x00000A7F
		internal static string SetEnabledMonitorsFormat
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SetEnabledMonitorsFormat", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002895 File Offset: 0x00000A95
		internal static string SetGuestNATSubnetFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SetGuestNATSubnetFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000028AB File Offset: 0x00000AAB
		internal static string SetNetworkThrottlingParamsFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SetNetworkThrottlingParamsFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000028C1 File Offset: 0x00000AC1
		internal static string SetSensorsEnabledStatesFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SetSensorsEnabledStatesFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000028D7 File Offset: 0x00000AD7
		internal static string SetSystemTimeAndZoneFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SetSystemTimeAndZoneFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000028ED File Offset: 0x00000AED
		internal static string SocksProxySetupFailed
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("SocksProxySetupFailed", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002903 File Offset: 0x00000B03
		internal static string VersionMismatchFailure
		{
			get
			{
				return PipeExceptions.ResourceManager.GetString("VersionMismatchFailure", PipeExceptions.resourceCulture);
			}
		}

		// Token: 0x04000009 RID: 9
		private static ResourceManager resourceMan;

		// Token: 0x0400000A RID: 10
		private static CultureInfo resourceCulture;
	}
}
