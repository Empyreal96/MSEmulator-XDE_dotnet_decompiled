using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E0 RID: 736
	internal class RemoteHostResponse
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x000C5FFF File Offset: 0x000C41FF
		internal long CallId
		{
			get
			{
				return this._callId;
			}
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000C6007 File Offset: 0x000C4207
		internal RemoteHostResponse(long callId, RemoteHostMethodId methodId, object returnValue, Exception exception)
		{
			this._callId = callId;
			this._methodId = methodId;
			this._returnValue = returnValue;
			this._exception = exception;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000C602C File Offset: 0x000C422C
		internal object SimulateExecution()
		{
			if (this._exception != null)
			{
				throw this._exception;
			}
			return this._returnValue;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000C6043 File Offset: 0x000C4243
		private static void EncodeAndAddReturnValue(PSObject psObject, object returnValue)
		{
			if (returnValue == null)
			{
				return;
			}
			RemoteHostEncoder.EncodeAndAddAsProperty(psObject, "mr", returnValue);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000C6058 File Offset: 0x000C4258
		private static object DecodeReturnValue(PSObject psObject, Type returnType)
		{
			return RemoteHostEncoder.DecodePropertyValue(psObject, "mr", returnType);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000C6073 File Offset: 0x000C4273
		private static void EncodeAndAddException(PSObject psObject, Exception exception)
		{
			RemoteHostEncoder.EncodeAndAddAsProperty(psObject, "me", exception);
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000C6084 File Offset: 0x000C4284
		private static Exception DecodeException(PSObject psObject)
		{
			object obj = RemoteHostEncoder.DecodePropertyValue(psObject, "me", typeof(Exception));
			if (obj == null)
			{
				return null;
			}
			if (obj is Exception)
			{
				return (Exception)obj;
			}
			throw RemoteHostExceptions.NewDecodingFailedException();
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000C60C0 File Offset: 0x000C42C0
		internal PSObject Encode()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			RemoteHostResponse.EncodeAndAddReturnValue(psobject, this._returnValue);
			RemoteHostResponse.EncodeAndAddException(psobject, this._exception);
			psobject.Properties.Add(new PSNoteProperty("ci", this._callId));
			psobject.Properties.Add(new PSNoteProperty("mi", this._methodId));
			return psobject;
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000C612C File Offset: 0x000C432C
		internal static RemoteHostResponse Decode(PSObject data)
		{
			long propertyValue = RemotingDecoder.GetPropertyValue<long>(data, "ci");
			RemoteHostMethodId propertyValue2 = RemotingDecoder.GetPropertyValue<RemoteHostMethodId>(data, "mi");
			RemoteHostMethodInfo remoteHostMethodInfo = RemoteHostMethodInfo.LookUp(propertyValue2);
			object returnValue = RemoteHostResponse.DecodeReturnValue(data, remoteHostMethodInfo.ReturnType);
			Exception exception = RemoteHostResponse.DecodeException(data);
			return new RemoteHostResponse(propertyValue, propertyValue2, returnValue, exception);
		}

		// Token: 0x04001121 RID: 4385
		private long _callId;

		// Token: 0x04001122 RID: 4386
		private RemoteHostMethodId _methodId;

		// Token: 0x04001123 RID: 4387
		private object _returnValue;

		// Token: 0x04001124 RID: 4388
		private Exception _exception;
	}
}
