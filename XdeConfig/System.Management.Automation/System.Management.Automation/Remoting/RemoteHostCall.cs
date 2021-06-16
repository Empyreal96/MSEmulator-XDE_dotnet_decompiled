using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Internal.Host;
using System.Reflection;
using System.Security;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002DF RID: 735
	internal class RemoteHostCall
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x000C5936 File Offset: 0x000C3B36
		internal string MethodName
		{
			get
			{
				return this._methodInfo.Name;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x000C5943 File Offset: 0x000C3B43
		internal RemoteHostMethodId MethodId
		{
			get
			{
				return this._methodId;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x000C594B File Offset: 0x000C3B4B
		internal object[] Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x000C5953 File Offset: 0x000C3B53
		internal long CallId
		{
			get
			{
				return this._callId;
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x000C595B File Offset: 0x000C3B5B
		internal RemoteHostCall(long callId, RemoteHostMethodId methodId, object[] parameters)
		{
			this._callId = callId;
			this._methodId = methodId;
			this._parameters = parameters;
			this._methodInfo = RemoteHostMethodInfo.LookUp(methodId);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000C5984 File Offset: 0x000C3B84
		private static PSObject EncodeParameters(object[] parameters)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < parameters.Length; i++)
			{
				object value = (parameters[i] == null) ? null : RemoteHostEncoder.EncodeObject(parameters[i]);
				arrayList.Add(value);
			}
			return new PSObject(arrayList);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000C59C4 File Offset: 0x000C3BC4
		private static object[] DecodeParameters(PSObject parametersPSObject, Type[] parameterTypes)
		{
			ArrayList arrayList = (ArrayList)parametersPSObject.BaseObject;
			List<object> list = new List<object>();
			for (int i = 0; i < arrayList.Count; i++)
			{
				object item = (arrayList[i] == null) ? null : RemoteHostEncoder.DecodeObject(arrayList[i], parameterTypes[i]);
				list.Add(item);
			}
			return list.ToArray();
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000C5A20 File Offset: 0x000C3C20
		internal PSObject Encode()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			PSObject value = RemoteHostCall.EncodeParameters(this._parameters);
			psobject.Properties.Add(new PSNoteProperty("ci", this._callId));
			psobject.Properties.Add(new PSNoteProperty("mi", this._methodId));
			psobject.Properties.Add(new PSNoteProperty("mp", value));
			return psobject;
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x000C5A98 File Offset: 0x000C3C98
		internal static RemoteHostCall Decode(PSObject data)
		{
			long propertyValue = RemotingDecoder.GetPropertyValue<long>(data, "ci");
			PSObject propertyValue2 = RemotingDecoder.GetPropertyValue<PSObject>(data, "mp");
			RemoteHostMethodId propertyValue3 = RemotingDecoder.GetPropertyValue<RemoteHostMethodId>(data, "mi");
			RemoteHostMethodInfo remoteHostMethodInfo = RemoteHostMethodInfo.LookUp(propertyValue3);
			object[] parameters = RemoteHostCall.DecodeParameters(propertyValue2, remoteHostMethodInfo.ParameterTypes);
			return new RemoteHostCall(propertyValue, propertyValue3, parameters);
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x000C5AE7 File Offset: 0x000C3CE7
		internal bool IsVoidMethod
		{
			get
			{
				return this._methodInfo.ReturnType == typeof(void);
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000C5B04 File Offset: 0x000C3D04
		internal void ExecuteVoidMethod(PSHost clientHost)
		{
			if (clientHost == null)
			{
				return;
			}
			RemoteRunspace remoteRunspace = null;
			if (this.IsSetShouldExitOrPopRunspace)
			{
				remoteRunspace = this.GetRemoteRunspaceToClose(clientHost);
			}
			try
			{
				object obj = this.SelectTargetObject(clientHost);
				this.MyMethodBase.Invoke(obj, this._parameters);
			}
			finally
			{
				if (remoteRunspace != null)
				{
					remoteRunspace.Close();
				}
			}
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000C5B60 File Offset: 0x000C3D60
		private RemoteRunspace GetRemoteRunspaceToClose(PSHost clientHost)
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = clientHost as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null || !hostSupportsInteractiveSession.IsRunspacePushed)
			{
				return null;
			}
			RemoteRunspace remoteRunspace = hostSupportsInteractiveSession.Runspace as RemoteRunspace;
			if (remoteRunspace == null || !remoteRunspace.ShouldCloseOnPop)
			{
				return null;
			}
			return remoteRunspace;
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x000C5B9B File Offset: 0x000C3D9B
		private MethodBase MyMethodBase
		{
			get
			{
				return this._methodInfo.InterfaceType.GetMethod(this._methodInfo.Name, this._methodInfo.ParameterTypes);
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000C5BC4 File Offset: 0x000C3DC4
		internal RemoteHostResponse ExecuteNonVoidMethod(PSHost clientHost)
		{
			if (clientHost == null)
			{
				throw RemoteHostExceptions.NewNullClientHostException();
			}
			object instance = this.SelectTargetObject(clientHost);
			return this.ExecuteNonVoidMethodOnObject(instance);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000C5BEC File Offset: 0x000C3DEC
		private RemoteHostResponse ExecuteNonVoidMethodOnObject(object instance)
		{
			Exception exception = null;
			object returnValue = null;
			try
			{
				if (this._methodId == RemoteHostMethodId.GetBufferContents)
				{
					throw new PSRemotingDataStructureException(RemotingErrorIdStrings.RemoteHostGetBufferContents, new object[]
					{
						this._computerName.ToUpper()
					});
				}
				returnValue = this.MyMethodBase.Invoke(instance, this._parameters);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				exception = ex.InnerException;
			}
			return new RemoteHostResponse(this._callId, this._methodId, returnValue, exception);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000C5C70 File Offset: 0x000C3E70
		private object SelectTargetObject(PSHost host)
		{
			if (host == null || host.UI == null)
			{
				return null;
			}
			if (this._methodInfo.InterfaceType == typeof(PSHost))
			{
				return host;
			}
			if (this._methodInfo.InterfaceType == typeof(IHostSupportsInteractiveSession))
			{
				return host;
			}
			if (this._methodInfo.InterfaceType == typeof(PSHostUserInterface))
			{
				return host.UI;
			}
			if (this._methodInfo.InterfaceType == typeof(IHostUISupportsMultipleChoiceSelection))
			{
				return host.UI;
			}
			if (this._methodInfo.InterfaceType == typeof(PSHostRawUserInterface))
			{
				return host.UI.RawUI;
			}
			throw RemoteHostExceptions.NewUnknownTargetClassException(this._methodInfo.InterfaceType.ToString());
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000C5D49 File Offset: 0x000C3F49
		internal bool IsSetShouldExit
		{
			get
			{
				return this._methodId == RemoteHostMethodId.SetShouldExit;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x000C5D54 File Offset: 0x000C3F54
		internal bool IsSetShouldExitOrPopRunspace
		{
			get
			{
				return this._methodId == RemoteHostMethodId.SetShouldExit || this._methodId == RemoteHostMethodId.PopRunspace;
			}
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000C5D6C File Offset: 0x000C3F6C
		internal Collection<RemoteHostCall> PerformSecurityChecksOnHostMessage(string computerName)
		{
			this._computerName = computerName;
			Collection<RemoteHostCall> collection = new Collection<RemoteHostCall>();
			if (this._methodId == RemoteHostMethodId.PromptForCredential1 || this._methodId == RemoteHostMethodId.PromptForCredential2)
			{
				string text = this.ModifyCaption((string)this._parameters[0]);
				string text2 = this.ModifyMessage((string)this._parameters[1], computerName);
				this._parameters[0] = text;
				this._parameters[1] = text2;
			}
			else if (this._methodId == RemoteHostMethodId.Prompt)
			{
				if (this._parameters.Length == 3)
				{
					Collection<FieldDescription> collection2 = (Collection<FieldDescription>)this._parameters[2];
					bool flag = false;
					foreach (FieldDescription fieldDescription in collection2)
					{
						fieldDescription.IsFromRemoteHost = true;
						Type fieldType = InternalHostUserInterface.GetFieldType(fieldDescription);
						if (fieldType != null)
						{
							if (fieldType == typeof(PSCredential))
							{
								flag = true;
								fieldDescription.ModifiedByRemotingProtocol = true;
							}
							else if (fieldType == typeof(SecureString))
							{
								collection.Add(this.ConstructWarningMessageForSecureString(computerName, RemotingErrorIdStrings.RemoteHostPromptSecureStringPrompt));
							}
						}
					}
					if (flag)
					{
						string text3 = this.ModifyCaption((string)this._parameters[0]);
						string text4 = this.ModifyMessage((string)this._parameters[1], computerName);
						this._parameters[0] = text3;
						this._parameters[1] = text4;
					}
				}
			}
			else if (this._methodId == RemoteHostMethodId.ReadLineAsSecureString)
			{
				collection.Add(this.ConstructWarningMessageForSecureString(computerName, RemotingErrorIdStrings.RemoteHostReadLineAsSecureStringPrompt));
			}
			else if (this._methodId == RemoteHostMethodId.GetBufferContents)
			{
				collection.Add(this.ConstructWarningMessageForGetBufferContents(computerName));
			}
			return collection;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000C5F1C File Offset: 0x000C411C
		private string ModifyCaption(string caption)
		{
			string promptForCredential_DefaultCaption = CredUI.PromptForCredential_DefaultCaption;
			if (!caption.Equals(promptForCredential_DefaultCaption, StringComparison.OrdinalIgnoreCase))
			{
				return PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostPromptForCredentialModifiedCaption, new object[]
				{
					caption
				});
			}
			return caption;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000C5F54 File Offset: 0x000C4154
		private string ModifyMessage(string message, string computerName)
		{
			return PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostPromptForCredentialModifiedMessage, new object[]
			{
				computerName.ToUpper(),
				message
			});
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000C5F84 File Offset: 0x000C4184
		private RemoteHostCall ConstructWarningMessageForSecureString(string computerName, string resourceString)
		{
			string text = PSRemotingErrorInvariants.FormatResourceString(resourceString, new object[]
			{
				computerName.ToUpper()
			});
			return new RemoteHostCall(-100L, RemoteHostMethodId.WriteWarningLine, new object[]
			{
				text
			});
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000C5FC0 File Offset: 0x000C41C0
		private RemoteHostCall ConstructWarningMessageForGetBufferContents(string computerName)
		{
			string text = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteHostGetBufferContents, new object[]
			{
				computerName.ToUpper()
			});
			return new RemoteHostCall(-100L, RemoteHostMethodId.WriteWarningLine, new object[]
			{
				text
			});
		}

		// Token: 0x0400111C RID: 4380
		private RemoteHostMethodId _methodId;

		// Token: 0x0400111D RID: 4381
		private object[] _parameters;

		// Token: 0x0400111E RID: 4382
		private RemoteHostMethodInfo _methodInfo;

		// Token: 0x0400111F RID: 4383
		private long _callId;

		// Token: 0x04001120 RID: 4384
		private string _computerName;
	}
}
