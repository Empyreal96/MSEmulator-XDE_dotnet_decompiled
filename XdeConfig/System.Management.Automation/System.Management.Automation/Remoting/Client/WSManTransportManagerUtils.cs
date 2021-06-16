using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x0200038E RID: 910
	internal static class WSManTransportManagerUtils
	{
		// Token: 0x06002C09 RID: 11273 RVA: 0x000F387C File Offset: 0x000F1A7C
		internal static TransportErrorOccuredEventArgs ConstructTransportErrorEventArgs(IntPtr wsmanAPIHandle, WSManClientSessionTransportManager wsmanSessionTM, WSManNativeApi.WSManError errorStruct, TransportMethodEnum transportMethodReportingError, string resourceString, params object[] resourceArgs)
		{
			PSRemotingTransportException ex;
			if (errorStruct.errorCode == -2144108135 && wsmanSessionTM != null)
			{
				IntPtr sessionHandle = wsmanSessionTM.SessionHandle;
				string text = WSManNativeApi.WSManGetSessionOptionAsString(sessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_REDIRECT_LOCATION);
				string text2 = WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(WSManNativeApi.WSManGetErrorMessage(wsmanAPIHandle, errorStruct.errorCode)).Trim();
				ex = new PSRemotingTransportRedirectException(text, PSRemotingErrorId.URIEndPointNotResolved, RemotingErrorIdStrings.URIEndPointNotResolved, new object[]
				{
					text2,
					text
				});
			}
			else if (errorStruct.errorCode == -2144108485 && wsmanSessionTM != null)
			{
				string text3 = wsmanSessionTM.ConnectionInfo.ShellUri.Replace("http://schemas.microsoft.com/powershell/", string.Empty);
				string text4 = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidConfigurationName, new object[]
				{
					text3,
					wsmanSessionTM.ConnectionInfo.ComputerName
				});
				ex = new PSRemotingTransportException(PSRemotingErrorId.InvalidConfigurationName, RemotingErrorIdStrings.ConnectExCallBackError, new object[]
				{
					wsmanSessionTM.ConnectionInfo.ComputerName,
					text4
				});
				ex.TransportMessage = WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(WSManNativeApi.WSManGetErrorMessage(wsmanAPIHandle, errorStruct.errorCode));
			}
			else
			{
				string text5 = PSRemotingErrorInvariants.FormatResourceString(resourceString, resourceArgs);
				ex = new PSRemotingTransportException(PSRemotingErrorId.TroubleShootingHelpTopic, RemotingErrorIdStrings.TroubleShootingHelpTopic, new object[]
				{
					text5
				});
				ex.TransportMessage = WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(WSManNativeApi.WSManGetErrorMessage(wsmanAPIHandle, errorStruct.errorCode));
			}
			ex.ErrorCode = errorStruct.errorCode;
			return new TransportErrorOccuredEventArgs(ex, transportMethodReportingError);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000F39F0 File Offset: 0x000F1BF0
		internal static string ParseEscapeWSManErrorMessage(string errorMessage)
		{
			if (string.IsNullOrEmpty(errorMessage) || !errorMessage.Contains("@{"))
			{
				return errorMessage;
			}
			return errorMessage.Replace("@{", "'@{").Replace("}", "}'");
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000F3A38 File Offset: 0x000F1C38
		internal static string GetFQEIDFromTransportError(int transportErrorCode, string defaultFQEID)
		{
			string str;
			if (WSManTransportManagerUtils._transportErrorCodeToFQEID.TryGetValue(transportErrorCode, out str))
			{
				return str + "," + defaultFQEID;
			}
			if (transportErrorCode != 0)
			{
				return transportErrorCode.ToString(NumberFormatInfo.InvariantInfo) + "," + defaultFQEID;
			}
			return defaultFQEID;
		}

		// Token: 0x04001623 RID: 5667
		private static Dictionary<int, string> _transportErrorCodeToFQEID = new Dictionary<int, string>
		{
			{
				5,
				"AccessDenied"
			},
			{
				14,
				"ServerOutOfMemory"
			},
			{
				53,
				"NetworkPathNotFound"
			},
			{
				-2144108103,
				"ComputerNotFound"
			},
			{
				1311,
				"AuthenticationFailed"
			},
			{
				1326,
				"LogonFailure"
			},
			{
				1722,
				"ImproperResponse"
			},
			{
				-2141974624,
				"IncorrectProtocolVersion"
			},
			{
				-2144108250,
				"WinRMOperationTimeout"
			},
			{
				-2144108269,
				"URLNotAvailable"
			},
			{
				-2144108526,
				"CannotConnect"
			},
			{
				-2144108485,
				"InvalidResourceUri"
			},
			{
				-2144108083,
				"CannotConnectAlreadyConnected"
			},
			{
				-2144108274,
				"InvalidAuthentication"
			},
			{
				1115,
				"ShutDownInProgress"
			},
			{
				-2144108080,
				"CannotConnectInvalidOperation"
			},
			{
				-2144108090,
				"CannotConnectMismatchSessions"
			},
			{
				-2144108065,
				"CannotConnectRunAsFailed"
			},
			{
				-2144108094,
				"SessionCreateFailedInvalidName"
			},
			{
				-2144108453,
				"CannotConnectTargetSessionDoesNotExist"
			},
			{
				-2144108116,
				"RemoteSessionDisallowed"
			},
			{
				-2144108061,
				"RemoteConnectionDisallowed"
			},
			{
				-2144108542,
				"InvalidResourceUri"
			},
			{
				-2144108539,
				"CorruptedWinRMConfig"
			},
			{
				995,
				"WinRMOperationAborted"
			},
			{
				-2144108499,
				"URIExceedsMaxAllowedSize"
			},
			{
				-2144108318,
				"ClientKerberosDisabled"
			},
			{
				-2144108316,
				"ServerNotTrusted"
			},
			{
				-2144108276,
				"WorkgroupCannotUseKerberos"
			},
			{
				-2144108315,
				"ExplicitCredentialsRequired"
			},
			{
				-2144108105,
				"RedirectLocationInvalid"
			},
			{
				-2144108135,
				"RedirectInformationRequired"
			},
			{
				-2144108428,
				"WinRMOperationNotSupportedOnServer"
			},
			{
				-2144108270,
				"CannotConnectWinRMService"
			},
			{
				-2144108176,
				"WinRMHttpError"
			},
			{
				-2146893053,
				"TargetUnknown"
			},
			{
				-2144108101,
				"CannotUseIPAddress"
			}
		};

		// Token: 0x0200038F RID: 911
		internal enum tmStartModes
		{
			// Token: 0x04001625 RID: 5669
			None = 1,
			// Token: 0x04001626 RID: 5670
			Create,
			// Token: 0x04001627 RID: 5671
			Connect
		}
	}
}
