using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Security;
using System.Text;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200089A RID: 2202
	internal static class PathUtils
	{
		// Token: 0x0600545F RID: 21599 RVA: 0x001BD75C File Offset: 0x001BB95C
		internal static void MasterStreamOpen(PSCmdlet cmdlet, string filePath, string encoding, bool defaultEncoding, bool Append, bool Force, bool NoClobber, out FileStream fileStream, out StreamWriter streamWriter, out FileInfo readOnlyFileInfo, bool isLiteralPath)
		{
			Encoding resolvedEncoding = EncodingConversion.Convert(cmdlet, encoding);
			PathUtils.MasterStreamOpen(cmdlet, filePath, resolvedEncoding, defaultEncoding, Append, Force, NoClobber, out fileStream, out streamWriter, out readOnlyFileInfo, isLiteralPath);
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x001BD788 File Offset: 0x001BB988
		internal static void MasterStreamOpen(PSCmdlet cmdlet, string filePath, Encoding resolvedEncoding, bool defaultEncoding, bool Append, bool Force, bool NoClobber, out FileStream fileStream, out StreamWriter streamWriter, out FileInfo readOnlyFileInfo, bool isLiteralPath)
		{
			fileStream = null;
			streamWriter = null;
			readOnlyFileInfo = null;
			string text = PathUtils.ResolveFilePath(filePath, cmdlet, isLiteralPath);
			try
			{
				FileMode mode = FileMode.Create;
				if (Append)
				{
					mode = FileMode.Append;
				}
				else if (NoClobber)
				{
					mode = FileMode.CreateNew;
				}
				if (Force && (Append || !NoClobber) && File.Exists(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						readOnlyFileInfo = fileInfo;
						fileInfo.Attributes &= ~FileAttributes.ReadOnly;
					}
				}
				FileShare share = Force ? FileShare.ReadWrite : FileShare.Read;
				fileStream = new FileStream(text, mode, FileAccess.Write, share);
				if (defaultEncoding)
				{
					streamWriter = new StreamWriter(fileStream);
				}
				else
				{
					streamWriter = new StreamWriter(fileStream, resolvedEncoding);
				}
			}
			catch (ArgumentException e)
			{
				PathUtils.ReportFileOpenFailure(cmdlet, text, e);
			}
			catch (IOException ex)
			{
				if (NoClobber && File.Exists(text))
				{
					cmdlet.ThrowTerminatingError(new ErrorRecord(ex, "NoClobber", ErrorCategory.ResourceExists, text)
					{
						ErrorDetails = new ErrorDetails(cmdlet, "PathUtilsStrings", "UtilityFileExistsNoClobber", new object[]
						{
							filePath,
							"NoClobber"
						})
					});
				}
				PathUtils.ReportFileOpenFailure(cmdlet, text, ex);
			}
			catch (UnauthorizedAccessException e2)
			{
				PathUtils.ReportFileOpenFailure(cmdlet, text, e2);
			}
			catch (NotSupportedException e3)
			{
				PathUtils.ReportFileOpenFailure(cmdlet, text, e3);
			}
			catch (SecurityException e4)
			{
				PathUtils.ReportFileOpenFailure(cmdlet, text, e4);
			}
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x001BD904 File Offset: 0x001BBB04
		internal static void ReportFileOpenFailure(Cmdlet cmdlet, string filePath, Exception e)
		{
			ErrorRecord errorRecord = new ErrorRecord(e, "FileOpenFailure", ErrorCategory.OpenError, null);
			cmdlet.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x001BD928 File Offset: 0x001BBB28
		internal static StreamReader OpenStreamReader(PSCmdlet command, string filePath, string encoding, bool isLiteralPath)
		{
			FileStream stream = PathUtils.OpenFileStream(filePath, command, isLiteralPath);
			if (encoding == null)
			{
				return new StreamReader(stream);
			}
			return new StreamReader(stream, EncodingConversion.Convert(command, encoding));
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x001BD958 File Offset: 0x001BBB58
		internal static FileStream OpenFileStream(string filePath, PSCmdlet command, bool isLiteralPath)
		{
			string path = PathUtils.ResolveFilePath(filePath, command, isLiteralPath);
			FileStream result;
			try
			{
				result = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			catch (ArgumentException e)
			{
				PathUtils.ReportFileOpenFailure(command, filePath, e);
				result = null;
			}
			catch (IOException e2)
			{
				PathUtils.ReportFileOpenFailure(command, filePath, e2);
				result = null;
			}
			catch (UnauthorizedAccessException e3)
			{
				PathUtils.ReportFileOpenFailure(command, filePath, e3);
				result = null;
			}
			catch (NotSupportedException e4)
			{
				PathUtils.ReportFileOpenFailure(command, filePath, e4);
				result = null;
			}
			catch (DriveNotFoundException e5)
			{
				PathUtils.ReportFileOpenFailure(command, filePath, e5);
				result = null;
			}
			return result;
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x001BDA08 File Offset: 0x001BBC08
		internal static string ResolveFilePath(string filePath, PSCmdlet command)
		{
			return PathUtils.ResolveFilePath(filePath, command, false);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x001BDA14 File Offset: 0x001BBC14
		internal static string ResolveFilePath(string filePath, PSCmdlet command, bool isLiteralPath)
		{
			string text = null;
			try
			{
				ProviderInfo providerInfo = null;
				PSDriveInfo psdriveInfo = null;
				List<string> list = new List<string>();
				if (isLiteralPath)
				{
					list.Add(command.SessionState.Path.GetUnresolvedProviderPathFromPSPath(filePath, out providerInfo, out psdriveInfo));
				}
				else
				{
					list.AddRange(command.SessionState.Path.GetResolvedProviderPathFromPSPath(filePath, out providerInfo));
				}
				if (!providerInfo.NameEquals(command.Context.ProviderNames.FileSystem))
				{
					PathUtils.ReportWrongProviderType(command, providerInfo.FullName);
				}
				if (list.Count > 1)
				{
					PathUtils.ReportMultipleFilesNotSupported(command);
				}
				if (list.Count == 0)
				{
					PathUtils.ReportWildcardingFailure(command, filePath);
				}
				text = list[0];
			}
			catch (ItemNotFoundException)
			{
				text = null;
			}
			if (string.IsNullOrEmpty(text))
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(command);
				ProviderInfo providerInfo2 = null;
				PSDriveInfo psdriveInfo2 = null;
				text = command.SessionState.Path.GetUnresolvedProviderPathFromPSPath(filePath, cmdletProviderContext, out providerInfo2, out psdriveInfo2);
				cmdletProviderContext.ThrowFirstErrorOrDoNothing();
				if (!providerInfo2.NameEquals(command.Context.ProviderNames.FileSystem))
				{
					PathUtils.ReportWrongProviderType(command, providerInfo2.FullName);
				}
			}
			return text;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x001BDB24 File Offset: 0x001BBD24
		internal static void ReportWrongProviderType(Cmdlet cmdlet, string providerId)
		{
			string message = StringUtil.Format(PathUtilsStrings.OutFile_ReadWriteFileNotFileSystemProvider, providerId);
			cmdlet.ThrowTerminatingError(new ErrorRecord(PSTraceSource.NewInvalidOperationException(), "ReadWriteFileNotFileSystemProvider", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x001BDB64 File Offset: 0x001BBD64
		internal static void ReportMultipleFilesNotSupported(Cmdlet cmdlet)
		{
			string message = StringUtil.Format(PathUtilsStrings.OutFile_MultipleFilesNotSupported, new object[0]);
			cmdlet.ThrowTerminatingError(new ErrorRecord(PSTraceSource.NewInvalidOperationException(), "ReadWriteMultipleFilesNotSupported", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x001BDBA8 File Offset: 0x001BBDA8
		internal static void ReportWildcardingFailure(Cmdlet cmdlet, string filePath)
		{
			string message = StringUtil.Format(PathUtilsStrings.OutFile_DidNotResolveFile, filePath);
			cmdlet.ThrowTerminatingError(new ErrorRecord(new FileNotFoundException(), "FileOpenFailure", ErrorCategory.OpenError, filePath)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x001BDBE8 File Offset: 0x001BBDE8
		internal static DirectoryInfo CreateModuleDirectory(PSCmdlet cmdlet, string moduleNameOrPath, bool force)
		{
			DirectoryInfo directoryInfo = null;
			try
			{
				string text = ModuleCmdletBase.ResolveRootedFilePath(moduleNameOrPath, cmdlet.Context);
				if (string.IsNullOrEmpty(text) && moduleNameOrPath.StartsWith(".", StringComparison.OrdinalIgnoreCase))
				{
					PathInfo pathInfo = cmdlet.CurrentProviderLocation(cmdlet.Context.ProviderNames.FileSystem);
					text = Path.Combine(pathInfo.ProviderPath, moduleNameOrPath);
				}
				if (string.IsNullOrEmpty(text))
				{
					string personalModulePath = ModuleIntrinsics.GetPersonalModulePath();
					text = Path.Combine(personalModulePath, moduleNameOrPath);
				}
				directoryInfo = new DirectoryInfo(text);
				if (directoryInfo.Exists)
				{
					if (!force)
					{
						string message = string.Format(CultureInfo.InvariantCulture, PathUtilsStrings.ExportPSSession_ErrorDirectoryExists, new object[]
						{
							directoryInfo.FullName
						});
						ErrorDetails errorDetails = new ErrorDetails(message);
						ErrorRecord errorRecord = new ErrorRecord(new ArgumentException(errorDetails.Message), "ExportProxyCommand_OutputDirectoryExists", ErrorCategory.ResourceExists, directoryInfo);
						cmdlet.ThrowTerminatingError(errorRecord);
					}
				}
				else
				{
					directoryInfo.Create();
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				string message2 = string.Format(CultureInfo.InvariantCulture, PathUtilsStrings.ExportPSSession_CannotCreateOutputDirectory, new object[]
				{
					moduleNameOrPath,
					ex.Message
				});
				ErrorDetails errorDetails2 = new ErrorDetails(message2);
				ErrorRecord errorRecord2 = new ErrorRecord(new ArgumentException(errorDetails2.Message, ex), "ExportProxyCommand_CannotCreateOutputDirectory", ErrorCategory.ResourceExists, moduleNameOrPath);
				cmdlet.ThrowTerminatingError(errorRecord2);
			}
			return directoryInfo;
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x001BDD38 File Offset: 0x001BBF38
		internal static DirectoryInfo CreateTemporaryDirectory()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetTempPath());
			DirectoryInfo directoryInfo2;
			do
			{
				directoryInfo2 = new DirectoryInfo(Path.Combine(directoryInfo.FullName, string.Format(null, "tmp_{0}", new object[]
				{
					Path.GetRandomFileName()
				})));
			}
			while (directoryInfo2.Exists);
			Directory.CreateDirectory(directoryInfo2.FullName);
			return new DirectoryInfo(directoryInfo2.FullName);
		}
	}
}
