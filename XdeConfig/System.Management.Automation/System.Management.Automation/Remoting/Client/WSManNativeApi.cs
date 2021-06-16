using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000396 RID: 918
	internal static class WSManNativeApi
	{
		// Token: 0x06002C70 RID: 11376
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManInitialize(int flags, [In] [Out] ref IntPtr wsManAPIHandle);

		// Token: 0x06002C71 RID: 11377
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManDeinitialize(IntPtr wsManAPIHandle, int flags);

		// Token: 0x06002C72 RID: 11378
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManCreateSession(IntPtr wsManAPIHandle, [MarshalAs(UnmanagedType.LPWStr)] string connection, int flags, IntPtr authenticationCredentials, IntPtr proxyInfo, [In] [Out] ref IntPtr wsManSessionHandle);

		// Token: 0x06002C73 RID: 11379
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManCloseSession(IntPtr wsManSessionHandle, int flags);

		// Token: 0x06002C74 RID: 11380 RVA: 0x000F8744 File Offset: 0x000F6944
		internal static int WSManSetSessionOption(IntPtr wsManSessionHandle, WSManNativeApi.WSManSessionOption option, WSManNativeApi.WSManDataDWord data)
		{
			WSManNativeApi.MarshalledObject marshalledObject = data.Marshal();
			int result;
			using (marshalledObject)
			{
				result = WSManNativeApi.WSManSetSessionOption(wsManSessionHandle, option, marshalledObject.DataPtr);
			}
			return result;
		}

		// Token: 0x06002C75 RID: 11381
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManSetSessionOption(IntPtr wsManSessionHandle, WSManNativeApi.WSManSessionOption option, IntPtr data);

		// Token: 0x06002C76 RID: 11382
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManGetSessionOptionAsDword(IntPtr wsManSessionHandle, WSManNativeApi.WSManSessionOption option, out int value);

		// Token: 0x06002C77 RID: 11383 RVA: 0x000F878C File Offset: 0x000F698C
		internal static string WSManGetSessionOptionAsString(IntPtr wsManAPIHandle, WSManNativeApi.WSManSessionOption option)
		{
			string result = "";
			int num = 0;
			if (122 != WSManNativeApi.WSManGetSessionOptionAsString(wsManAPIHandle, option, 0, null, out num))
			{
				return result;
			}
			int num2 = num * 2;
			byte[] array = new byte[num2];
			int num3;
			if (WSManNativeApi.WSManGetSessionOptionAsString(wsManAPIHandle, option, num2, array, out num3) != 0)
			{
				return result;
			}
			try
			{
				result = Encoding.Unicode.GetString(array, 0, num2);
			}
			catch (ArgumentNullException)
			{
			}
			catch (DecoderFallbackException)
			{
			}
			return result;
		}

		// Token: 0x06002C78 RID: 11384
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		private static extern int WSManGetSessionOptionAsString(IntPtr wsManSessionHandle, WSManNativeApi.WSManSessionOption option, int optionLength, byte[] optionAsString, out int optionLengthUsed);

		// Token: 0x06002C79 RID: 11385 RVA: 0x000F8800 File Offset: 0x000F6A00
		internal static void WSManCreateShellEx(IntPtr wsManSessionHandle, int flags, string resourceUri, string shellId, WSManNativeApi.WSManShellStartupInfo_ManToUn startupInfo, WSManNativeApi.WSManOptionSet optionSet, WSManNativeApi.WSManData_ManToUn openContent, IntPtr asyncCallback, ref IntPtr shellOperationHandle)
		{
			WSManNativeApi.WSManCreateShellExInternal(wsManSessionHandle, flags, resourceUri, shellId, startupInfo, optionSet, openContent, asyncCallback, ref shellOperationHandle);
		}

		// Token: 0x06002C7A RID: 11386
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManCreateShellEx")]
		private static extern void WSManCreateShellExInternal(IntPtr wsManSessionHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string resourceUri, [MarshalAs(UnmanagedType.LPWStr)] string shellId, IntPtr startupInfo, IntPtr optionSet, IntPtr openContent, IntPtr asyncCallback, [In] [Out] ref IntPtr shellOperationHandle);

		// Token: 0x06002C7B RID: 11387
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManConnectShell")]
		internal static extern void WSManConnectShellEx(IntPtr wsManSessionHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string resourceUri, [MarshalAs(UnmanagedType.LPWStr)] string shellId, IntPtr optionSet, IntPtr connectXml, IntPtr asyncCallback, [In] [Out] ref IntPtr shellOperationHandle);

		// Token: 0x06002C7C RID: 11388
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManDisconnectShell")]
		internal static extern void WSManDisconnectShellEx(IntPtr wsManSessionHandle, int flags, IntPtr disconnectInfo, IntPtr asyncCallback);

		// Token: 0x06002C7D RID: 11389
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManReconnectShell")]
		internal static extern void WSManReconnectShellEx(IntPtr wsManSessionHandle, int flags, IntPtr asyncCallback);

		// Token: 0x06002C7E RID: 11390
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManReconnectShellCommand")]
		internal static extern void WSManReconnectShellCommandEx(IntPtr wsManCommandHandle, int flags, IntPtr asyncCallback);

		// Token: 0x06002C7F RID: 11391
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManRunShellCommandEx(IntPtr shellOperationHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string commandId, [MarshalAs(UnmanagedType.LPWStr)] string commandLine, IntPtr commandArgSet, IntPtr optionSet, IntPtr asyncCallback, ref IntPtr commandOperationHandle);

		// Token: 0x06002C80 RID: 11392
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManConnectShellCommand")]
		internal static extern void WSManConnectShellCommandEx(IntPtr shellOperationHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string commandID, IntPtr optionSet, IntPtr connectXml, IntPtr asyncCallback, ref IntPtr commandOperationHandle);

		// Token: 0x06002C81 RID: 11393
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManReceiveShellOutput")]
		internal static extern void WSManReceiveShellOutputEx(IntPtr shellOperationHandle, IntPtr commandOperationHandle, int flags, IntPtr desiredStreamSet, IntPtr asyncCallback, [In] [Out] ref IntPtr receiveOperationHandle);

		// Token: 0x06002C82 RID: 11394 RVA: 0x000F882F File Offset: 0x000F6A2F
		internal static void WSManSendShellInputEx(IntPtr shellOperationHandle, IntPtr commandOperationHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string streamId, WSManNativeApi.WSManData_ManToUn streamData, IntPtr asyncCallback, ref IntPtr sendOperationHandle)
		{
			WSManNativeApi.WSManSendShellInputExInternal(shellOperationHandle, commandOperationHandle, flags, streamId, streamData, false, asyncCallback, ref sendOperationHandle);
		}

		// Token: 0x06002C83 RID: 11395
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManSendShellInput")]
		private static extern void WSManSendShellInputExInternal(IntPtr shellOperationHandle, IntPtr commandOperationHandle, int flags, [MarshalAs(UnmanagedType.LPWStr)] string streamId, IntPtr streamData, bool endOfStream, IntPtr asyncCallback, [In] [Out] ref IntPtr sendOperationHandle);

		// Token: 0x06002C84 RID: 11396
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManCloseShell(IntPtr shellHandle, int flags, IntPtr asyncCallback);

		// Token: 0x06002C85 RID: 11397
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManCloseCommand(IntPtr cmdHandle, int flags, IntPtr asyncCallback);

		// Token: 0x06002C86 RID: 11398
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode, EntryPoint = "WSManSignalShell")]
		internal static extern void WSManSignalShellEx(IntPtr shellOperationHandle, IntPtr cmdOperationHandle, int flags, string code, IntPtr asyncCallback, [In] [Out] ref IntPtr signalOperationHandle);

		// Token: 0x06002C87 RID: 11399
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern void WSManCloseOperation(IntPtr operationHandle, int flags);

		// Token: 0x06002C88 RID: 11400 RVA: 0x000F8848 File Offset: 0x000F6A48
		internal static string WSManGetErrorMessage(IntPtr wsManAPIHandle, int errorCode)
		{
			string name = CultureInfo.CurrentUICulture.Name;
			string result = "";
			int num = 0;
			if (122 != WSManNativeApi.WSManGetErrorMessage(wsManAPIHandle, 0, name, errorCode, 0, null, out num))
			{
				return result;
			}
			int num2 = num * 2;
			byte[] array = new byte[num2];
			int num3;
			if (WSManNativeApi.WSManGetErrorMessage(wsManAPIHandle, 0, name, errorCode, num2, array, out num3) != 0)
			{
				return result;
			}
			try
			{
				result = Encoding.Unicode.GetString(array, 0, num2);
			}
			catch (ArgumentNullException)
			{
			}
			catch (DecoderFallbackException)
			{
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			return result;
		}

		// Token: 0x06002C89 RID: 11401
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManGetErrorMessage(IntPtr wsManAPIHandle, int flags, string languageCode, int errorCode, int messageLength, byte[] message, out int messageLengthUsed);

		// Token: 0x06002C8A RID: 11402
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManPluginGetOperationParameters(IntPtr requestDetails, int flags, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] WSManNativeApi.WSManDataStruct data);

		// Token: 0x06002C8B RID: 11403
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManPluginOperationComplete(IntPtr requestDetails, int flags, int errorCode, [MarshalAs(UnmanagedType.LPWStr)] string extendedInformation);

		// Token: 0x06002C8C RID: 11404
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManPluginReceiveResult(IntPtr requestDetails, int flags, [MarshalAs(UnmanagedType.LPWStr)] string stream, IntPtr streamResult, [MarshalAs(UnmanagedType.LPWStr)] string commandState, int exitCode);

		// Token: 0x06002C8D RID: 11405
		[DllImport("WsmSvc.dll", CharSet = CharSet.Unicode)]
		internal static extern int WSManPluginReportContext(IntPtr requestDetails, int flags, IntPtr context);

		// Token: 0x04001682 RID: 5762
		internal const uint INFINITE = 4294967295U;

		// Token: 0x04001683 RID: 5763
		internal const string PS_CREATION_XML_TAG = "creationXml";

		// Token: 0x04001684 RID: 5764
		internal const string PS_CONNECT_XML_TAG = "connectXml";

		// Token: 0x04001685 RID: 5765
		internal const string PS_CONNECTRESPONSE_XML_TAG = "connectResponseXml";

		// Token: 0x04001686 RID: 5766
		internal const string PS_XML_NAMESPACE = "http://schemas.microsoft.com/powershell";

		// Token: 0x04001687 RID: 5767
		internal const string WSMAN_STREAM_ID_STDOUT = "stdout";

		// Token: 0x04001688 RID: 5768
		internal const string WSMAN_STREAM_ID_PROMPTRESPONSE = "pr";

		// Token: 0x04001689 RID: 5769
		internal const string WSMAN_STREAM_ID_STDIN = "stdin";

		// Token: 0x0400168A RID: 5770
		internal const string ResourceURIPrefix = "http://schemas.microsoft.com/powershell/";

		// Token: 0x0400168B RID: 5771
		internal const string NoProfile = "WINRS_NOPROFILE";

		// Token: 0x0400168C RID: 5772
		internal const string CodePage = "WINRS_CODEPAGE";

		// Token: 0x0400168D RID: 5773
		internal const int WSMAN_FLAG_REQUESTED_API_VERSION_1_1 = 1;

		// Token: 0x0400168E RID: 5774
		internal const int WSMAN_DEFAULT_MAX_ENVELOPE_SIZE_KB_V2 = 150;

		// Token: 0x0400168F RID: 5775
		internal const int WSMAN_DEFAULT_MAX_ENVELOPE_SIZE_KB_V3 = 500;

		// Token: 0x04001690 RID: 5776
		internal const int ERROR_WSMAN_REDIRECT_REQUESTED = -2144108135;

		// Token: 0x04001691 RID: 5777
		internal const int ERROR_WSMAN_INVALID_RESOURCE_URI = -2144108485;

		// Token: 0x04001692 RID: 5778
		internal const int ERROR_WSMAN_INUSE_CANNOT_RECONNECT = -2144108083;

		// Token: 0x04001693 RID: 5779
		internal const int ERROR_WSMAN_SENDDATA_CANNOT_CONNECT = -2144108526;

		// Token: 0x04001694 RID: 5780
		internal const int ERROR_WSMAN_SENDDATA_CANNOT_COMPLETE = -2144108250;

		// Token: 0x04001695 RID: 5781
		internal const int ERROR_WSMAN_ACCESS_DENIED = 5;

		// Token: 0x04001696 RID: 5782
		internal const int ERROR_WSMAN_OUTOF_MEMORY = 14;

		// Token: 0x04001697 RID: 5783
		internal const int ERROR_WSMAN_NETWORKPATH_NOTFOUND = 53;

		// Token: 0x04001698 RID: 5784
		internal const int ERROR_WSMAN_OPERATION_ABORTED = 995;

		// Token: 0x04001699 RID: 5785
		internal const int ERROR_WSMAN_SHUTDOWN_INPROGRESS = 1115;

		// Token: 0x0400169A RID: 5786
		internal const int ERROR_WSMAN_AUTHENTICATION_FAILED = 1311;

		// Token: 0x0400169B RID: 5787
		internal const int ERROR_WSMAN_LOGON_FAILURE = 1326;

		// Token: 0x0400169C RID: 5788
		internal const int ERROR_WSMAN_IMPROPER_RESPONSE = 1722;

		// Token: 0x0400169D RID: 5789
		internal const int ERROR_WSMAN_INCORRECT_PROTOCOLVERSION = -2141974624;

		// Token: 0x0400169E RID: 5790
		internal const int ERROR_WSMAN_URL_NOTAVAILABLE = -2144108269;

		// Token: 0x0400169F RID: 5791
		internal const int ERROR_WSMAN_INVALID_AUTHENTICATION = -2144108274;

		// Token: 0x040016A0 RID: 5792
		internal const int ERROR_WSMAN_CANNOT_CONNECT_INVALID = -2144108080;

		// Token: 0x040016A1 RID: 5793
		internal const int ERROR_WSMAN_CANNOT_CONNECT_MISMATCH = -2144108090;

		// Token: 0x040016A2 RID: 5794
		internal const int ERROR_WSMAN_CANNOT_CONNECT_RUNASFAILED = -2144108065;

		// Token: 0x040016A3 RID: 5795
		internal const int ERROR_WSMAN_CREATEFAILED_INVALIDNAME = -2144108094;

		// Token: 0x040016A4 RID: 5796
		internal const int ERROR_WSMAN_TARGETSESSION_DOESNOTEXIST = -2144108453;

		// Token: 0x040016A5 RID: 5797
		internal const int ERROR_WSMAN_REMOTESESSION_DISALLOWED = -2144108116;

		// Token: 0x040016A6 RID: 5798
		internal const int ERROR_WSMAN_REMOTECONNECTION_DISALLOWED = -2144108061;

		// Token: 0x040016A7 RID: 5799
		internal const int ERROR_WSMAN_INVALID_RESOURCE_URI2 = -2144108542;

		// Token: 0x040016A8 RID: 5800
		internal const int ERROR_WSMAN_CORRUPTED_CONFIG = -2144108539;

		// Token: 0x040016A9 RID: 5801
		internal const int ERROR_WSMAN_URI_LIMIT = -2144108499;

		// Token: 0x040016AA RID: 5802
		internal const int ERROR_WSMAN_CLIENT_KERBEROS_DISABLED = -2144108318;

		// Token: 0x040016AB RID: 5803
		internal const int ERROR_WSMAN_SERVER_NOTTRUSTED = -2144108316;

		// Token: 0x040016AC RID: 5804
		internal const int ERROR_WSMAN_WORKGROUP_NO_KERBEROS = -2144108276;

		// Token: 0x040016AD RID: 5805
		internal const int ERROR_WSMAN_EXPLICIT_CREDENTIALS_REQUIRED = -2144108315;

		// Token: 0x040016AE RID: 5806
		internal const int ERROR_WSMAN_REDIRECT_LOCATION_INVALID = -2144108105;

		// Token: 0x040016AF RID: 5807
		internal const int ERROR_WSMAN_BAD_METHOD = -2144108428;

		// Token: 0x040016B0 RID: 5808
		internal const int ERROR_WSMAN_HTTP_SERVICE_UNAVAILABLE = -2144108270;

		// Token: 0x040016B1 RID: 5809
		internal const int ERROR_WSMAN_HTTP_SERVICE_ERROR = -2144108176;

		// Token: 0x040016B2 RID: 5810
		internal const int ERROR_WSMAN_COMPUTER_NOTFOUND = -2144108103;

		// Token: 0x040016B3 RID: 5811
		internal const int ERROR_WSMAN_TARGET_UNKOWN = -2146893053;

		// Token: 0x040016B4 RID: 5812
		internal const int ERROR_WSMAN_CANNOTUSE_IP = -2144108101;

		// Token: 0x040016B5 RID: 5813
		internal const string WSManApiDll = "WsmSvc.dll";

		// Token: 0x040016B6 RID: 5814
		internal const string WSMAN_SHELL_NAMESPACE = "http://schemas.microsoft.com/wbem/wsman/1/windows/shell";

		// Token: 0x040016B7 RID: 5815
		internal const string WSMAN_COMMAND_STATE_DONE = "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Done";

		// Token: 0x040016B8 RID: 5816
		internal const string WSMAN_COMMAND_STATE_PENDING = "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Pending";

		// Token: 0x040016B9 RID: 5817
		internal const string WSMAN_COMMAND_STATE_RUNNING = "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Running";

		// Token: 0x040016BA RID: 5818
		internal static readonly Version WSMAN_STACK_VERSION = new Version(3, 0);

		// Token: 0x02000397 RID: 919
		internal struct MarshalledObject : IDisposable
		{
			// Token: 0x06002C8F RID: 11407 RVA: 0x000F88EA File Offset: 0x000F6AEA
			internal MarshalledObject(IntPtr dataPtr)
			{
				this.dataPtr = dataPtr;
			}

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000F88F3 File Offset: 0x000F6AF3
			internal IntPtr DataPtr
			{
				get
				{
					return this.dataPtr;
				}
			}

			// Token: 0x06002C91 RID: 11409 RVA: 0x000F88FC File Offset: 0x000F6AFC
			internal static WSManNativeApi.MarshalledObject Create<T>(T obj)
			{
				IntPtr ptr = Marshal.AllocHGlobal(ClrFacade.SizeOf<T>());
				Marshal.StructureToPtr(obj, ptr, false);
				return new WSManNativeApi.MarshalledObject
				{
					dataPtr = ptr
				};
			}

			// Token: 0x06002C92 RID: 11410 RVA: 0x000F8932 File Offset: 0x000F6B32
			public void Dispose()
			{
				if (IntPtr.Zero != this.dataPtr)
				{
					Marshal.FreeHGlobal(this.dataPtr);
					this.dataPtr = IntPtr.Zero;
				}
			}

			// Token: 0x06002C93 RID: 11411 RVA: 0x000F895C File Offset: 0x000F6B5C
			public static implicit operator IntPtr(WSManNativeApi.MarshalledObject obj)
			{
				return obj.dataPtr;
			}

			// Token: 0x040016BB RID: 5819
			private IntPtr dataPtr;
		}

		// Token: 0x02000398 RID: 920
		[Flags]
		internal enum WSManAuthenticationMechanism
		{
			// Token: 0x040016BD RID: 5821
			WSMAN_FLAG_DEFAULT_AUTHENTICATION = 0,
			// Token: 0x040016BE RID: 5822
			WSMAN_FLAG_NO_AUTHENTICATION = 1,
			// Token: 0x040016BF RID: 5823
			WSMAN_FLAG_AUTH_DIGEST = 2,
			// Token: 0x040016C0 RID: 5824
			WSMAN_FLAG_AUTH_NEGOTIATE = 4,
			// Token: 0x040016C1 RID: 5825
			WSMAN_FLAG_AUTH_BASIC = 8,
			// Token: 0x040016C2 RID: 5826
			WSMAN_FLAG_AUTH_KERBEROS = 16,
			// Token: 0x040016C3 RID: 5827
			WSMAN_FLAG_AUTH_CLIENT_CERTIFICATE = 32,
			// Token: 0x040016C4 RID: 5828
			WSMAN_FLAG_AUTH_CREDSSP = 128
		}

		// Token: 0x02000399 RID: 921
		internal abstract class BaseWSManAuthenticationCredentials : IDisposable
		{
			// Token: 0x06002C94 RID: 11412
			public abstract WSManNativeApi.MarshalledObject GetMarshalledObject();

			// Token: 0x06002C95 RID: 11413 RVA: 0x000F8965 File Offset: 0x000F6B65
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002C96 RID: 11414 RVA: 0x000F8974 File Offset: 0x000F6B74
			protected virtual void Dispose(bool isDisposing)
			{
			}
		}

		// Token: 0x0200039A RID: 922
		internal class WSManUserNameAuthenticationCredentials : WSManNativeApi.BaseWSManAuthenticationCredentials
		{
			// Token: 0x06002C98 RID: 11416 RVA: 0x000F897E File Offset: 0x000F6B7E
			internal WSManUserNameAuthenticationCredentials()
			{
				this.cred = default(WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct);
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct>(this.cred);
			}

			// Token: 0x06002C99 RID: 11417 RVA: 0x000F89A4 File Offset: 0x000F6BA4
			internal WSManUserNameAuthenticationCredentials(string name, SecureString pwd, WSManNativeApi.WSManAuthenticationMechanism authMechanism)
			{
				this.cred = default(WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct);
				this.cred.authenticationMechanism = authMechanism;
				this.cred.userName = name;
				if (pwd != null)
				{
					this.cred.password = ClrFacade.SecureStringToCoTaskMemUnicode(pwd);
				}
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct>(this.cred);
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000F8A00 File Offset: 0x000F6C00
			internal WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct CredentialStruct
			{
				get
				{
					return this.cred;
				}
			}

			// Token: 0x06002C9B RID: 11419 RVA: 0x000F8A08 File Offset: 0x000F6C08
			public override WSManNativeApi.MarshalledObject GetMarshalledObject()
			{
				return this.data;
			}

			// Token: 0x06002C9C RID: 11420 RVA: 0x000F8A10 File Offset: 0x000F6C10
			protected override void Dispose(bool isDisposing)
			{
				if (this.cred.password != IntPtr.Zero)
				{
					ClrFacade.ZeroFreeCoTaskMemUnicode(this.cred.password);
					this.cred.password = IntPtr.Zero;
				}
				this.data.Dispose();
			}

			// Token: 0x040016C5 RID: 5829
			private WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct cred;

			// Token: 0x040016C6 RID: 5830
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x0200039B RID: 923
			internal struct WSManUserNameCredentialStruct
			{
				// Token: 0x040016C7 RID: 5831
				internal WSManNativeApi.WSManAuthenticationMechanism authenticationMechanism;

				// Token: 0x040016C8 RID: 5832
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string userName;

				// Token: 0x040016C9 RID: 5833
				internal IntPtr password;
			}
		}

		// Token: 0x0200039C RID: 924
		internal class WSManCertificateThumbprintCredentials : WSManNativeApi.BaseWSManAuthenticationCredentials
		{
			// Token: 0x06002C9D RID: 11421 RVA: 0x000F8A60 File Offset: 0x000F6C60
			internal WSManCertificateThumbprintCredentials(string thumbPrint)
			{
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManCertificateThumbprintCredentials.WSManThumbprintStruct>(new WSManNativeApi.WSManCertificateThumbprintCredentials.WSManThumbprintStruct
				{
					authenticationMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_CLIENT_CERTIFICATE,
					certificateThumbprint = thumbPrint,
					reserved = IntPtr.Zero
				});
			}

			// Token: 0x06002C9E RID: 11422 RVA: 0x000F8AA4 File Offset: 0x000F6CA4
			public override WSManNativeApi.MarshalledObject GetMarshalledObject()
			{
				return this.data;
			}

			// Token: 0x06002C9F RID: 11423 RVA: 0x000F8AAC File Offset: 0x000F6CAC
			protected override void Dispose(bool isDisposing)
			{
				this.data.Dispose();
			}

			// Token: 0x040016CA RID: 5834
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x0200039D RID: 925
			private struct WSManThumbprintStruct
			{
				// Token: 0x040016CB RID: 5835
				internal WSManNativeApi.WSManAuthenticationMechanism authenticationMechanism;

				// Token: 0x040016CC RID: 5836
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string certificateThumbprint;

				// Token: 0x040016CD RID: 5837
				internal IntPtr reserved;
			}
		}

		// Token: 0x0200039E RID: 926
		internal enum WSManSessionOption
		{
			// Token: 0x040016CF RID: 5839
			WSMAN_OPTION_DEFAULT_OPERATION_TIMEOUTMS = 1,
			// Token: 0x040016D0 RID: 5840
			WSMAN_OPTION_MAX_RETRY_TIME = 11,
			// Token: 0x040016D1 RID: 5841
			WSMAN_OPTION_TIMEOUTMS_CREATE_SHELL,
			// Token: 0x040016D2 RID: 5842
			WSMAN_OPTION_TIMEOUTMS_RECEIVE_SHELL_OUTPUT = 14,
			// Token: 0x040016D3 RID: 5843
			WSMAN_OPTION_TIMEOUTMS_SEND_SHELL_INPUT,
			// Token: 0x040016D4 RID: 5844
			WSMAN_OPTION_TIMEOUTMS_SIGNAL_SHELL,
			// Token: 0x040016D5 RID: 5845
			WSMAN_OPTION_TIMEOUTMS_CLOSE_SHELL_OPERATION,
			// Token: 0x040016D6 RID: 5846
			WSMAN_OPTION_SKIP_CA_CHECK,
			// Token: 0x040016D7 RID: 5847
			WSMAN_OPTION_SKIP_CN_CHECK,
			// Token: 0x040016D8 RID: 5848
			WSMAN_OPTION_UNENCRYPTED_MESSAGES,
			// Token: 0x040016D9 RID: 5849
			WSMAN_OPTION_UTF16,
			// Token: 0x040016DA RID: 5850
			WSMAN_OPTION_ENABLE_SPN_SERVER_PORT,
			// Token: 0x040016DB RID: 5851
			WSMAN_OPTION_MACHINE_ID,
			// Token: 0x040016DC RID: 5852
			WSMAN_OPTION_USE_INTERACTIVE_TOKEN = 34,
			// Token: 0x040016DD RID: 5853
			WSMAN_OPTION_LOCALE = 25,
			// Token: 0x040016DE RID: 5854
			WSMAN_OPTION_UI_LANGUAGE,
			// Token: 0x040016DF RID: 5855
			WSMAN_OPTION_MAX_ENVELOPE_SIZE_KB = 28,
			// Token: 0x040016E0 RID: 5856
			WSMAN_OPTION_SHELL_MAX_DATA_SIZE_PER_MESSAGE_KB,
			// Token: 0x040016E1 RID: 5857
			WSMAN_OPTION_REDIRECT_LOCATION,
			// Token: 0x040016E2 RID: 5858
			WSMAN_OPTION_SKIP_REVOCATION_CHECK,
			// Token: 0x040016E3 RID: 5859
			WSMAN_OPTION_ALLOW_NEGOTIATE_IMPLICIT_CREDENTIALS,
			// Token: 0x040016E4 RID: 5860
			WSMAN_OPTION_USE_SSL
		}

		// Token: 0x0200039F RID: 927
		internal enum WSManShellFlag
		{
			// Token: 0x040016E6 RID: 5862
			WSMAN_FLAG_NO_COMPRESSION = 1,
			// Token: 0x040016E7 RID: 5863
			WSMAN_FLAG_SERVER_BUFFERING_MODE_DROP = 4,
			// Token: 0x040016E8 RID: 5864
			WSMAN_FLAG_SERVER_BUFFERING_MODE_BLOCK = 8,
			// Token: 0x040016E9 RID: 5865
			WSMAN_FLAG_RECEIVE_DELAY_OUTPUT_STREAM = 16
		}

		// Token: 0x020003A0 RID: 928
		internal enum WSManDataType : uint
		{
			// Token: 0x040016EB RID: 5867
			WSMAN_DATA_NONE,
			// Token: 0x040016EC RID: 5868
			WSMAN_DATA_TYPE_TEXT,
			// Token: 0x040016ED RID: 5869
			WSMAN_DATA_TYPE_BINARY,
			// Token: 0x040016EE RID: 5870
			WSMAN_DATA_TYPE_WS_XML_READER,
			// Token: 0x040016EF RID: 5871
			WSMAN_DATA_TYPE_DWORD
		}

		// Token: 0x020003A1 RID: 929
		[StructLayout(LayoutKind.Sequential)]
		internal class WSManDataStruct
		{
			// Token: 0x040016F0 RID: 5872
			internal uint type;

			// Token: 0x040016F1 RID: 5873
			internal WSManNativeApi.WSManBinaryOrTextDataStruct binaryOrTextData;
		}

		// Token: 0x020003A2 RID: 930
		[StructLayout(LayoutKind.Sequential)]
		internal class WSManBinaryOrTextDataStruct
		{
			// Token: 0x040016F2 RID: 5874
			internal int bufferLength;

			// Token: 0x040016F3 RID: 5875
			internal IntPtr data;
		}

		// Token: 0x020003A3 RID: 931
		internal class WSManData_ManToUn : IDisposable
		{
			// Token: 0x06002CA2 RID: 11426 RVA: 0x000F8ACC File Offset: 0x000F6CCC
			internal WSManData_ManToUn(byte[] data)
			{
				this.internalData = new WSManNativeApi.WSManDataStruct();
				this.internalData.binaryOrTextData = new WSManNativeApi.WSManBinaryOrTextDataStruct();
				this.internalData.binaryOrTextData.bufferLength = data.Length;
				this.internalData.type = 2U;
				IntPtr data2 = Marshal.AllocHGlobal(this.internalData.binaryOrTextData.bufferLength);
				this.internalData.binaryOrTextData.data = data2;
				this.marshalledBuffer = data2;
				Marshal.Copy(data, 0, this.internalData.binaryOrTextData.data, this.internalData.binaryOrTextData.bufferLength);
				this.marshalledObject = Marshal.AllocHGlobal(ClrFacade.SizeOf<WSManNativeApi.WSManDataStruct>());
				Marshal.StructureToPtr(this.internalData, this.marshalledObject, false);
			}

			// Token: 0x06002CA3 RID: 11427 RVA: 0x000F8BA8 File Offset: 0x000F6DA8
			internal WSManData_ManToUn(string data)
			{
				this.internalData = new WSManNativeApi.WSManDataStruct();
				this.internalData.binaryOrTextData = new WSManNativeApi.WSManBinaryOrTextDataStruct();
				this.internalData.binaryOrTextData.bufferLength = data.Length;
				this.internalData.type = 1U;
				this.internalData.binaryOrTextData.data = Marshal.StringToHGlobalUni(data);
				this.marshalledBuffer = this.internalData.binaryOrTextData.data;
				this.marshalledObject = Marshal.AllocHGlobal(ClrFacade.SizeOf<WSManNativeApi.WSManDataStruct>());
				Marshal.StructureToPtr(this.internalData, this.marshalledObject, false);
			}

			// Token: 0x06002CA4 RID: 11428 RVA: 0x000F8C5C File Offset: 0x000F6E5C
			~WSManData_ManToUn()
			{
				this.Dispose(false);
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x000F8C8C File Offset: 0x000F6E8C
			// (set) Token: 0x06002CA6 RID: 11430 RVA: 0x000F8C99 File Offset: 0x000F6E99
			internal uint Type
			{
				get
				{
					return this.internalData.type;
				}
				set
				{
					this.internalData.type = value;
				}
			}

			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x000F8CA7 File Offset: 0x000F6EA7
			// (set) Token: 0x06002CA8 RID: 11432 RVA: 0x000F8CB9 File Offset: 0x000F6EB9
			internal int BufferLength
			{
				get
				{
					return this.internalData.binaryOrTextData.bufferLength;
				}
				set
				{
					this.internalData.binaryOrTextData.bufferLength = value;
				}
			}

			// Token: 0x06002CA9 RID: 11433 RVA: 0x000F8CCC File Offset: 0x000F6ECC
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002CAA RID: 11434 RVA: 0x000F8CDC File Offset: 0x000F6EDC
			private void Dispose(bool isDisposing)
			{
				if (this.marshalledBuffer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.marshalledBuffer);
					this.marshalledBuffer = IntPtr.Zero;
				}
				if (this.marshalledObject != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.marshalledObject);
					this.marshalledObject = IntPtr.Zero;
				}
			}

			// Token: 0x06002CAB RID: 11435 RVA: 0x000F8D39 File Offset: 0x000F6F39
			public static implicit operator IntPtr(WSManNativeApi.WSManData_ManToUn data)
			{
				if (data != null)
				{
					return data.marshalledObject;
				}
				return IntPtr.Zero;
			}

			// Token: 0x040016F4 RID: 5876
			private WSManNativeApi.WSManDataStruct internalData;

			// Token: 0x040016F5 RID: 5877
			private IntPtr marshalledObject = IntPtr.Zero;

			// Token: 0x040016F6 RID: 5878
			private IntPtr marshalledBuffer = IntPtr.Zero;
		}

		// Token: 0x020003A4 RID: 932
		internal class WSManData_UnToMan
		{
			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000F8D4A File Offset: 0x000F6F4A
			// (set) Token: 0x06002CAD RID: 11437 RVA: 0x000F8D52 File Offset: 0x000F6F52
			internal uint Type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._type = value;
				}
			}

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000F8D5B File Offset: 0x000F6F5B
			// (set) Token: 0x06002CAF RID: 11439 RVA: 0x000F8D63 File Offset: 0x000F6F63
			internal int BufferLength
			{
				get
				{
					return this._bufferLength;
				}
				set
				{
					this._bufferLength = value;
				}
			}

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000F8D6C File Offset: 0x000F6F6C
			internal string Text
			{
				get
				{
					if (this.Type == 1U)
					{
						return this._text;
					}
					return string.Empty;
				}
			}

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x000F8D83 File Offset: 0x000F6F83
			internal byte[] Data
			{
				get
				{
					if (this.Type == 2U)
					{
						return this._data;
					}
					return new byte[0];
				}
			}

			// Token: 0x06002CB2 RID: 11442 RVA: 0x000F8D9C File Offset: 0x000F6F9C
			internal static WSManNativeApi.WSManData_UnToMan UnMarshal(WSManNativeApi.WSManDataStruct dataStruct)
			{
				WSManNativeApi.WSManData_UnToMan wsmanData_UnToMan = new WSManNativeApi.WSManData_UnToMan();
				wsmanData_UnToMan._type = dataStruct.type;
				wsmanData_UnToMan._bufferLength = dataStruct.binaryOrTextData.bufferLength;
				switch (dataStruct.type)
				{
				case 1U:
					if (dataStruct.binaryOrTextData.bufferLength > 0)
					{
						string text = Marshal.PtrToStringUni(dataStruct.binaryOrTextData.data, dataStruct.binaryOrTextData.bufferLength);
						wsmanData_UnToMan._text = text;
					}
					break;
				case 2U:
					if (dataStruct.binaryOrTextData.bufferLength > 0)
					{
						byte[] array = new byte[dataStruct.binaryOrTextData.bufferLength];
						Marshal.Copy(dataStruct.binaryOrTextData.data, array, 0, dataStruct.binaryOrTextData.bufferLength);
						wsmanData_UnToMan._data = array;
					}
					break;
				default:
					throw new NotSupportedException();
				}
				return wsmanData_UnToMan;
			}

			// Token: 0x06002CB3 RID: 11443 RVA: 0x000F8E64 File Offset: 0x000F7064
			internal static WSManNativeApi.WSManData_UnToMan UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManData_UnToMan result = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManDataStruct dataStruct = ClrFacade.PtrToStructure<WSManNativeApi.WSManDataStruct>(unmanagedData);
					result = WSManNativeApi.WSManData_UnToMan.UnMarshal(dataStruct);
				}
				return result;
			}

			// Token: 0x040016F7 RID: 5879
			private uint _type;

			// Token: 0x040016F8 RID: 5880
			private int _bufferLength;

			// Token: 0x040016F9 RID: 5881
			private string _text;

			// Token: 0x040016FA RID: 5882
			private byte[] _data;
		}

		// Token: 0x020003A5 RID: 933
		internal struct WSManDataDWord
		{
			// Token: 0x06002CB5 RID: 11445 RVA: 0x000F8E97 File Offset: 0x000F7097
			internal WSManDataDWord(int data)
			{
				this.dwordData = default(WSManNativeApi.WSManDataDWord.WSManDWordDataInternal);
				this.dwordData.number = data;
				this.type = WSManNativeApi.WSManDataType.WSMAN_DATA_TYPE_DWORD;
			}

			// Token: 0x06002CB6 RID: 11446 RVA: 0x000F8EB8 File Offset: 0x000F70B8
			internal WSManNativeApi.MarshalledObject Marshal()
			{
				return WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManDataDWord>(this);
			}

			// Token: 0x040016FB RID: 5883
			private WSManNativeApi.WSManDataType type;

			// Token: 0x040016FC RID: 5884
			private WSManNativeApi.WSManDataDWord.WSManDWordDataInternal dwordData;

			// Token: 0x020003A6 RID: 934
			private struct WSManDWordDataInternal
			{
				// Token: 0x040016FD RID: 5885
				internal int number;

				// Token: 0x040016FE RID: 5886
				internal IntPtr reserved;
			}
		}

		// Token: 0x020003A7 RID: 935
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WSManStreamIDSetStruct
		{
			// Token: 0x040016FF RID: 5887
			internal int streamIDsCount;

			// Token: 0x04001700 RID: 5888
			internal IntPtr streamIDs;
		}

		// Token: 0x020003A8 RID: 936
		internal struct WSManStreamIDSet_ManToUn
		{
			// Token: 0x06002CB7 RID: 11447 RVA: 0x000F8EC8 File Offset: 0x000F70C8
			internal WSManStreamIDSet_ManToUn(string[] streamIds)
			{
				int num = ClrFacade.SizeOf<IntPtr>();
				this.streamSetInfo = default(WSManNativeApi.WSManStreamIDSetStruct);
				this.streamSetInfo.streamIDsCount = streamIds.Length;
				this.streamSetInfo.streamIDs = Marshal.AllocHGlobal(num * streamIds.Length);
				for (int i = 0; i < streamIds.Length; i++)
				{
					IntPtr val = Marshal.StringToHGlobalUni(streamIds[i]);
					Marshal.WriteIntPtr(this.streamSetInfo.streamIDs, i * num, val);
				}
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManStreamIDSetStruct>(this.streamSetInfo);
			}

			// Token: 0x06002CB8 RID: 11448 RVA: 0x000F8F48 File Offset: 0x000F7148
			internal void Dispose()
			{
				if (IntPtr.Zero != this.streamSetInfo.streamIDs)
				{
					int num = ClrFacade.SizeOf<IntPtr>();
					for (int i = 0; i < this.streamSetInfo.streamIDsCount; i++)
					{
						IntPtr intPtr = IntPtr.Zero;
						intPtr = Marshal.ReadIntPtr(this.streamSetInfo.streamIDs, i * num);
						if (IntPtr.Zero != intPtr)
						{
							Marshal.FreeHGlobal(intPtr);
							intPtr = IntPtr.Zero;
						}
					}
					Marshal.FreeHGlobal(this.streamSetInfo.streamIDs);
					this.streamSetInfo.streamIDs = IntPtr.Zero;
				}
				this.data.Dispose();
			}

			// Token: 0x06002CB9 RID: 11449 RVA: 0x000F8FE6 File Offset: 0x000F71E6
			public static implicit operator IntPtr(WSManNativeApi.WSManStreamIDSet_ManToUn obj)
			{
				return obj.data.DataPtr;
			}

			// Token: 0x04001701 RID: 5889
			private WSManNativeApi.WSManStreamIDSetStruct streamSetInfo;

			// Token: 0x04001702 RID: 5890
			private WSManNativeApi.MarshalledObject data;
		}

		// Token: 0x020003A9 RID: 937
		internal class WSManStreamIDSet_UnToMan
		{
			// Token: 0x06002CBA RID: 11450 RVA: 0x000F8FF4 File Offset: 0x000F71F4
			internal static WSManNativeApi.WSManStreamIDSet_UnToMan UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManStreamIDSet_UnToMan wsmanStreamIDSet_UnToMan = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManStreamIDSetStruct wsmanStreamIDSetStruct = ClrFacade.PtrToStructure<WSManNativeApi.WSManStreamIDSetStruct>(unmanagedData);
					wsmanStreamIDSet_UnToMan = new WSManNativeApi.WSManStreamIDSet_UnToMan();
					string[] array = null;
					if (wsmanStreamIDSetStruct.streamIDsCount > 0)
					{
						array = new string[wsmanStreamIDSetStruct.streamIDsCount];
						IntPtr[] array2 = new IntPtr[wsmanStreamIDSetStruct.streamIDsCount];
						Marshal.Copy(wsmanStreamIDSetStruct.streamIDs, array2, 0, wsmanStreamIDSetStruct.streamIDsCount);
						for (int i = 0; i < wsmanStreamIDSetStruct.streamIDsCount; i++)
						{
							array[i] = Marshal.PtrToStringUni(array2[i]);
						}
					}
					wsmanStreamIDSet_UnToMan.streamIDs = array;
					wsmanStreamIDSet_UnToMan.streamIDsCount = wsmanStreamIDSetStruct.streamIDsCount;
				}
				return wsmanStreamIDSet_UnToMan;
			}

			// Token: 0x04001703 RID: 5891
			internal string[] streamIDs;

			// Token: 0x04001704 RID: 5892
			internal int streamIDsCount;
		}

		// Token: 0x020003AA RID: 938
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WSManOption
		{
			// Token: 0x04001705 RID: 5893
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string name;

			// Token: 0x04001706 RID: 5894
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string value;

			// Token: 0x04001707 RID: 5895
			internal bool mustComply;
		}

		// Token: 0x020003AB RID: 939
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WSManOptionSetStruct
		{
			// Token: 0x04001708 RID: 5896
			internal int optionsCount;

			// Token: 0x04001709 RID: 5897
			internal IntPtr options;

			// Token: 0x0400170A RID: 5898
			internal bool optionsMustUnderstand;
		}

		// Token: 0x020003AC RID: 940
		internal struct WSManOptionSet : IDisposable
		{
			// Token: 0x06002CBC RID: 11452 RVA: 0x000F90A4 File Offset: 0x000F72A4
			internal WSManOptionSet(WSManNativeApi.WSManOption[] options)
			{
				int num = ClrFacade.SizeOf<WSManNativeApi.WSManOption>();
				this.optionSet = default(WSManNativeApi.WSManOptionSetStruct);
				this.optionSet.optionsCount = options.Length;
				this.optionSet.optionsMustUnderstand = true;
				this.optionSet.options = Marshal.AllocHGlobal(num * options.Length);
				for (int i = 0; i < options.Length; i++)
				{
					Marshal.StructureToPtr(options[i], (IntPtr)(this.optionSet.options.ToInt64() + (long)(num * i)), false);
				}
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManOptionSetStruct>(this.optionSet);
				this.optionsCount = 0;
				this.options = null;
				this.optionsMustUnderstand = false;
			}

			// Token: 0x06002CBD RID: 11453 RVA: 0x000F9158 File Offset: 0x000F7358
			public void Dispose()
			{
				if (IntPtr.Zero != this.optionSet.options)
				{
					Marshal.FreeHGlobal(this.optionSet.options);
					this.optionSet.options = IntPtr.Zero;
				}
				this.data.Dispose();
			}

			// Token: 0x06002CBE RID: 11454 RVA: 0x000F91A7 File Offset: 0x000F73A7
			public static implicit operator IntPtr(WSManNativeApi.WSManOptionSet optionSet)
			{
				return optionSet.data.DataPtr;
			}

			// Token: 0x06002CBF RID: 11455 RVA: 0x000F91B8 File Offset: 0x000F73B8
			internal static WSManNativeApi.WSManOptionSet UnMarshal(IntPtr unmanagedData)
			{
				if (IntPtr.Zero == unmanagedData)
				{
					return default(WSManNativeApi.WSManOptionSet);
				}
				WSManNativeApi.WSManOptionSetStruct resultInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManOptionSetStruct>(unmanagedData);
				return WSManNativeApi.WSManOptionSet.UnMarshal(resultInternal);
			}

			// Token: 0x06002CC0 RID: 11456 RVA: 0x000F91EC File Offset: 0x000F73EC
			internal static WSManNativeApi.WSManOptionSet UnMarshal(WSManNativeApi.WSManOptionSetStruct resultInternal)
			{
				WSManNativeApi.WSManOption[] array = null;
				if (resultInternal.optionsCount > 0)
				{
					array = new WSManNativeApi.WSManOption[resultInternal.optionsCount];
					int num = ClrFacade.SizeOf<WSManNativeApi.WSManOption>();
					IntPtr pointer = resultInternal.options;
					for (int i = 0; i < resultInternal.optionsCount; i++)
					{
						IntPtr ptr = IntPtr.Add(pointer, i * num);
						array[i] = ClrFacade.PtrToStructure<WSManNativeApi.WSManOption>(ptr);
					}
				}
				return new WSManNativeApi.WSManOptionSet
				{
					optionsCount = resultInternal.optionsCount,
					options = array,
					optionsMustUnderstand = resultInternal.optionsMustUnderstand
				};
			}

			// Token: 0x0400170B RID: 5899
			private WSManNativeApi.WSManOptionSetStruct optionSet;

			// Token: 0x0400170C RID: 5900
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x0400170D RID: 5901
			internal int optionsCount;

			// Token: 0x0400170E RID: 5902
			internal WSManNativeApi.WSManOption[] options;

			// Token: 0x0400170F RID: 5903
			internal bool optionsMustUnderstand;
		}

		// Token: 0x020003AD RID: 941
		internal struct WSManCommandArgSet : IDisposable
		{
			// Token: 0x06002CC1 RID: 11457 RVA: 0x000F9280 File Offset: 0x000F7480
			internal WSManCommandArgSet(byte[] firstArgument)
			{
				this.internalData = default(WSManNativeApi.WSManCommandArgSet.WSManCommandArgSetInternal);
				this.internalData.argsCount = 1;
				this.internalData.args = Marshal.AllocHGlobal(ClrFacade.SizeOf<IntPtr>());
				string s = Convert.ToBase64String(firstArgument);
				IntPtr val = Marshal.StringToHGlobalUni(s);
				Marshal.WriteIntPtr(this.internalData.args, val);
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManCommandArgSet.WSManCommandArgSetInternal>(this.internalData);
				this.args = null;
				this.argsCount = 0;
			}

			// Token: 0x06002CC2 RID: 11458 RVA: 0x000F92F8 File Offset: 0x000F74F8
			public void Dispose()
			{
				IntPtr intPtr = Marshal.ReadIntPtr(this.internalData.args);
				if (IntPtr.Zero != intPtr)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				Marshal.FreeHGlobal(this.internalData.args);
				this.data.Dispose();
			}

			// Token: 0x06002CC3 RID: 11459 RVA: 0x000F9344 File Offset: 0x000F7544
			public static implicit operator IntPtr(WSManNativeApi.WSManCommandArgSet obj)
			{
				return obj.data.DataPtr;
			}

			// Token: 0x06002CC4 RID: 11460 RVA: 0x000F9354 File Offset: 0x000F7554
			internal static WSManNativeApi.WSManCommandArgSet UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManCommandArgSet result = default(WSManNativeApi.WSManCommandArgSet);
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManCommandArgSet.WSManCommandArgSetInternal wsmanCommandArgSetInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManCommandArgSet.WSManCommandArgSetInternal>(unmanagedData);
					string[] array = null;
					if (wsmanCommandArgSetInternal.argsCount > 0)
					{
						array = new string[wsmanCommandArgSetInternal.argsCount];
						IntPtr[] array2 = new IntPtr[wsmanCommandArgSetInternal.argsCount];
						Marshal.Copy(wsmanCommandArgSetInternal.args, array2, 0, wsmanCommandArgSetInternal.argsCount);
						for (int i = 0; i < wsmanCommandArgSetInternal.argsCount; i++)
						{
							array[i] = Marshal.PtrToStringUni(array2[i]);
						}
					}
					result.argsCount = wsmanCommandArgSetInternal.argsCount;
					result.args = array;
				}
				return result;
			}

			// Token: 0x04001710 RID: 5904
			private WSManNativeApi.WSManCommandArgSet.WSManCommandArgSetInternal internalData;

			// Token: 0x04001711 RID: 5905
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x04001712 RID: 5906
			internal string[] args;

			// Token: 0x04001713 RID: 5907
			internal int argsCount;

			// Token: 0x020003AE RID: 942
			internal struct WSManCommandArgSetInternal
			{
				// Token: 0x04001714 RID: 5908
				internal int argsCount;

				// Token: 0x04001715 RID: 5909
				internal IntPtr args;
			}
		}

		// Token: 0x020003AF RID: 943
		internal struct WSManShellDisconnectInfo : IDisposable
		{
			// Token: 0x06002CC5 RID: 11461 RVA: 0x000F93FE File Offset: 0x000F75FE
			internal WSManShellDisconnectInfo(uint serverIdleTimeOut)
			{
				this.internalInfo = default(WSManNativeApi.WSManShellDisconnectInfo.WSManShellDisconnectInfoInternal);
				this.internalInfo.idleTimeoutMs = serverIdleTimeOut;
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManShellDisconnectInfo.WSManShellDisconnectInfoInternal>(this.internalInfo);
			}

			// Token: 0x06002CC6 RID: 11462 RVA: 0x000F9429 File Offset: 0x000F7629
			public void Dispose()
			{
				this.data.Dispose();
			}

			// Token: 0x06002CC7 RID: 11463 RVA: 0x000F9436 File Offset: 0x000F7636
			public static implicit operator IntPtr(WSManNativeApi.WSManShellDisconnectInfo disconnectInfo)
			{
				return disconnectInfo.data.DataPtr;
			}

			// Token: 0x04001716 RID: 5910
			private WSManNativeApi.WSManShellDisconnectInfo.WSManShellDisconnectInfoInternal internalInfo;

			// Token: 0x04001717 RID: 5911
			internal WSManNativeApi.MarshalledObject data;

			// Token: 0x020003B0 RID: 944
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManShellDisconnectInfoInternal
			{
				// Token: 0x04001718 RID: 5912
				internal uint idleTimeoutMs;
			}
		}

		// Token: 0x020003B1 RID: 945
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WSManShellStartupInfoStruct
		{
			// Token: 0x04001719 RID: 5913
			internal IntPtr inputStreamSet;

			// Token: 0x0400171A RID: 5914
			internal IntPtr outputStreamSet;

			// Token: 0x0400171B RID: 5915
			internal uint idleTimeoutMs;

			// Token: 0x0400171C RID: 5916
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string workingDirectory;

			// Token: 0x0400171D RID: 5917
			internal IntPtr environmentVariableSet;

			// Token: 0x0400171E RID: 5918
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string name;
		}

		// Token: 0x020003B2 RID: 946
		internal struct WSManShellStartupInfo_ManToUn : IDisposable
		{
			// Token: 0x06002CC8 RID: 11464 RVA: 0x000F9444 File Offset: 0x000F7644
			internal WSManShellStartupInfo_ManToUn(WSManNativeApi.WSManStreamIDSet_ManToUn inputStreamSet, WSManNativeApi.WSManStreamIDSet_ManToUn outputStreamSet, uint serverIdleTimeOut, string name)
			{
				this.internalInfo = default(WSManNativeApi.WSManShellStartupInfoStruct);
				this.internalInfo.inputStreamSet = inputStreamSet;
				this.internalInfo.outputStreamSet = outputStreamSet;
				this.internalInfo.idleTimeoutMs = serverIdleTimeOut;
				this.internalInfo.workingDirectory = null;
				this.internalInfo.environmentVariableSet = IntPtr.Zero;
				this.internalInfo.name = name;
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManShellStartupInfoStruct>(this.internalInfo);
			}

			// Token: 0x06002CC9 RID: 11465 RVA: 0x000F94C5 File Offset: 0x000F76C5
			public void Dispose()
			{
				this.data.Dispose();
			}

			// Token: 0x06002CCA RID: 11466 RVA: 0x000F94D2 File Offset: 0x000F76D2
			public static implicit operator IntPtr(WSManNativeApi.WSManShellStartupInfo_ManToUn startupInfo)
			{
				return startupInfo.data.DataPtr;
			}

			// Token: 0x0400171F RID: 5919
			private WSManNativeApi.WSManShellStartupInfoStruct internalInfo;

			// Token: 0x04001720 RID: 5920
			internal WSManNativeApi.MarshalledObject data;
		}

		// Token: 0x020003B3 RID: 947
		internal class WSManShellStartupInfo_UnToMan
		{
			// Token: 0x06002CCB RID: 11467 RVA: 0x000F94E0 File Offset: 0x000F76E0
			internal static WSManNativeApi.WSManShellStartupInfo_UnToMan UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManShellStartupInfo_UnToMan wsmanShellStartupInfo_UnToMan = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManShellStartupInfoStruct wsmanShellStartupInfoStruct = ClrFacade.PtrToStructure<WSManNativeApi.WSManShellStartupInfoStruct>(unmanagedData);
					wsmanShellStartupInfo_UnToMan = new WSManNativeApi.WSManShellStartupInfo_UnToMan();
					wsmanShellStartupInfo_UnToMan.inputStreamSet = WSManNativeApi.WSManStreamIDSet_UnToMan.UnMarshal(wsmanShellStartupInfoStruct.inputStreamSet);
					wsmanShellStartupInfo_UnToMan.outputStreamSet = WSManNativeApi.WSManStreamIDSet_UnToMan.UnMarshal(wsmanShellStartupInfoStruct.outputStreamSet);
					wsmanShellStartupInfo_UnToMan.idleTimeoutMS = wsmanShellStartupInfoStruct.idleTimeoutMs;
					wsmanShellStartupInfo_UnToMan.workingDirectory = wsmanShellStartupInfoStruct.workingDirectory;
					wsmanShellStartupInfo_UnToMan.environmentVariableSet = WSManNativeApi.WSManEnvironmentVariableSet.UnMarshal(wsmanShellStartupInfoStruct.environmentVariableSet);
					wsmanShellStartupInfo_UnToMan.name = wsmanShellStartupInfoStruct.name;
				}
				return wsmanShellStartupInfo_UnToMan;
			}

			// Token: 0x04001721 RID: 5921
			internal WSManNativeApi.WSManStreamIDSet_UnToMan inputStreamSet;

			// Token: 0x04001722 RID: 5922
			internal WSManNativeApi.WSManStreamIDSet_UnToMan outputStreamSet;

			// Token: 0x04001723 RID: 5923
			internal uint idleTimeoutMS;

			// Token: 0x04001724 RID: 5924
			internal string workingDirectory;

			// Token: 0x04001725 RID: 5925
			internal WSManNativeApi.WSManEnvironmentVariableSet environmentVariableSet;

			// Token: 0x04001726 RID: 5926
			internal string name;
		}

		// Token: 0x020003B4 RID: 948
		internal class WSManEnvironmentVariableSet
		{
			// Token: 0x06002CCD RID: 11469 RVA: 0x000F9570 File Offset: 0x000F7770
			internal static WSManNativeApi.WSManEnvironmentVariableSet UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManEnvironmentVariableSet wsmanEnvironmentVariableSet = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableSetInternal wsmanEnvironmentVariableSetInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableSetInternal>(unmanagedData);
					wsmanEnvironmentVariableSet = new WSManNativeApi.WSManEnvironmentVariableSet();
					WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableInternal[] array = null;
					if (wsmanEnvironmentVariableSetInternal.varsCount > 0U)
					{
						array = new WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableInternal[wsmanEnvironmentVariableSetInternal.varsCount];
						int num = ClrFacade.SizeOf<WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableInternal>();
						IntPtr pointer = wsmanEnvironmentVariableSetInternal.vars;
						int num2 = 0;
						while ((long)num2 < (long)((ulong)wsmanEnvironmentVariableSetInternal.varsCount))
						{
							IntPtr ptr = IntPtr.Add(pointer, num2 * num);
							array[num2] = ClrFacade.PtrToStructure<WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableInternal>(ptr);
							num2++;
						}
					}
					wsmanEnvironmentVariableSet.vars = array;
					wsmanEnvironmentVariableSet.varsCount = wsmanEnvironmentVariableSetInternal.varsCount;
				}
				return wsmanEnvironmentVariableSet;
			}

			// Token: 0x04001727 RID: 5927
			internal uint varsCount;

			// Token: 0x04001728 RID: 5928
			internal WSManNativeApi.WSManEnvironmentVariableSet.WSManEnvironmentVariableInternal[] vars;

			// Token: 0x020003B5 RID: 949
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManEnvironmentVariableSetInternal
			{
				// Token: 0x04001729 RID: 5929
				internal uint varsCount;

				// Token: 0x0400172A RID: 5930
				internal IntPtr vars;
			}

			// Token: 0x020003B6 RID: 950
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct WSManEnvironmentVariableInternal
			{
				// Token: 0x0400172B RID: 5931
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string name;

				// Token: 0x0400172C RID: 5932
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string value;
			}
		}

		// Token: 0x020003B7 RID: 951
		internal class WSManProxyInfo : IDisposable
		{
			// Token: 0x06002CCF RID: 11471 RVA: 0x000F961C File Offset: 0x000F781C
			internal WSManProxyInfo(ProxyAccessType proxyAccessType, WSManNativeApi.WSManUserNameAuthenticationCredentials authCredentials)
			{
				WSManNativeApi.WSManProxyInfo.WSManProxyInfoInternal obj = default(WSManNativeApi.WSManProxyInfo.WSManProxyInfoInternal);
				obj.proxyAccessType = (int)proxyAccessType;
				obj.proxyAuthCredentialsStruct = default(WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct);
				obj.proxyAuthCredentialsStruct.authenticationMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_DEFAULT_AUTHENTICATION;
				if (authCredentials != null)
				{
					obj.proxyAuthCredentialsStruct = authCredentials.CredentialStruct;
				}
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManProxyInfo.WSManProxyInfoInternal>(obj);
			}

			// Token: 0x06002CD0 RID: 11472 RVA: 0x000F9675 File Offset: 0x000F7875
			public void Dispose()
			{
				this.data.Dispose();
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002CD1 RID: 11473 RVA: 0x000F9688 File Offset: 0x000F7888
			public static implicit operator IntPtr(WSManNativeApi.WSManProxyInfo proxyInfo)
			{
				return proxyInfo.data.DataPtr;
			}

			// Token: 0x0400172D RID: 5933
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x020003B8 RID: 952
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManProxyInfoInternal
			{
				// Token: 0x0400172E RID: 5934
				public int proxyAccessType;

				// Token: 0x0400172F RID: 5935
				public WSManNativeApi.WSManUserNameAuthenticationCredentials.WSManUserNameCredentialStruct proxyAuthCredentialsStruct;
			}
		}

		// Token: 0x020003B9 RID: 953
		internal enum WSManCallbackFlags
		{
			// Token: 0x04001731 RID: 5937
			WSMAN_FLAG_CALLBACK_END_OF_OPERATION = 1,
			// Token: 0x04001732 RID: 5938
			WSMAN_FLAG_CALLBACK_END_OF_STREAM = 8,
			// Token: 0x04001733 RID: 5939
			WSMAN_FLAG_CALLBACK_SHELL_SUPPORTS_DISCONNECT = 32,
			// Token: 0x04001734 RID: 5940
			WSMAN_FLAG_CALLBACK_SHELL_AUTODISCONNECTED = 64,
			// Token: 0x04001735 RID: 5941
			WSMAN_FLAG_CALLBACK_NETWORK_FAILURE_DETECTED = 256,
			// Token: 0x04001736 RID: 5942
			WSMAN_FLAG_CALLBACK_RETRYING_AFTER_NETWORK_FAILURE = 512,
			// Token: 0x04001737 RID: 5943
			WSMAN_FLAG_CALLBACK_RECONNECTED_AFTER_NETWORK_FAILURE = 1024,
			// Token: 0x04001738 RID: 5944
			WSMAN_FLAG_CALLBACK_SHELL_AUTODISCONNECTING = 2048,
			// Token: 0x04001739 RID: 5945
			WSMAN_FLAG_CALLBACK_RETRY_ABORTED_DUE_TO_INTERNAL_ERROR = 4096,
			// Token: 0x0400173A RID: 5946
			WSMAN_FLAG_RECEIVE_DELAY_STREAM_REQUEST_PROCESSED = 8192
		}

		// Token: 0x020003BA RID: 954
		// (Invoke) Token: 0x06002CD3 RID: 11475
		internal delegate void WSManShellCompletionFunction(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data);

		// Token: 0x020003BB RID: 955
		internal struct WSManShellAsyncCallback
		{
			// Token: 0x06002CD6 RID: 11478 RVA: 0x000F9695 File Offset: 0x000F7895
			internal WSManShellAsyncCallback(WSManNativeApi.WSManShellCompletionFunction callback)
			{
				this.gcHandle = GCHandle.Alloc(callback);
				this.asyncCallback = Marshal.GetFunctionPointerForDelegate(callback);
			}

			// Token: 0x06002CD7 RID: 11479 RVA: 0x000F96AF File Offset: 0x000F78AF
			public static implicit operator IntPtr(WSManNativeApi.WSManShellAsyncCallback callback)
			{
				return callback.asyncCallback;
			}

			// Token: 0x0400173B RID: 5947
			private GCHandle gcHandle;

			// Token: 0x0400173C RID: 5948
			private IntPtr asyncCallback;
		}

		// Token: 0x020003BC RID: 956
		internal class WSManShellAsync
		{
			// Token: 0x06002CD8 RID: 11480 RVA: 0x000F96B8 File Offset: 0x000F78B8
			internal WSManShellAsync(IntPtr context, WSManNativeApi.WSManShellAsyncCallback callback)
			{
				this.internalData = default(WSManNativeApi.WSManShellAsync.WSManShellAsyncInternal);
				this.internalData.operationContext = context;
				this.internalData.asyncCallback = callback;
				this.data = WSManNativeApi.MarshalledObject.Create<WSManNativeApi.WSManShellAsync.WSManShellAsyncInternal>(this.internalData);
			}

			// Token: 0x06002CD9 RID: 11481 RVA: 0x000F9705 File Offset: 0x000F7905
			public void Dispose()
			{
				this.data.Dispose();
			}

			// Token: 0x06002CDA RID: 11482 RVA: 0x000F9712 File Offset: 0x000F7912
			public static implicit operator IntPtr(WSManNativeApi.WSManShellAsync async)
			{
				return async.data;
			}

			// Token: 0x0400173D RID: 5949
			private WSManNativeApi.MarshalledObject data;

			// Token: 0x0400173E RID: 5950
			private WSManNativeApi.WSManShellAsync.WSManShellAsyncInternal internalData;

			// Token: 0x020003BD RID: 957
			internal struct WSManShellAsyncInternal
			{
				// Token: 0x0400173F RID: 5951
				internal IntPtr operationContext;

				// Token: 0x04001740 RID: 5952
				internal IntPtr asyncCallback;
			}
		}

		// Token: 0x020003BE RID: 958
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WSManError
		{
			// Token: 0x06002CDB RID: 11483 RVA: 0x000F971F File Offset: 0x000F791F
			internal static WSManNativeApi.WSManError UnMarshal(IntPtr unmanagedData)
			{
				return ClrFacade.PtrToStructure<WSManNativeApi.WSManError>(unmanagedData);
			}

			// Token: 0x04001741 RID: 5953
			internal int errorCode;

			// Token: 0x04001742 RID: 5954
			internal string errorDetail;

			// Token: 0x04001743 RID: 5955
			internal string language;

			// Token: 0x04001744 RID: 5956
			internal string machineName;
		}

		// Token: 0x020003BF RID: 959
		internal class WSManCreateShellDataResult
		{
			// Token: 0x06002CDC RID: 11484 RVA: 0x000F9728 File Offset: 0x000F7928
			internal static WSManNativeApi.WSManCreateShellDataResult UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManCreateShellDataResult wsmanCreateShellDataResult = new WSManNativeApi.WSManCreateShellDataResult();
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManCreateShellDataResult.WSManCreateShellDataResultInternal wsmanCreateShellDataResultInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManCreateShellDataResult.WSManCreateShellDataResultInternal>(unmanagedData);
					string text = null;
					if (wsmanCreateShellDataResultInternal.data.textData.textLength > 0)
					{
						text = Marshal.PtrToStringUni(wsmanCreateShellDataResultInternal.data.textData.text, wsmanCreateShellDataResultInternal.data.textData.textLength);
					}
					wsmanCreateShellDataResult.data = text;
				}
				return wsmanCreateShellDataResult;
			}

			// Token: 0x04001745 RID: 5957
			internal string data;

			// Token: 0x020003C0 RID: 960
			private struct WSManCreateShellDataResultInternal
			{
				// Token: 0x04001746 RID: 5958
				internal WSManNativeApi.WSManCreateShellDataResult.WSManDataStruct data;
			}

			// Token: 0x020003C1 RID: 961
			private struct WSManDataStruct
			{
				// Token: 0x04001747 RID: 5959
				internal uint type;

				// Token: 0x04001748 RID: 5960
				internal WSManNativeApi.WSManCreateShellDataResult.WSManTextDataInternal textData;
			}

			// Token: 0x020003C2 RID: 962
			private struct WSManTextDataInternal
			{
				// Token: 0x04001749 RID: 5961
				internal int textLength;

				// Token: 0x0400174A RID: 5962
				internal IntPtr text;
			}
		}

		// Token: 0x020003C3 RID: 963
		internal class WSManConnectDataResult
		{
			// Token: 0x06002CDE RID: 11486 RVA: 0x000F97A0 File Offset: 0x000F79A0
			internal static WSManNativeApi.WSManConnectDataResult UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManConnectDataResult.WSManConnectDataResultInternal wsmanConnectDataResultInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManConnectDataResult.WSManConnectDataResultInternal>(unmanagedData);
				string text = null;
				if (wsmanConnectDataResultInternal.data.textData.textLength > 0)
				{
					text = Marshal.PtrToStringUni(wsmanConnectDataResultInternal.data.textData.text, wsmanConnectDataResultInternal.data.textData.textLength);
				}
				return new WSManNativeApi.WSManConnectDataResult
				{
					data = text
				};
			}

			// Token: 0x0400174B RID: 5963
			internal string data;

			// Token: 0x020003C4 RID: 964
			private struct WSManConnectDataResultInternal
			{
				// Token: 0x0400174C RID: 5964
				internal WSManNativeApi.WSManConnectDataResult.WSManDataStruct data;
			}

			// Token: 0x020003C5 RID: 965
			private struct WSManDataStruct
			{
				// Token: 0x0400174D RID: 5965
				internal uint type;

				// Token: 0x0400174E RID: 5966
				internal WSManNativeApi.WSManConnectDataResult.WSManTextDataInternal textData;
			}

			// Token: 0x020003C6 RID: 966
			private struct WSManTextDataInternal
			{
				// Token: 0x0400174F RID: 5967
				internal int textLength;

				// Token: 0x04001750 RID: 5968
				internal IntPtr text;
			}
		}

		// Token: 0x020003C7 RID: 967
		internal class WSManReceiveDataResult
		{
			// Token: 0x06002CE0 RID: 11488 RVA: 0x000F9808 File Offset: 0x000F7A08
			internal static WSManNativeApi.WSManReceiveDataResult UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManReceiveDataResult.WSManReceiveDataResultInternal wsmanReceiveDataResultInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManReceiveDataResult.WSManReceiveDataResultInternal>(unmanagedData);
				byte[] destination = null;
				if (wsmanReceiveDataResultInternal.data.binaryData.bufferLength > 0)
				{
					destination = new byte[wsmanReceiveDataResultInternal.data.binaryData.bufferLength];
					Marshal.Copy(wsmanReceiveDataResultInternal.data.binaryData.buffer, destination, 0, wsmanReceiveDataResultInternal.data.binaryData.bufferLength);
				}
				return new WSManNativeApi.WSManReceiveDataResult
				{
					data = destination,
					stream = wsmanReceiveDataResultInternal.streamId
				};
			}

			// Token: 0x04001751 RID: 5969
			internal byte[] data;

			// Token: 0x04001752 RID: 5970
			internal string stream;

			// Token: 0x020003C8 RID: 968
			private struct WSManReceiveDataResultInternal
			{
				// Token: 0x04001753 RID: 5971
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string streamId;

				// Token: 0x04001754 RID: 5972
				internal WSManNativeApi.WSManReceiveDataResult.WSManDataStruct data;

				// Token: 0x04001755 RID: 5973
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string commandState;

				// Token: 0x04001756 RID: 5974
				internal int exitCode;
			}

			// Token: 0x020003C9 RID: 969
			private struct WSManDataStruct
			{
				// Token: 0x04001757 RID: 5975
				internal uint type;

				// Token: 0x04001758 RID: 5976
				internal WSManNativeApi.WSManReceiveDataResult.WSManBinaryDataInternal binaryData;
			}

			// Token: 0x020003CA RID: 970
			private struct WSManBinaryDataInternal
			{
				// Token: 0x04001759 RID: 5977
				internal int bufferLength;

				// Token: 0x0400175A RID: 5978
				internal IntPtr buffer;
			}
		}

		// Token: 0x020003CB RID: 971
		internal class WSManPluginRequest
		{
			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000F9895 File Offset: 0x000F7A95
			internal bool shutdownNotification
			{
				get
				{
					return this.internalDetails.shutdownNotification;
				}
			}

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x000F98A2 File Offset: 0x000F7AA2
			internal IntPtr shutdownNotificationHandle
			{
				get
				{
					return this.internalDetails.shutdownNotificationHandle;
				}
			}

			// Token: 0x06002CE4 RID: 11492 RVA: 0x000F98B0 File Offset: 0x000F7AB0
			internal static WSManNativeApi.WSManPluginRequest UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManPluginRequest wsmanPluginRequest = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManPluginRequest.WSManPluginRequestInternal wsmanPluginRequestInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManPluginRequest.WSManPluginRequestInternal>(unmanagedData);
					wsmanPluginRequest = new WSManNativeApi.WSManPluginRequest();
					wsmanPluginRequest.senderDetails = WSManNativeApi.WSManSenderDetails.UnMarshal(wsmanPluginRequestInternal.senderDetails);
					wsmanPluginRequest.locale = wsmanPluginRequestInternal.locale;
					wsmanPluginRequest.resourceUri = wsmanPluginRequestInternal.resourceUri;
					wsmanPluginRequest.operationInfo = WSManNativeApi.WSManOperationInfo.UnMarshal(wsmanPluginRequestInternal.operationInfo);
					wsmanPluginRequest.internalDetails = wsmanPluginRequestInternal;
					wsmanPluginRequest.unmanagedHandle = unmanagedData;
				}
				return wsmanPluginRequest;
			}

			// Token: 0x0400175B RID: 5979
			internal WSManNativeApi.WSManSenderDetails senderDetails;

			// Token: 0x0400175C RID: 5980
			internal string locale;

			// Token: 0x0400175D RID: 5981
			internal string resourceUri;

			// Token: 0x0400175E RID: 5982
			internal WSManNativeApi.WSManOperationInfo operationInfo;

			// Token: 0x0400175F RID: 5983
			private WSManNativeApi.WSManPluginRequest.WSManPluginRequestInternal internalDetails;

			// Token: 0x04001760 RID: 5984
			internal IntPtr unmanagedHandle;

			// Token: 0x020003CC RID: 972
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManPluginRequestInternal
			{
				// Token: 0x04001761 RID: 5985
				internal IntPtr senderDetails;

				// Token: 0x04001762 RID: 5986
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string locale;

				// Token: 0x04001763 RID: 5987
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string resourceUri;

				// Token: 0x04001764 RID: 5988
				internal IntPtr operationInfo;

				// Token: 0x04001765 RID: 5989
				internal bool shutdownNotification;

				// Token: 0x04001766 RID: 5990
				internal IntPtr shutdownNotificationHandle;
			}
		}

		// Token: 0x020003CD RID: 973
		internal class WSManSenderDetails
		{
			// Token: 0x06002CE6 RID: 11494 RVA: 0x000F9930 File Offset: 0x000F7B30
			internal static WSManNativeApi.WSManSenderDetails UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManSenderDetails wsmanSenderDetails = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManSenderDetails.WSManSenderDetailsInternal wsmanSenderDetailsInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManSenderDetails.WSManSenderDetailsInternal>(unmanagedData);
					wsmanSenderDetails = new WSManNativeApi.WSManSenderDetails();
					wsmanSenderDetails.senderName = wsmanSenderDetailsInternal.senderName;
					wsmanSenderDetails.authenticationMechanism = wsmanSenderDetailsInternal.authenticationMechanism;
					wsmanSenderDetails.certificateDetails = WSManNativeApi.WSManCertificateDetails.UnMarshal(wsmanSenderDetailsInternal.certificateDetails);
					wsmanSenderDetails.clientToken = wsmanSenderDetailsInternal.clientToken;
					wsmanSenderDetails.httpUrl = wsmanSenderDetailsInternal.httpUrl;
				}
				return wsmanSenderDetails;
			}

			// Token: 0x04001767 RID: 5991
			internal string senderName;

			// Token: 0x04001768 RID: 5992
			internal string authenticationMechanism;

			// Token: 0x04001769 RID: 5993
			internal WSManNativeApi.WSManCertificateDetails certificateDetails;

			// Token: 0x0400176A RID: 5994
			internal IntPtr clientToken;

			// Token: 0x0400176B RID: 5995
			internal string httpUrl;

			// Token: 0x020003CE RID: 974
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManSenderDetailsInternal
			{
				// Token: 0x0400176C RID: 5996
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string senderName;

				// Token: 0x0400176D RID: 5997
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string authenticationMechanism;

				// Token: 0x0400176E RID: 5998
				internal IntPtr certificateDetails;

				// Token: 0x0400176F RID: 5999
				internal IntPtr clientToken;

				// Token: 0x04001770 RID: 6000
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string httpUrl;
			}
		}

		// Token: 0x020003CF RID: 975
		internal class WSManCertificateDetails
		{
			// Token: 0x06002CE8 RID: 11496 RVA: 0x000F99A8 File Offset: 0x000F7BA8
			internal static WSManNativeApi.WSManCertificateDetails UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManCertificateDetails wsmanCertificateDetails = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManCertificateDetails.WSManCertificateDetailsInternal wsmanCertificateDetailsInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManCertificateDetails.WSManCertificateDetailsInternal>(unmanagedData);
					wsmanCertificateDetails = new WSManNativeApi.WSManCertificateDetails();
					wsmanCertificateDetails.subject = wsmanCertificateDetailsInternal.subject;
					wsmanCertificateDetails.issuerName = wsmanCertificateDetailsInternal.issuerName;
					wsmanCertificateDetails.issuerThumbprint = wsmanCertificateDetailsInternal.issuerThumbprint;
					wsmanCertificateDetails.subjectName = wsmanCertificateDetailsInternal.subjectName;
				}
				return wsmanCertificateDetails;
			}

			// Token: 0x04001771 RID: 6001
			internal string subject;

			// Token: 0x04001772 RID: 6002
			internal string issuerName;

			// Token: 0x04001773 RID: 6003
			internal string issuerThumbprint;

			// Token: 0x04001774 RID: 6004
			internal string subjectName;

			// Token: 0x020003D0 RID: 976
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManCertificateDetailsInternal
			{
				// Token: 0x04001775 RID: 6005
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string subject;

				// Token: 0x04001776 RID: 6006
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string issuerName;

				// Token: 0x04001777 RID: 6007
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string issuerThumbprint;

				// Token: 0x04001778 RID: 6008
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string subjectName;
			}
		}

		// Token: 0x020003D1 RID: 977
		internal class WSManOperationInfo
		{
			// Token: 0x06002CEA RID: 11498 RVA: 0x000F9A10 File Offset: 0x000F7C10
			internal static WSManNativeApi.WSManOperationInfo UnMarshal(IntPtr unmanagedData)
			{
				WSManNativeApi.WSManOperationInfo wsmanOperationInfo = null;
				if (IntPtr.Zero != unmanagedData)
				{
					WSManNativeApi.WSManOperationInfo.WSManOperationInfoInternal wsmanOperationInfoInternal = ClrFacade.PtrToStructure<WSManNativeApi.WSManOperationInfo.WSManOperationInfoInternal>(unmanagedData);
					wsmanOperationInfo = new WSManNativeApi.WSManOperationInfo();
					wsmanOperationInfo.fragment = wsmanOperationInfoInternal.fragment;
					wsmanOperationInfo.filter = wsmanOperationInfoInternal.filter;
					wsmanOperationInfo.selectorSet = WSManNativeApi.WSManSelectorSet.UnMarshal(wsmanOperationInfoInternal.selectorSet);
					wsmanOperationInfo.optionSet = WSManNativeApi.WSManOptionSet.UnMarshal(wsmanOperationInfoInternal.optionSet);
				}
				return wsmanOperationInfo;
			}

			// Token: 0x04001779 RID: 6009
			internal WSManNativeApi.WSManOperationInfo.WSManFragmentInternal fragment;

			// Token: 0x0400177A RID: 6010
			internal WSManNativeApi.WSManOperationInfo.WSManFilterInternal filter;

			// Token: 0x0400177B RID: 6011
			internal WSManNativeApi.WSManSelectorSet selectorSet;

			// Token: 0x0400177C RID: 6012
			internal WSManNativeApi.WSManOptionSet optionSet;

			// Token: 0x020003D2 RID: 978
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct WSManOperationInfoInternal
			{
				// Token: 0x0400177D RID: 6013
				internal WSManNativeApi.WSManOperationInfo.WSManFragmentInternal fragment;

				// Token: 0x0400177E RID: 6014
				internal WSManNativeApi.WSManOperationInfo.WSManFilterInternal filter;

				// Token: 0x0400177F RID: 6015
				internal WSManNativeApi.WSManSelectorSet.WSManSelectorSetStruct selectorSet;

				// Token: 0x04001780 RID: 6016
				internal WSManNativeApi.WSManOptionSetStruct optionSet;
			}

			// Token: 0x020003D3 RID: 979
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct WSManFragmentInternal
			{
				// Token: 0x04001781 RID: 6017
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string path;

				// Token: 0x04001782 RID: 6018
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string dialect;
			}

			// Token: 0x020003D4 RID: 980
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct WSManFilterInternal
			{
				// Token: 0x04001783 RID: 6019
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string filter;

				// Token: 0x04001784 RID: 6020
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string dialect;
			}
		}

		// Token: 0x020003D5 RID: 981
		internal class WSManSelectorSet
		{
			// Token: 0x06002CEC RID: 11500 RVA: 0x000F9A80 File Offset: 0x000F7C80
			internal static WSManNativeApi.WSManSelectorSet UnMarshal(WSManNativeApi.WSManSelectorSet.WSManSelectorSetStruct resultInternal)
			{
				WSManNativeApi.WSManSelectorSet.WSManKeyStruct[] array = null;
				if (resultInternal.numberKeys > 0)
				{
					array = new WSManNativeApi.WSManSelectorSet.WSManKeyStruct[resultInternal.numberKeys];
					int num = ClrFacade.SizeOf<WSManNativeApi.WSManSelectorSet.WSManKeyStruct>();
					IntPtr pointer = resultInternal.keys;
					for (int i = 0; i < resultInternal.numberKeys; i++)
					{
						IntPtr ptr = IntPtr.Add(pointer, i * num);
						array[i] = ClrFacade.PtrToStructure<WSManNativeApi.WSManSelectorSet.WSManKeyStruct>(ptr);
					}
				}
				return new WSManNativeApi.WSManSelectorSet
				{
					numberKeys = resultInternal.numberKeys,
					keys = array
				};
			}

			// Token: 0x04001785 RID: 6021
			internal int numberKeys;

			// Token: 0x04001786 RID: 6022
			internal WSManNativeApi.WSManSelectorSet.WSManKeyStruct[] keys;

			// Token: 0x020003D6 RID: 982
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct WSManSelectorSetStruct
			{
				// Token: 0x04001787 RID: 6023
				internal int numberKeys;

				// Token: 0x04001788 RID: 6024
				internal IntPtr keys;
			}

			// Token: 0x020003D7 RID: 983
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct WSManKeyStruct
			{
				// Token: 0x04001789 RID: 6025
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string key;

				// Token: 0x0400178A RID: 6026
				[MarshalAs(UnmanagedType.LPWStr)]
				internal string value;
			}
		}

		// Token: 0x020003D8 RID: 984
		internal enum WSManFlagReceive
		{
			// Token: 0x0400178C RID: 6028
			WSMAN_FLAG_RECEIVE_RESULT_NO_MORE_DATA = 1,
			// Token: 0x0400178D RID: 6029
			WSMAN_FLAG_RECEIVE_FLUSH,
			// Token: 0x0400178E RID: 6030
			WSMAN_FLAG_RECEIVE_RESULT_DATA_BOUNDARY = 4
		}
	}
}
