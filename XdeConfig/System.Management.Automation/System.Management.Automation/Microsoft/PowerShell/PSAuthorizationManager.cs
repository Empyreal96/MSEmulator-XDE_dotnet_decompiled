using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Microsoft.PowerShell
{
	// Token: 0x02000811 RID: 2065
	public sealed class PSAuthorizationManager : AuthorizationManager
	{
		// Token: 0x06004F8E RID: 20366 RVA: 0x001A609E File Offset: 0x001A429E
		public PSAuthorizationManager(string shellId) : base(shellId)
		{
			if (string.IsNullOrEmpty(shellId))
			{
				throw PSTraceSource.NewArgumentNullException("shellId");
			}
			this.shellId = shellId;
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001A60C4 File Offset: 0x001A42C4
		private static bool IsSupportedExtension(string ext)
		{
			return ext.Equals(".ps1", StringComparison.OrdinalIgnoreCase) || ext.Equals(".ps1xml", StringComparison.OrdinalIgnoreCase) || ext.Equals(".psm1", StringComparison.OrdinalIgnoreCase) || ext.Equals(".psd1", StringComparison.OrdinalIgnoreCase) || ext.Equals(".xaml", StringComparison.OrdinalIgnoreCase) || ext.Equals(".cdxml", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x001A6128 File Offset: 0x001A4328
		private bool CheckPolicy(ExternalScriptInfo script, PSHost host, out Exception reason)
		{
			bool flag = false;
			reason = null;
			string path = script.Path;
			if (path.IndexOf('\\') < 0)
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (path.LastIndexOf('\\') == path.Length - 1)
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists)
			{
				reason = new FileNotFoundException(path);
				return false;
			}
			if (!PSAuthorizationManager.IsSupportedExtension(fileInfo.Extension))
			{
				return true;
			}
			if (SecuritySupport.IsProductBinary(path))
			{
				return true;
			}
			this.executionPolicy = SecuritySupport.GetExecutionPolicy(this.shellId);
			if (this.executionPolicy == ExecutionPolicy.Bypass)
			{
				return true;
			}
			if (SystemPolicy.GetSystemLockdownPolicy() != SystemEnforcementMode.Enforce)
			{
				SaferPolicy saferPolicy = SaferPolicy.Disallowed;
				int num = 0;
				bool flag2 = false;
				while (!flag2 && num < 5)
				{
					try
					{
						saferPolicy = SecuritySupport.GetSaferPolicy(path, null);
						flag2 = true;
					}
					catch (Win32Exception)
					{
						if (num > 4)
						{
							throw;
						}
						num++;
						Thread.Sleep(100);
					}
				}
				if (saferPolicy == SaferPolicy.Disallowed)
				{
					string message = StringUtil.Format(Authenticode.Reason_DisallowedBySafer, path);
					reason = new UnauthorizedAccessException(message);
					return false;
				}
			}
			if (this.executionPolicy == ExecutionPolicy.Unrestricted)
			{
				if (!this.IsLocalFile(fileInfo.FullName))
				{
					if (string.IsNullOrEmpty(script.ScriptContents))
					{
						string message = StringUtil.Format(Authenticode.Reason_FileContentUnavailable, path);
						reason = new UnauthorizedAccessException(message);
						return false;
					}
					Signature signatureWithEncodingRetry = this.GetSignatureWithEncodingRetry(path, script);
					if (signatureWithEncodingRetry.Status == SignatureStatus.Valid && this.IsTrustedPublisher(signatureWithEncodingRetry, path))
					{
						flag = true;
					}
					if (!flag)
					{
						PSAuthorizationManager.RunPromptDecision runPromptDecision;
						do
						{
							runPromptDecision = this.RemoteFilePrompt(path, host);
							if (runPromptDecision == PSAuthorizationManager.RunPromptDecision.Suspend)
							{
								host.EnterNestedPrompt();
							}
						}
						while (runPromptDecision == PSAuthorizationManager.RunPromptDecision.Suspend);
						switch (runPromptDecision)
						{
						case PSAuthorizationManager.RunPromptDecision.RunOnce:
							return true;
						}
						flag = false;
						string message = StringUtil.Format(Authenticode.Reason_DoNotRun, path);
						reason = new UnauthorizedAccessException(message);
					}
				}
				else
				{
					flag = true;
				}
			}
			else if (this.IsLocalFile(fileInfo.FullName) && this.executionPolicy == ExecutionPolicy.RemoteSigned)
			{
				flag = true;
			}
			else if (this.executionPolicy == ExecutionPolicy.AllSigned || this.executionPolicy == ExecutionPolicy.RemoteSigned)
			{
				if (string.IsNullOrEmpty(script.ScriptContents))
				{
					string message = StringUtil.Format(Authenticode.Reason_FileContentUnavailable, path);
					reason = new UnauthorizedAccessException(message);
					return false;
				}
				Signature signatureWithEncodingRetry2 = this.GetSignatureWithEncodingRetry(path, script);
				if (signatureWithEncodingRetry2.Status == SignatureStatus.Valid)
				{
					flag = (this.IsTrustedPublisher(signatureWithEncodingRetry2, path) || this.SetPolicyFromAuthenticodePrompt(path, host, ref reason, signatureWithEncodingRetry2));
				}
				else
				{
					flag = false;
					if (signatureWithEncodingRetry2.Status == SignatureStatus.NotTrusted)
					{
						reason = new UnauthorizedAccessException(StringUtil.Format(Authenticode.Reason_NotTrusted, path, signatureWithEncodingRetry2.SignerCertificate.SubjectName.Name));
					}
					else
					{
						reason = new UnauthorizedAccessException(StringUtil.Format(Authenticode.Reason_Unknown, path, signatureWithEncodingRetry2.StatusMessage));
					}
				}
			}
			else
			{
				flag = false;
				bool flag3 = false;
				if (string.Equals(fileInfo.Extension, ".ps1xml", StringComparison.OrdinalIgnoreCase))
				{
					string[] array = new string[]
					{
						Environment.GetFolderPath(Environment.SpecialFolder.System),
						Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
					};
					foreach (string value in array)
					{
						if (fileInfo.FullName.StartsWith(value, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						Signature signatureWithEncodingRetry3 = this.GetSignatureWithEncodingRetry(path, script);
						if (signatureWithEncodingRetry3.Status == SignatureStatus.Valid)
						{
							if (this.IsTrustedPublisher(signatureWithEncodingRetry3, path))
							{
								flag = true;
							}
							else
							{
								flag = this.SetPolicyFromAuthenticodePrompt(path, host, ref reason, signatureWithEncodingRetry3);
								flag3 = true;
							}
						}
					}
				}
				if (!flag && !flag3)
				{
					reason = new UnauthorizedAccessException(StringUtil.Format(Authenticode.Reason_RestrictedMode, path));
				}
			}
			return flag;
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001A648C File Offset: 0x001A468C
		private bool SetPolicyFromAuthenticodePrompt(string path, PSHost host, ref Exception reason, Signature signature)
		{
			bool result = false;
			switch (this.AuthenticodePrompt(path, signature, host))
			{
			case PSAuthorizationManager.RunPromptDecision.NeverRun:
			{
				this.UntrustPublisher(signature);
				string message = StringUtil.Format(Authenticode.Reason_NeverRun, path);
				reason = new UnauthorizedAccessException(message);
				result = false;
				break;
			}
			case PSAuthorizationManager.RunPromptDecision.DoNotRun:
			{
				result = false;
				string message = StringUtil.Format(Authenticode.Reason_DoNotRun, path);
				reason = new UnauthorizedAccessException(message);
				break;
			}
			case PSAuthorizationManager.RunPromptDecision.RunOnce:
				result = true;
				break;
			case PSAuthorizationManager.RunPromptDecision.AlwaysRun:
				this.TrustPublisher(signature);
				result = true;
				break;
			}
			return result;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x001A6508 File Offset: 0x001A4708
		private bool IsLocalFile(string filename)
		{
			SecurityZone fileSecurityZone = ClrFacade.GetFileSecurityZone(filename);
			return fileSecurityZone == SecurityZone.MyComputer || fileSecurityZone == SecurityZone.Intranet || fileSecurityZone == SecurityZone.Trusted;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001A652C File Offset: 0x001A472C
		private bool IsTrustedPublisher(Signature signature, string file)
		{
			X509Certificate2 signerCertificate = signature.SignerCertificate;
			string thumbprint = signerCertificate.Thumbprint;
			X509Store x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.ReadOnly);
			foreach (X509Certificate2 x509Certificate in x509Store.Certificates)
			{
				if (string.Equals(x509Certificate.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase) && !this.IsUntrustedPublisher(signature, file))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001A6598 File Offset: 0x001A4798
		private bool IsUntrustedPublisher(Signature signature, string file)
		{
			X509Certificate2 signerCertificate = signature.SignerCertificate;
			string thumbprint = signerCertificate.Thumbprint;
			X509Store x509Store = new X509Store(StoreName.Disallowed, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.ReadOnly);
			foreach (X509Certificate2 x509Certificate in x509Store.Certificates)
			{
				if (string.Equals(x509Certificate.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001A65FC File Offset: 0x001A47FC
		private void TrustPublisher(Signature signature)
		{
			X509Certificate2 signerCertificate = signature.SignerCertificate;
			X509Store x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.CurrentUser);
			try
			{
				x509Store.Open(OpenFlags.ReadWrite);
				x509Store.Add(signerCertificate);
			}
			finally
			{
				x509Store.Close();
			}
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001A6640 File Offset: 0x001A4840
		private void UntrustPublisher(Signature signature)
		{
			X509Certificate2 signerCertificate = signature.SignerCertificate;
			X509Store x509Store = new X509Store(StoreName.Disallowed, StoreLocation.CurrentUser);
			X509Store x509Store2 = new X509Store(StoreName.TrustedPublisher, StoreLocation.CurrentUser);
			try
			{
				x509Store2.Open(OpenFlags.ReadWrite);
				x509Store2.Remove(signerCertificate);
			}
			finally
			{
				x509Store2.Close();
			}
			try
			{
				x509Store.Open(OpenFlags.ReadWrite);
				x509Store.Add(signerCertificate);
			}
			finally
			{
				x509Store.Close();
			}
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001A66B0 File Offset: 0x001A48B0
		private Signature GetSignatureWithEncodingRetry(string path, ExternalScriptInfo script)
		{
			string fileContent = Encoding.Unicode.GetString(script.OriginalEncoding.GetPreamble()) + script.ScriptContents;
			Signature signature = SignatureHelper.GetSignature(path, fileContent);
			if (signature.Status != SignatureStatus.Valid && script.OriginalEncoding != Encoding.Unicode)
			{
				fileContent = Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble()) + script.ScriptContents;
				Signature signature2 = SignatureHelper.GetSignature(path, fileContent);
				if (signature2.Status == SignatureStatus.Valid)
				{
					signature = signature2;
				}
			}
			return signature;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x001A6730 File Offset: 0x001A4930
		protected internal override bool ShouldRun(CommandInfo commandInfo, CommandOrigin origin, PSHost host, out Exception reason)
		{
			bool result = false;
			reason = null;
			Utils.CheckArgForNull(commandInfo, "commandInfo");
			Utils.CheckArgForNullOrEmpty(commandInfo.Name, "commandInfo.Name");
			CommandTypes commandType = commandInfo.CommandType;
			if (commandType <= CommandTypes.ExternalScript)
			{
				switch (commandType)
				{
				case CommandTypes.Alias:
					return true;
				case CommandTypes.Function:
				case CommandTypes.Filter:
					break;
				case CommandTypes.Alias | CommandTypes.Function:
					return result;
				default:
				{
					if (commandType == CommandTypes.Cmdlet)
					{
						return true;
					}
					if (commandType != CommandTypes.ExternalScript)
					{
						return result;
					}
					ExternalScriptInfo externalScriptInfo = commandInfo as ExternalScriptInfo;
					if (externalScriptInfo == null)
					{
						reason = PSTraceSource.NewArgumentException("scriptInfo");
						return result;
					}
					return this.CheckPolicy(externalScriptInfo, host, out reason);
				}
				}
			}
			else if (commandType <= CommandTypes.Script)
			{
				if (commandType == CommandTypes.Application)
				{
					return true;
				}
				if (commandType != CommandTypes.Script)
				{
					return result;
				}
				return true;
			}
			else if (commandType != CommandTypes.Workflow && commandType != CommandTypes.Configuration)
			{
				return result;
			}
			result = true;
			return result;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x001A67EC File Offset: 0x001A49EC
		private PSAuthorizationManager.RunPromptDecision AuthenticodePrompt(string path, Signature signature, PSHost host)
		{
			if (host == null || host.UI == null)
			{
				return PSAuthorizationManager.RunPromptDecision.DoNotRun;
			}
			PSAuthorizationManager.RunPromptDecision result = PSAuthorizationManager.RunPromptDecision.DoNotRun;
			if (signature == null)
			{
				return result;
			}
			switch (signature.Status)
			{
			case SignatureStatus.Valid:
			{
				Collection<ChoiceDescription> authenticodePromptChoices = this.GetAuthenticodePromptChoices();
				string authenticodePromptCaption = Authenticode.AuthenticodePromptCaption;
				string message;
				if (signature.SignerCertificate == null)
				{
					message = StringUtil.Format(Authenticode.AuthenticodePromptText_UnknownPublisher, path);
				}
				else
				{
					message = StringUtil.Format(Authenticode.AuthenticodePromptText, path, signature.SignerCertificate.SubjectName.Name);
				}
				return (PSAuthorizationManager.RunPromptDecision)host.UI.PromptForChoice(authenticodePromptCaption, message, authenticodePromptChoices, 1);
			}
			case SignatureStatus.UnknownError:
			case SignatureStatus.NotSigned:
			case SignatureStatus.HashMismatch:
			case SignatureStatus.NotSupportedFileFormat:
				return PSAuthorizationManager.RunPromptDecision.DoNotRun;
			}
			return PSAuthorizationManager.RunPromptDecision.DoNotRun;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x001A6894 File Offset: 0x001A4A94
		private PSAuthorizationManager.RunPromptDecision RemoteFilePrompt(string path, PSHost host)
		{
			if (host == null || host.UI == null)
			{
				return PSAuthorizationManager.RunPromptDecision.DoNotRun;
			}
			Collection<ChoiceDescription> remoteFilePromptChoices = this.GetRemoteFilePromptChoices();
			string remoteFilePromptCaption = Authenticode.RemoteFilePromptCaption;
			string message = StringUtil.Format(Authenticode.RemoteFilePromptText, path);
			switch (host.UI.PromptForChoice(remoteFilePromptCaption, message, remoteFilePromptChoices, 0))
			{
			case 0:
				return PSAuthorizationManager.RunPromptDecision.DoNotRun;
			case 1:
				return PSAuthorizationManager.RunPromptDecision.RunOnce;
			case 2:
				return PSAuthorizationManager.RunPromptDecision.Suspend;
			default:
				return PSAuthorizationManager.RunPromptDecision.DoNotRun;
			}
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x001A68F8 File Offset: 0x001A4AF8
		private Collection<ChoiceDescription> GetAuthenticodePromptChoices()
		{
			Collection<ChoiceDescription> collection = new Collection<ChoiceDescription>();
			string choice_NeverRun = Authenticode.Choice_NeverRun;
			string choice_NeverRun_Help = Authenticode.Choice_NeverRun_Help;
			string choice_DoNotRun = Authenticode.Choice_DoNotRun;
			string choice_DoNotRun_Help = Authenticode.Choice_DoNotRun_Help;
			string choice_RunOnce = Authenticode.Choice_RunOnce;
			string choice_RunOnce_Help = Authenticode.Choice_RunOnce_Help;
			string choice_AlwaysRun = Authenticode.Choice_AlwaysRun;
			string choice_AlwaysRun_Help = Authenticode.Choice_AlwaysRun_Help;
			collection.Add(new ChoiceDescription(choice_NeverRun, choice_NeverRun_Help));
			collection.Add(new ChoiceDescription(choice_DoNotRun, choice_DoNotRun_Help));
			collection.Add(new ChoiceDescription(choice_RunOnce, choice_RunOnce_Help));
			collection.Add(new ChoiceDescription(choice_AlwaysRun, choice_AlwaysRun_Help));
			return collection;
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x001A697C File Offset: 0x001A4B7C
		private Collection<ChoiceDescription> GetRemoteFilePromptChoices()
		{
			Collection<ChoiceDescription> collection = new Collection<ChoiceDescription>();
			string choice_DoNotRun = Authenticode.Choice_DoNotRun;
			string choice_DoNotRun_Help = Authenticode.Choice_DoNotRun_Help;
			string choice_RunOnce = Authenticode.Choice_RunOnce;
			string choice_RunOnce_Help = Authenticode.Choice_RunOnce_Help;
			string choice_Suspend = Authenticode.Choice_Suspend;
			string choice_Suspend_Help = Authenticode.Choice_Suspend_Help;
			collection.Add(new ChoiceDescription(choice_DoNotRun, choice_DoNotRun_Help));
			collection.Add(new ChoiceDescription(choice_RunOnce, choice_RunOnce_Help));
			collection.Add(new ChoiceDescription(choice_Suspend, choice_Suspend_Help));
			return collection;
		}

		// Token: 0x040028AE RID: 10414
		private ExecutionPolicy executionPolicy;

		// Token: 0x040028AF RID: 10415
		private string shellId;

		// Token: 0x02000812 RID: 2066
		internal enum RunPromptDecision
		{
			// Token: 0x040028B1 RID: 10417
			NeverRun,
			// Token: 0x040028B2 RID: 10418
			DoNotRun,
			// Token: 0x040028B3 RID: 10419
			RunOnce,
			// Token: 0x040028B4 RID: 10420
			AlwaysRun,
			// Token: 0x040028B5 RID: 10421
			Suspend
		}
	}
}
