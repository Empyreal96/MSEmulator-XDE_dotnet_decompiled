using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal.Host;
using System.Security;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000308 RID: 776
	internal class ServerRemoteHostUserInterface : PSHostUserInterface, IHostUISupportsMultipleChoiceSelection
	{
		// Token: 0x060024AD RID: 9389 RVA: 0x000CDAB4 File Offset: 0x000CBCB4
		internal ServerRemoteHostUserInterface(ServerRemoteHost remoteHost)
		{
			this._remoteHost = remoteHost;
			this._serverMethodExecutor = remoteHost.ServerMethodExecutor;
			this._rawUI = (remoteHost.HostInfo.IsHostRawUINull ? null : new ServerRemoteHostRawUserInterface(this));
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x000CDAEB File Offset: 0x000CBCEB
		public override PSHostRawUserInterface RawUI
		{
			get
			{
				return this._rawUI;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000CDAF3 File Offset: 0x000CBCF3
		internal ServerRemoteHost ServerRemoteHost
		{
			get
			{
				return this._remoteHost;
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000CDAFB File Offset: 0x000CBCFB
		public override string ReadLine()
		{
			return this._serverMethodExecutor.ExecuteMethod<string>(RemoteHostMethodId.ReadLine);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000CDB0C File Offset: 0x000CBD0C
		public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
		{
			return this._serverMethodExecutor.ExecuteMethod<int>(RemoteHostMethodId.PromptForChoice, new object[]
			{
				caption,
				message,
				choices,
				defaultChoice
			});
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000CDB44 File Offset: 0x000CBD44
		public Collection<int> PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, IEnumerable<int> defaultChoices)
		{
			return this._serverMethodExecutor.ExecuteMethod<Collection<int>>(RemoteHostMethodId.PromptForChoiceMultipleSelection, new object[]
			{
				caption,
				message,
				choices,
				defaultChoices
			});
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000CDB78 File Offset: 0x000CBD78
		public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
		{
			Dictionary<string, PSObject> dictionary = this._serverMethodExecutor.ExecuteMethod<Dictionary<string, PSObject>>(RemoteHostMethodId.Prompt, new object[]
			{
				caption,
				message,
				descriptions
			});
			foreach (FieldDescription fieldDescription in descriptions)
			{
				Type fieldType = InternalHostUserInterface.GetFieldType(fieldDescription);
				PSObject valueToConvert;
				object obj;
				if (fieldType != null && dictionary.TryGetValue(fieldDescription.Name, out valueToConvert) && LanguagePrimitives.TryConvertTo(valueToConvert, fieldType, CultureInfo.InvariantCulture, out obj))
				{
					if (obj != null)
					{
						dictionary[fieldDescription.Name] = PSObject.AsPSObject(obj);
					}
					else
					{
						dictionary[fieldDescription.Name] = null;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000CDC3C File Offset: 0x000CBE3C
		public override void Write(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.Write1, new object[]
			{
				message
			});
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000CDC64 File Offset: 0x000CBE64
		public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.Write2, new object[]
			{
				foregroundColor,
				backgroundColor,
				message
			});
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000CDC9C File Offset: 0x000CBE9C
		public override void WriteLine()
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteLine1);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000CDCAC File Offset: 0x000CBEAC
		public override void WriteLine(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteLine2, new object[]
			{
				message
			});
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000CDCD4 File Offset: 0x000CBED4
		public override void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteLine3, new object[]
			{
				foregroundColor,
				backgroundColor,
				message
			});
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000CDD0C File Offset: 0x000CBF0C
		public override void WriteErrorLine(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteErrorLine, new object[]
			{
				message
			});
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000CDD34 File Offset: 0x000CBF34
		public override void WriteDebugLine(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteDebugLine, new object[]
			{
				message
			});
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000CDD5C File Offset: 0x000CBF5C
		public override void WriteProgress(long sourceId, ProgressRecord record)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteProgress, new object[]
			{
				sourceId,
				record
			});
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000CDD8C File Offset: 0x000CBF8C
		public override void WriteVerboseLine(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteVerboseLine, new object[]
			{
				message
			});
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000CDDB4 File Offset: 0x000CBFB4
		public override void WriteWarningLine(string message)
		{
			this._serverMethodExecutor.ExecuteVoidMethod(RemoteHostMethodId.WriteWarningLine, new object[]
			{
				message
			});
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000CDDDA File Offset: 0x000CBFDA
		public override SecureString ReadLineAsSecureString()
		{
			return this._serverMethodExecutor.ExecuteMethod<SecureString>(RemoteHostMethodId.ReadLineAsSecureString);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000CDDEC File Offset: 0x000CBFEC
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
		{
			return this._serverMethodExecutor.ExecuteMethod<PSCredential>(RemoteHostMethodId.PromptForCredential1, new object[]
			{
				caption,
				message,
				userName,
				targetName
			});
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000CDE20 File Offset: 0x000CC020
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
		{
			return this._serverMethodExecutor.ExecuteMethod<PSCredential>(RemoteHostMethodId.PromptForCredential2, new object[]
			{
				caption,
				message,
				userName,
				targetName,
				allowedCredentialTypes,
				options
			});
		}

		// Token: 0x04001209 RID: 4617
		private PSHostRawUserInterface _rawUI;

		// Token: 0x0400120A RID: 4618
		private ServerRemoteHost _remoteHost;

		// Token: 0x0400120B RID: 4619
		private ServerMethodExecutor _serverMethodExecutor;
	}
}
