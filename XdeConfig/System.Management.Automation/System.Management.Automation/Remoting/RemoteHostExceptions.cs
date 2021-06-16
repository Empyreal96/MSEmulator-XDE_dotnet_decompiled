using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E1 RID: 737
	internal static class RemoteHostExceptions
	{
		// Token: 0x0600232C RID: 9004 RVA: 0x000C6178 File Offset: 0x000C4378
		internal static Exception NewRemoteRunspaceDoesNotSupportPushRunspaceException()
		{
			string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteRunspaceDoesNotSupportPushRunspace, new object[0]);
			return new PSRemotingDataStructureException(message);
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000C619C File Offset: 0x000C439C
		internal static Exception NewDecodingFailedException()
		{
			string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostDecodingFailed, new object[0]);
			return new PSRemotingDataStructureException(message);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000C61C0 File Offset: 0x000C43C0
		internal static Exception NewNotImplementedException(RemoteHostMethodId methodId)
		{
			RemoteHostMethodInfo remoteHostMethodInfo = RemoteHostMethodInfo.LookUp(methodId);
			string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostMethodNotImplemented, new object[]
			{
				remoteHostMethodInfo.Name
			});
			return new PSRemotingDataStructureException(message, new PSNotImplementedException());
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000C61FC File Offset: 0x000C43FC
		internal static Exception NewRemoteHostCallFailedException(RemoteHostMethodId methodId)
		{
			RemoteHostMethodInfo remoteHostMethodInfo = RemoteHostMethodInfo.LookUp(methodId);
			string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostCallFailed, new object[]
			{
				remoteHostMethodInfo.Name
			});
			return new PSRemotingDataStructureException(message);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000C6232 File Offset: 0x000C4432
		internal static Exception NewDecodingErrorForErrorRecordException()
		{
			return new PSRemotingDataStructureException(RemotingErrorIdStrings.DecodingErrorForErrorRecord);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000C6240 File Offset: 0x000C4440
		internal static Exception NewRemoteHostDataEncodingNotSupportedException(Type type)
		{
			return new PSRemotingDataStructureException(RemotingErrorIdStrings.RemoteHostDataEncodingNotSupported, new object[]
			{
				type.ToString()
			});
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000C6268 File Offset: 0x000C4468
		internal static Exception NewRemoteHostDataDecodingNotSupportedException(Type type)
		{
			return new PSRemotingDataStructureException(RemotingErrorIdStrings.RemoteHostDataDecodingNotSupported, new object[]
			{
				type.ToString()
			});
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x000C6290 File Offset: 0x000C4490
		internal static Exception NewUnknownTargetClassException(string className)
		{
			return new PSRemotingDataStructureException(RemotingErrorIdStrings.UnknownTargetClass, new object[]
			{
				className
			});
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000C62B3 File Offset: 0x000C44B3
		internal static Exception NewNullClientHostException()
		{
			return new PSRemotingDataStructureException(RemotingErrorIdStrings.RemoteHostNullClientHost);
		}
	}
}
